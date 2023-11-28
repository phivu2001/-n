using BLL.IServices;
using DTO.Category;
using DTO.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyCTDT.Controllers
{
    public class CategoryManagementController : BaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoryManagementController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        // GET: ClassManagerment
        public async Task<ActionResult> Index(string search)
        {
            var models = new List<CategoryDTO>();
            models = _categoryService.GetAll();
            return View(models);
        }
        public async Task<ActionResult> New()
        {
            var model = new CategoryDTO();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> New(CategoryDTO model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            var result = _categoryService.Create(model);
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
            var model = new CategoryDTO();
            model = _categoryService.GetById(Id);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CategoryDTO model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            var result = _categoryService.Update(model);
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
            var result = _categoryService.Delete(Id);
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