using BLL.IServices;
using DTO.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyCTDT.Controllers
{
    public class UserManagementController : BaseController
    {
        private readonly IUserService _userService;
        public UserManagementController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: UserManagerment
        public async Task<ActionResult> Index()
        {
            var models = _userService.GetAll();
            return View(models);
        }
        public ActionResult New()
        {
            var model = new UserDTO();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> New(UserDTO model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            var result = _userService.Create(model);
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
            var model = new UserDTO();
            model = _userService.GetById(Id);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserDTO model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            var result = _userService.Update(model);
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
            var result = _userService.Delete(Id);
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

        public ActionResult SignIn()
        {
            var model = new LoginDTO();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(LoginDTO model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            var userInfor = _userService.Login(model.Email, model.Password);
            if (userInfor != null)
            {
                Session.Add("User", userInfor);
                //
                // create cookie user
                string myObjectJson = JsonConvert.SerializeObject(userInfor);  //new JavaScriptSerializer().Serialize(userSession);
                HttpCookie userCookie = new HttpCookie("UserCookie");
                userCookie.Expires = DateTime.Now.AddDays(1);
                userCookie.Value = Server.UrlEncode(myObjectJson);
                HttpContext.Response.Cookies.Add(userCookie);
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                TempData["LockLogin"] = "The Infomation Users Not Correct";
                return View(model);
            }
        }
        public ActionResult Logout()
        {
            if (HttpContext.Session != null && HttpContext.Session["User"] != null)
                HttpContext.Session.Remove("User");
            HttpCookie cookie = new HttpCookie("UserCookie");
            HttpContext.Response.Cookies.Remove("UserCookie");
            HttpContext.Response.SetCookie(cookie);
            return RedirectToAction("Index", "Home");
        }
    }
}