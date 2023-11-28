using BLL.IServices;
using DTO.Category;
using DTO.Stock;
using QuanLyCTDT.Extentions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyCTDT.Controllers
{
    public class StockManagementController : BaseController
    {
        private readonly IStockService _stockService;
        private readonly ICategoryService _categoryService;
        public StockManagementController(IStockService stockService, ICategoryService categoryService)
        {
            _stockService = stockService;
            _categoryService = categoryService;
        }
        // GET: ClassManagerment
        public async Task<ActionResult> Index(string search)
        {
            var models = new List<StockDTO>();
            models = _stockService.GetAll();
            return View(models);
        }
        public async Task<ActionResult> New()
        {
            var model = new StockDTO();
            model.Code = Guid.NewGuid().ToString();
            model.Categorys = GetSelectListCate();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> New(StockDTO model)
        {
            if (ModelState.IsValid == false)
            {
                model.Categorys = GetSelectListCate();
                return View(model);
            }
            byte[] photoByte = null;
            if (model.PictureUpload != null && model.PictureUpload.ContentLength > 0)
            {
                Byte[] imgByte = new Byte[model.PictureUpload.ContentLength];
                model.PictureUpload.InputStream.Read(imgByte, 0, model.PictureUpload.ContentLength);
                model.PictureByte = imgByte;
                model.ImageURL = string.Format("/Uploads/{0}", Guid.NewGuid() + Path.GetExtension(model.PictureUpload.FileName));
                photoByte = imgByte;
            }
            if (string.IsNullOrEmpty(model.ImageURL))
            {
                model.ImageURL = "/Images/DefaultUser.jpg";
            }
            var result = _stockService.Create(model);
            if (result)
            {
                if (!string.IsNullOrEmpty(model.ImageURL) && model.PictureByte != null)
                {
                    var path = Server.MapPath(model.ImageURL);
                    MemoryStream ms = new MemoryStream(photoByte, 0, photoByte.Length);
                    ms.Write(photoByte, 0, photoByte.Length);
                    System.Drawing.Image imageTmp = System.Drawing.Image.FromStream(ms, true);
                    ImageHelper.Me.SaveCroppedImage(imageTmp, path, model.ImageURL, ref photoByte, imageTmp.Width, imageTmp.Width, imageTmp.Height);
                }
                TempData["Successful"] = "Thành công";
                return RedirectToAction("Index");
            }
            else
            {
                model.Categorys = GetSelectListCate();
                TempData["IntervalServer"] = "Lỗi";
            }
            return View(model);
        }

        public async Task<ActionResult> Edit(long Id)
        {
            var model = new StockDTO();
            model = _stockService.GetById(Id);
            model.Categorys = GetSelectListCate();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(StockDTO model)
        {
            if (ModelState.IsValid == false)
            {
                model.Categorys = GetSelectListCate();
                return View(model);
            }
            byte[] photoByte = null;
            var backUpURL = model.ImageURL;
            if (!string.IsNullOrEmpty(model.ImageURL))
            {
                backUpURL = model.ImageURL;
            }

            if (model.PictureUpload != null && model.PictureUpload.ContentLength > 0)
            {
                Byte[] imgByte = new Byte[model.PictureUpload.ContentLength];
                model.PictureUpload.InputStream.Read(imgByte, 0, model.PictureUpload.ContentLength);
                model.PictureByte = imgByte;
                model.ImageURL = string.Format("/Uploads/{0}", Guid.NewGuid() + Path.GetExtension(model.PictureUpload.FileName));
                model.PictureUpload = null;
                photoByte = imgByte;
            }
            if (string.IsNullOrEmpty(model.ImageURL))
            {
                model.ImageURL = "/Images/DefaultUser.jpg";
            }
            var result = _stockService.Update(model);
            if (result)
            {
                if (!string.IsNullOrEmpty(model.ImageURL) && model.PictureByte != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(backUpURL)) && !backUpURL.Contains("DefaultUser.jpg"))
                    {
                        ImageHelper.Me.TryDeleteImageUpdated(Server.MapPath(backUpURL));
                    }

                    var path = Server.MapPath(model.ImageURL);
                    MemoryStream ms = new MemoryStream(photoByte, 0, photoByte.Length);
                    ms.Write(photoByte, 0, photoByte.Length);
                    System.Drawing.Image imageTmp = System.Drawing.Image.FromStream(ms, true);

                    ImageHelper.Me.SaveCroppedImage(imageTmp, path, model.ImageURL, ref photoByte, imageTmp.Width, imageTmp.Width, imageTmp.Height);
                }
                TempData["Successful"] = "Thành công";
                return RedirectToAction("Index");
            }
            else
            {
                model.Categorys = GetSelectListCate();
                TempData["IntervalServer"] = "Lỗi";
            }
            return View(model);
        }

        public async Task<ActionResult> Destroy(long Id)
        {
            var result = _stockService.Delete(Id);
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

        public List<CategoryDTO> GetSelectListCate()
        {
            return _categoryService.GetAll();
        }
    }
}