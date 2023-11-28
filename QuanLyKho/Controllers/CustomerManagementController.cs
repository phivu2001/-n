using BLL.IServices;
using DTO.Category;
using DTO.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyCTDT.Controllers
{
    public class CustomerManagementController : BaseController
    {
        private readonly ICustomerService _customerService;
        public CustomerManagementController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        // GET: ClassManagerment
        public async Task<ActionResult> Index(string search)
        {
            var models = new List<CustomerDTO>();
            models = _customerService.GetAll();
            return View(models);
        }
        public async Task<ActionResult> New()
        {
            var model = new CustomerDTO();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> New(CustomerDTO model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            var result = _customerService.Create(model);
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
            var model = new CustomerDTO();
            model = _customerService.GetById(Id);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CustomerDTO model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            var result = _customerService.Update(model);
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
            var result = _customerService.Delete(Id);
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