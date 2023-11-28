using BLL.IServices;
using DTO.Customer;
using DTO.Stock;
using DTO.Vendor;
using DTO.Warehouse;
using QuanLyCTDT.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyCTDT.Controllers
{
    public class OutWarehouseManagementController : BaseController
    {
        private readonly IWarehouseService _wareHouseService;
        private readonly ICustomerService _customerService;
        private readonly IStockService _stockService;
        public OutWarehouseManagementController(IWarehouseService wareHouseService, ICustomerService customerService, IStockService stockService)
        {
            _wareHouseService = wareHouseService;
            _customerService = customerService;
            _stockService = stockService;
        }
        // GET: ClassManagerment
        public async Task<ActionResult> Index(string search)
        {
            var models = new List<OutWarehouseDTO>();
            models = _wareHouseService.OutWarehouseGetAll();
            return View(models);
        }
        public async Task<ActionResult> New()
        {
            var model = new OutWarehouseDTO();
            model.Customers = GetSelectListCustomer();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> New(OutWarehouseDTO model)
        {

            if (model.CustomerId < 0)
            {
                model.Customers = GetSelectListCustomer();
                if (model.OutWarehousDetails != null)
                {
                    foreach (var item in model.OutWarehousDetails)
                    {
                        item.Stocks = GetSelectListStock();
                    }
                }
                TempData["IntervalServer"] = "Vui lòng chọn khách hàng!";
                return View(model);
            }
            if (model.OutWarehousDetails != null && model.OutWarehousDetails.Any())
            {
                model.OutWarehousDetails = model.OutWarehousDetails.GroupBy(g => new { g.StockId, g.Status }).Select(s => new OutWarehousDetailDTO()
                {
                    StockId = s.Key.StockId,
                    Status = s.Key.Status,
                    Quantity = Convert.ToDouble(s.Sum(z => z.Quantity))
                }).ToList();
            }
            var checkAllowSell = _wareHouseService.CheckOutOffStock(model.OutWarehousDetails);
            if (!checkAllowSell)
            {
                if (model.OutWarehousDetails != null)
                {
                    foreach (var item in model.OutWarehousDetails)
                    {
                        item.Stocks = GetSelectListStock();
                    }
                }
                model.Customers = GetSelectListCustomer();
                TempData["IntervalServer"] = "Số lượng vật tư bán ra vượt quá số lượng hiện có trong kho. Vui lòng nhập kho!"; 
                return View(model);
            }
            var result = _wareHouseService.OutWarehouseCreate(model);
            if (result)
            {
                TempData["Successful"] = "Thành công";
                return RedirectToAction("Index");
            }
            else
            {
                model.Customers = GetSelectListCustomer();
                TempData["IntervalServer"] = "Lỗi";
            }
            return View(model);
        }

        public async Task<ActionResult> Edit(long Id)
        {
            var model = new OutWarehouseDTO();
            model = _wareHouseService.OutWarehouseGetById(Id);
            model.Customers = GetSelectListCustomer();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(OutWarehouseDTO model)
        {
            if (ModelState.IsValid == false)
            {
                model.Customers = GetSelectListCustomer();
                return View(model);
            }
            if (model.OutWarehousDetails != null && model.OutWarehousDetails.Any())
            {
                model.OutWarehousDetails = model.OutWarehousDetails.GroupBy(g => new { g.StockId, g.Status }).Select(s => new OutWarehousDetailDTO()
                {
                    StockId = s.Key.StockId,
                    Status = s.Key.Status,
                    Quantity = Convert.ToDouble(s.Sum(z => z.Quantity))
                }).ToList();
            }
            var result = _wareHouseService.OutWarehouseUpdate(model);
            if (result)
            {
                TempData["Successful"] = "Thành công";
                return RedirectToAction("Index");
            }
            else
            {
                model.Customers = GetSelectListCustomer();
                TempData["IntervalServer"] = "Lỗi";
            }
            return View(model);
        }

        public async Task<ActionResult> Destroy(long Id)
        {
            var result = _wareHouseService.OutWarehouseDelete(Id);
            if (result)
            {
                TempData["Successful"] = "Thành công";
            }
            else
            {
                TempData["IntervalServer"] = "Lỗi";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Canceled(long id)
        {
            _wareHouseService.ChangeStatusOutWarehouse(id, (int)EnumCommon.EWarehouseStatus.Cancelled);
            return RedirectToAction("Index");
        }
        public ActionResult CreateSell(long id)
        {
            _wareHouseService.SellCreate(id);
            return RedirectToAction("Index");
        }
        public ActionResult AddStock(int offSet)
        {
            OutWarehousDetailDTO model = new OutWarehousDetailDTO();
            model.Stocks = GetSelectListStock();
            model.Offset = offSet;
            return PartialView("_ItemStock", model);
        }

        public List<CustomerDTO> GetSelectListCustomer()
        {
            return _customerService.GetAll();
        }
        public List<StockDTO> GetSelectListStock()
        {
            return _stockService.GetAll();
        }
    }
}