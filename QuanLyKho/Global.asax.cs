using DTO.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace QuanLyCTDT
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            UnityConfig.RegisterComponents();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        void Application_AcquireRequestState(object sender, EventArgs e)
        {
            HttpCookie _UserAdminCookie = Request.Cookies["UserCookie"];
            if (_UserAdminCookie != null)
            {
                var input = Server.UrlDecode(_UserAdminCookie.Value);
                UserDTO userSession = JsonConvert.DeserializeObject<UserDTO>(input); //new JavaScriptSerializer().Deserialize<UserSession>(input);
                if (userSession != null && HttpContext.Current.Session != null)
                {
                    Session.Add("User", userSession);
                }
            }
        }
    }
}
