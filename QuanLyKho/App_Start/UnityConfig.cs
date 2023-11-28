using BLL.IServices;
using BLL.Services;
using Entities;
using System.Data.Entity;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace QuanLyCTDT
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<DbContext, ApplicationDbContext>();

            container.RegisterType<IUserService, UserService>();

            container.RegisterType<IVendorService, VendorService>();

            container.RegisterType<ICustomerService, CustomerService>();

            container.RegisterType<ICategoryService, CategoryService>();

            container.RegisterType<IStockService, StockService>();

            container.RegisterType<IWarehouseService, WarehouseService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}