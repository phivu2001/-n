using BLL.IServices;
using DTO.Invoice;
using DTO.Stock;
using DTO.Warehouse;
using Entities;
using Entities.Invoice;
using Entities.Stock;
using Entities.Warehouse;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class WarehouseService  : IWarehouseService
    {
        private readonly ApplicationDbContext _dbContext;

        public WarehouseService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region Thông tin hóa đơn
        public List<int> GetInvoiceSendMail()
        {
            //var invoices = _dbContext.Invoice.Where(w => w.CreateAt.Day == DateTime.Now.AddDays(-1).Day);
            var invoices = _dbContext.Invoice.ToList();
            if (invoices != null)
            {
                return invoices.Select(s => (int)s.Id).ToList();
            }
            return new List<int>();
        }
        public InvoiceDTO InVoiceDetail(long id)
        {
            var Invoice = _dbContext.Invoice.FirstOrDefault(x => x.Id == id);
            if (Invoice != null)
            {
                var result = new InvoiceDTO()
                {
                    Id = Invoice.Id,
                    CreateAt = Invoice.CreateAt,
                    VenderId = Invoice.VenderId,
                    Status = Invoice.Status,
                    VendorName = Invoice.VendorName,
                    CustomerId = Invoice.CustomerId,
                    CustomerName = Invoice.CustomerName
                };
                result.InvoiceDetails = _dbContext.InvoiceDetail.Where(x => x.InvoiceId == id).Select(s => new InvoiceDetailDTO()
                {
                    InvoiceId = id,
                    ResourceId = s.ResourceId,
                    StockId = s.StockId,
                    Quantity = s.Quantity,
                    Status = s.Status,
                }).ToList();
                return result;
            }

            return new InvoiceDTO(); ;
        }
        #endregion
        #region Thay đổi trạng thái xuất nhập kho
        public bool ChangeStatusInWarehouse(long inWarehouseId, int status)
        {
            var entity = _dbContext.InWarehouse.FirstOrDefault(f => f.Id == inWarehouseId);
            if (entity != null)
            {
                entity.Status= status;
                _dbContext.SaveChanges();
            }
            return true;
        }
        public bool ChangeStatusOutWarehouse(long outWarehouseId, int status)
        {
            var entity = _dbContext.OutWarehouse.FirstOrDefault(f => f.Id == outWarehouseId);
            if (entity != null)
            {
                entity.Status = status;
                _dbContext.SaveChanges();
            }
            return true;
        }
        #endregion

        #region Tạo hóa đơn mua bán
        public bool PurchaseCreate(long inWarehouseId)
        {
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var inWarehouse = _dbContext.InWarehouse.FirstOrDefault(f => f.Id == inWarehouseId);
                    if (inWarehouse != null)
                    {
                        inWarehouse.Status = 1;
                        InvoiceEntities Invoice = new InvoiceEntities();
                        List<InvoiceDetailEntities> InvoiceDetailS = new List<InvoiceDetailEntities>();
                        Invoice.CreateAt = DateTime.Now;
                        Invoice.ResourceId = inWarehouseId;
                        Invoice.VenderId = inWarehouse.VenderId;
                        Invoice.VendorName = _dbContext.Vendor.FirstOrDefault(f => f.Id == inWarehouse.VenderId)?.FullName;
                        double qty = 0;
                        decimal price = 0;
                        var itemDetails = _dbContext.InWarehouseStock.Where(w => w.InWarehouseId == inWarehouseId).ToList();
                        if (itemDetails != null)
                        {
                            var stock = _dbContext.Stock.AsQueryable();
                            foreach (var item in itemDetails)
                            {
                                decimal priceStock = stock.FirstOrDefault(f => f.Id == item.StockId) != null ? stock.FirstOrDefault(f => f.Id == item.StockId).Price : 0; ;
                                qty = qty + item.Quantity;
                                price = price + Convert.ToDecimal(item.Quantity) * priceStock;
                                InvoiceDetailS.Add(new InvoiceDetailEntities
                                {
                                    ResourceId = inWarehouseId,
                                    StockId = item.StockId,
                                    Status = item.Status,
                                    Quantity = item.Quantity,
                                });
                            }
                        }
                        Invoice.Quantity = qty;
                        Invoice.Price = price;
                        _dbContext.Invoice.Add(Invoice);
                        var result = _dbContext.SaveChanges() > 0;
                        if (result)
                        {
                            InvoiceDetailS.ForEach(f => f.InvoiceId = Invoice.Id);                            
                            _dbContext.InvoiceDetail.AddRange(InvoiceDetailS);
                            _dbContext.SaveChanges();
                        }
                        trans.Commit();
                    }                    
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return false;
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return true;
        }
        public bool SellCreate(long outWarehouseId)
        {
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var outWarehouse = _dbContext.OutWarehouse.FirstOrDefault(f => f.Id == outWarehouseId);
                    if (outWarehouse != null)
                    {
                        outWarehouse.Status = 1;
                        InvoiceEntities Invoice = new InvoiceEntities();
                        List<InvoiceDetailEntities> InvoiceDetailS = new List<InvoiceDetailEntities>();
                        Invoice.CreateAt = DateTime.Now;
                        Invoice.ResourceId = outWarehouseId;
                        Invoice.CustomerId = outWarehouse.CustomerId;
                        Invoice.CustomerName = _dbContext.Customer.FirstOrDefault(f => f.Id == outWarehouse.CustomerId)?.FullName;
                        double qty = 0;
                        decimal price = 0;
                        var itemDetails = _dbContext.OutWarehouseStock.Where(w => w.OutWarehouseId == outWarehouseId).ToList();
                        if (itemDetails != null)
                        {
                            var stock = _dbContext.Stock.AsQueryable();
                            foreach (var item in itemDetails)
                            {
                                decimal priceStock = stock.FirstOrDefault(f => f.Id == item.StockId) != null ? stock.FirstOrDefault(f => f.Id == item.StockId).Price : 0; ;
                                qty = qty + item.Quantity;
                                price = price + Convert.ToDecimal(item.Quantity) * priceStock;
                                InvoiceDetailS.Add(new InvoiceDetailEntities
                                {
                                    ResourceId = outWarehouseId,
                                    StockId = item.StockId,
                                    Status = item.Status,
                                    Quantity = item.Quantity,
                                });
                                var inww = _dbContext.InWarehouseStock.FirstOrDefault(f => f.StockId == item.StockId && f.Status == item.Status);
                                if (inww != null) {
                                    inww.Quantity = inww.Quantity - item.Quantity;
                                    _dbContext.SaveChanges();
                                }
                            }
                        }
                        Invoice.Quantity = qty;
                        Invoice.Price = price;
                        _dbContext.Invoice.Add(Invoice);
                        var result = _dbContext.SaveChanges() > 0;
                        if (result)
                        {
                            InvoiceDetailS.ForEach(f => f.InvoiceId = Invoice.Id);
                            _dbContext.InvoiceDetail.AddRange(InvoiceDetailS);
                            _dbContext.SaveChanges();
                        }
                        trans.Commit();
                    }
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return false;
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return true;
        }
        #endregion
        #region Check out off stock
        public bool CheckOutOffStock(List<OutWarehousDetailDTO> input)
        {
            var result = true;
            var inStock = _dbContext.InWarehouseStock.AsQueryable();
            if (input != null)
            {
                foreach (var item in input)
                {
                    var temp = inStock.Where(w => w.StockId == item.StockId && w.Status == item.Status);
                    var qty = inStock.Where(w => w.StockId == item.StockId && w.Status == item.Status)?.Sum(s => s.Quantity);
                    if (item.Quantity > qty)
                    {
                        result = false;
                    }

                }
            }
            return result;
        }
        #endregion
        #region In stock
        public List<InWarehouseDTO> InWarehouseGetAll()
        {
            var vendor = _dbContext.Vendor.AsQueryable();
            var entity = _dbContext.InWarehouse.AsQueryable();
            return entity.Select(s => new InWarehouseDTO()
            {
                Id = s.Id,
                CreateAt = s.CreateAt,
                VenderId = s.VenderId,
                Status= s.Status,
                VenderName = vendor.FirstOrDefault(f => f.Id == s.VenderId).FullName
            }).ToList();
        }
        public bool InWarehouseCreate(InWarehouseDTO input)
        {
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var inWarehouse = new InWarehouseEntities()
                    {
                        CreateAt = input.CreateAt,
                        Number = input.Number,
                        VenderId = input.VenderId,
                        Status= input.Status,
                    };
                    _dbContext.InWarehouse.Add(inWarehouse);
                    var result = _dbContext.SaveChanges() > 0;
                    if (result)
                    {
                        List<InWarehouseStockEntities> inWarehouseStockEntities = new List<InWarehouseStockEntities>();
                        if (input.InWarehousDetails != null)
                        {
                            foreach (var item in input.InWarehousDetails)
                            {
                                inWarehouseStockEntities.Add(new InWarehouseStockEntities()
                                {
                                    InWarehouseId = inWarehouse.Id,
                                    StockId = item.StockId,
                                    Quantity = item.Quantity,
                                    Status = item.Status
                                });
                            }
                        }
                        _dbContext.InWarehouseStock.AddRange(inWarehouseStockEntities);
                        _dbContext.SaveChanges();
                    }
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return false;
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return true;
        }
        public bool InWarehouseDelete(long id)
        {
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var inWarehouseStocks = _dbContext.InWarehouseStock.Where(x => x.InWarehouseId == id);
                    if (inWarehouseStocks != null)
                    {
                        _dbContext.InWarehouseStock.RemoveRange(inWarehouseStocks);
                        _dbContext.SaveChanges();
                    }
                    var inWarehouse = _dbContext.InWarehouse.FirstOrDefault(x => x.Id == id);
                    if (inWarehouse != null)
                    {
                        _dbContext.InWarehouse.Remove(inWarehouse);
                        _dbContext.SaveChanges();
                    }
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return false;
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return true;            
        }

        public InWarehouseDTO InWarehouseGetById(long id)
        {
            var vendor = _dbContext.Vendor.AsQueryable();
            var inWarehouse = _dbContext.InWarehouse.FirstOrDefault(x => x.Id == id);
            if (inWarehouse != null)
            {
                var result = new InWarehouseDTO()
                {
                    Id = inWarehouse.Id,
                    CreateAt = inWarehouse.CreateAt,
                    VenderId = inWarehouse.VenderId,
                    Status= inWarehouse.Status,
                    VenderName = vendor.FirstOrDefault(f => f.Id == inWarehouse.VenderId)?.FullName
                };
                result.InWarehousDetails = _dbContext.InWarehouseStock.Where(x => x.InWarehouseId == id).Select(s => new InWarehousDetailDTO()
                {
                    InWarehouseId = s.Id,
                    StockId = s.StockId,
                    Quantity = s.Quantity,
                    Status = s.Status,
                    Stocks = _dbContext.Stock.Select(z => new StockDTO() { Id = z.Id, Name = z.Name }).ToList()
                }).ToList();
                return result;
            }
           
            return new InWarehouseDTO(); ;
        }

        public bool InWarehouseUpdate(InWarehouseDTO input)
        {
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var inWarehouse = _dbContext.InWarehouse.FirstOrDefault(x => x.Id == input.Id);
                    inWarehouse.CreateAt = input.CreateAt;
                    inWarehouse.Number = input.Number;
                    inWarehouse.VenderId = input.VenderId;
                    inWarehouse.Status = input.Status;
                    _dbContext.SaveChanges();
                    var inWarehouseStocks = _dbContext.InWarehouseStock.Where(x => x.InWarehouseId == input.Id);
                    if (inWarehouseStocks != null)
                    {
                        _dbContext.InWarehouseStock.RemoveRange(inWarehouseStocks);
                        _dbContext.SaveChanges();
                    }
                    List<InWarehouseStockEntities> inWarehouseStockEntities = new List<InWarehouseStockEntities>();
                    if (input.InWarehousDetails != null)
                    {
                        foreach (var item in input.InWarehousDetails)
                        {
                            inWarehouseStockEntities.Add(new InWarehouseStockEntities()
                            {
                                InWarehouseId = inWarehouse.Id,
                                StockId = item.StockId,
                                Quantity = item.Quantity,
                                Status = item.Status
                            });
                        }
                    }
                    _dbContext.InWarehouseStock.AddRange(inWarehouseStockEntities);
                    _dbContext.SaveChanges();
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return false;
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return true;            
        }
        #endregion

        #region Out stock
        public List<OutWarehouseDTO> OutWarehouseGetAll()
        {
            var customer = _dbContext.Customer.AsQueryable();
            var entity = _dbContext.OutWarehouse.AsQueryable();
            return entity.Select(s => new OutWarehouseDTO()
            {
                Id = s.Id,
                CreateAt = s.CreateAt,
                CustomerId = s.CustomerId,
                Status= s.Status,
                CustomerName = customer.FirstOrDefault(f => f.Id == s.CustomerId).FullName
            }).ToList();
        }
        public bool OutWarehouseCreate(OutWarehouseDTO input)
        {
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var outWarehouse = new OutWarehouseEntities()
                    {
                        CreateAt = input.CreateAt,
                        Number = input.Number,
                        CustomerId = input.CustomerId,
                        Status = input.Status,
                    };
                    _dbContext.OutWarehouse.Add(outWarehouse);
                    var result = _dbContext.SaveChanges() > 0;
                    if (result)
                    {
                        List<OutWarehouseStockEntities> outWarehouseStockEntities = new List<OutWarehouseStockEntities>();
                        if (input.OutWarehousDetails != null)
                        {
                            foreach (var item in input.OutWarehousDetails)
                            {
                                outWarehouseStockEntities.Add(new OutWarehouseStockEntities()
                                {
                                    OutWarehouseId = outWarehouse.Id,
                                    StockId = item.StockId,
                                    Quantity = item.Quantity,
                                    Status = item.Status
                                });
                            }
                        }
                        _dbContext.OutWarehouseStock.AddRange(outWarehouseStockEntities);
                        _dbContext.SaveChanges();
                    }
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return false;
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return true;
        }
        public bool OutWarehouseDelete(long id)
        {
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var outWarehouseStocks = _dbContext.OutWarehouseStock.Where(x => x.OutWarehouseId == id);
                    if (outWarehouseStocks != null)
                    {
                        _dbContext.OutWarehouseStock.RemoveRange(outWarehouseStocks);
                        _dbContext.SaveChanges();
                    }
                    var outWarehouse = _dbContext.OutWarehouse.FirstOrDefault(x => x.Id == id);
                    if (outWarehouse != null)
                    {
                        _dbContext.OutWarehouse.Remove(outWarehouse);
                        _dbContext.SaveChanges();
                    }
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return false;
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return true;
        }

        public OutWarehouseDTO OutWarehouseGetById(long id)
        {
            var customer = _dbContext.Customer.AsQueryable();
            var outWarehouse = _dbContext.OutWarehouse.FirstOrDefault(x => x.Id == id);
            if (outWarehouse != null)
            {
                var result = new OutWarehouseDTO()
                {
                    Id = outWarehouse.Id,
                    CreateAt = outWarehouse.CreateAt,
                    CustomerId = outWarehouse.CustomerId,
                    Status= outWarehouse.Status,
                    CustomerName = customer.FirstOrDefault(f => f.Id == outWarehouse.CustomerId)?.FullName
                };
                result.OutWarehousDetails = _dbContext.OutWarehouseStock.Where(x => x.OutWarehouseId == id).Select(s => new OutWarehousDetailDTO()
                {
                    InWarehouseId = s.Id,
                    StockId = s.StockId,
                    Quantity = s.Quantity,
                    Status = s.Status,
                    Stocks = _dbContext.Stock.Select(z => new StockDTO() { Id = z.Id, Name = z.Name }).ToList()
                }).ToList();
                return result;
            }

            return new OutWarehouseDTO(); ;
        }

        public bool OutWarehouseUpdate(OutWarehouseDTO input)
        {
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var outWarehouse = _dbContext.OutWarehouse.FirstOrDefault(x => x.Id == input.Id);
                    outWarehouse.CreateAt = input.CreateAt;
                    outWarehouse.Number = input.Number;
                    outWarehouse.CustomerId = input.CustomerId;
                    outWarehouse.Status = input.Status;
                    var result = _dbContext.SaveChanges() > 0;
                    if (result)
                    {
                        var outWarehouseStocks = _dbContext.OutWarehouseStock.Where(x => x.OutWarehouseId == input.Id);
                        if (outWarehouseStocks != null)
                        {
                            _dbContext.OutWarehouseStock.RemoveRange(outWarehouseStocks);
                            _dbContext.SaveChanges();
                        }
                        List<OutWarehouseStockEntities> outWarehouseStockEntities = new List<OutWarehouseStockEntities>();
                        if (input.OutWarehousDetails != null)
                        {
                            foreach (var item in input.OutWarehousDetails)
                            {
                                outWarehouseStockEntities.Add(new OutWarehouseStockEntities()
                                {
                                    OutWarehouseId = outWarehouse.Id,
                                    StockId = item.StockId,
                                    Quantity = item.Quantity,
                                    Status = item.Status
                                });
                            }
                        }
                        _dbContext.OutWarehouseStock.AddRange(outWarehouseStockEntities);
                        _dbContext.SaveChanges();
                    }
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return false;
                }
                finally
                {
                    trans.Dispose();
                }
            }
            return true;
        }
        #endregion
    }
}
