using BLL.IServices;
using DTO.Category;
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
    public class InWarehouseManagementController : BaseController
    {
        private readonly IWarehouseService _wareHouseService;
        private readonly IVendorService _vendorService;
        private readonly IStockService _stockService;
        public InWarehouseManagementController(IWarehouseService wareHouseService, IVendorService vendorService, IStockService stockService)
        {
            _wareHouseService = wareHouseService;
            _vendorService = vendorService;
            _stockService = stockService;
        }
        // GET: ClassManagerment
        public async Task<ActionResult> Index(string search)
        {
            var models = new List<InWarehouseDTO>();
            models = _wareHouseService.InWarehouseGetAll();
            return View(models);
        }
        public async Task<ActionResult> New()
        {
            var model = new InWarehouseDTO();
            model.Vendors = GetSelectListVendor();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> New(InWarehouseDTO model)
        {
            model.CreateAt = DateTime.Now;
            if (model.VenderId > -1)
            {
                model.Vendors = GetSelectListVendor();
                TempData["IntervalServer"] = "Vui lòng chọn khách hàng!";
                return View(model);
            }
            if (model.InWarehousDetails != null && model.InWarehousDetails.Any())
            {
                model.InWarehousDetails = model.InWarehousDetails.GroupBy(g => new { g.StockId, g.Status }).Select(s => new InWarehousDetailDTO()
                {
                    StockId = s.Key.StockId,
                    Status = s.Key.Status,
                    Quantity = Convert.ToDouble(s.Sum(z => z.Quantity))
                }).ToList();
            }
            var result = _wareHouseService.InWarehouseCreate(model);
            if (result)
            {
                TempData["Successful"] = "Thành công";
                return RedirectToAction("Index");
            }
            else
            {
                model.Vendors = GetSelectListVendor();
                TempData["IntervalServer"] = "Lỗi";
            }
            return View(model);
        }

        public async Task<ActionResult> Edit(long Id)
        {
            var model = new InWarehouseDTO();
            model = _wareHouseService.InWarehouseGetById(Id);
            model.Vendors = GetSelectListVendor();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(InWarehouseDTO model)
        {
            if (ModelState.IsValid == false)
            {
                model.Vendors = GetSelectListVendor();
                return View(model);
            }
            if (model.InWarehousDetails != null && model.InWarehousDetails.Any())
            {
                model.InWarehousDetails = model.InWarehousDetails.GroupBy(g => new { g.StockId, g.Status }).Select(s => new InWarehousDetailDTO()
                {
                    StockId = s.Key.StockId,
                    Status = s.Key.Status,
                    Quantity = Convert.ToDouble(s.Sum(z => z.Quantity))
                }).ToList();
            }
            var result = _wareHouseService.InWarehouseUpdate(model);
            if (result)
            {
                TempData["Successful"] = "Thành công";
                return RedirectToAction("Index");
            }
            else
            {
                model.Vendors = GetSelectListVendor();
                TempData["IntervalServer"] = "Lỗi";
            }
            return View(model);
        }

        public async Task<ActionResult> Destroy(long Id)
        {
            var result = _wareHouseService.InWarehouseDelete(Id);
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
            _wareHouseService.ChangeStatusInWarehouse(id, (int)EnumCommon.EWarehouseStatus.Cancelled);
            return RedirectToAction("Index");
        }
        public ActionResult CreatePurchase(long id)
        {
            _wareHouseService.PurchaseCreate(id);
            return RedirectToAction("Index");
        }
        public ActionResult AddStock(int offSet)
        {
            InWarehousDetailDTO model = new InWarehousDetailDTO();
            model.Stocks = GetSelectListStock();
            model.Offset = offSet;
            return PartialView("_ItemStock", model);
        }

        public List<VendorDTO> GetSelectListVendor()
        {
            return _vendorService.GetAll();
        }
        public List<StockDTO> GetSelectListStock()
        {
            return _stockService.GetAll();
        }
    }
}