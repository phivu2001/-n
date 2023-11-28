using BLL.IServices;
using DTO.Customer;
using DTO.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyCTDT.Controllers
{
    public class VendorManagementController : BaseController
    {
        private readonly IVendorService _vendorService;
        public VendorManagementController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }
        // GET: ClassManagerment
        public async Task<ActionResult> Index(string search)
        {
            var models = new List<VendorDTO>();
            models = _vendorService.GetAll();
            return View(models);
        }
        public async Task<ActionResult> New()
        {
            var model = new VendorDTO();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> New(VendorDTO model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            var result = _vendorService.Create(model);
            if (result)
            {
                TempData["Successful"] = "Thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IntervalServer"] = "Lỗi";
            }
            return View(model);
        }

        public async Task<ActionResult> Edit(long Id)
        {
            var model = new VendorDTO();
            model = _vendorService.GetById(Id);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(VendorDTO model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            var result = _vendorService.Update(model);
            if (result)
            {
                TempData["Successful"] = "Thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IntervalServer"] = "Lỗi";
            }
            return View(model);
        }

        public async Task<ActionResult> Destroy(long Id)
        {
            var result = _vendorService.Delete(Id);
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

    }
}