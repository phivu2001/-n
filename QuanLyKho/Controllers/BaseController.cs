using DTO.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyCTDT.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            var temp = System.Web.HttpContext.Current.Session["User"] as UserDTO;
            ViewData["User"] = temp;
        }
    }
}