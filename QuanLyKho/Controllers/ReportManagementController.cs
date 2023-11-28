using BLL.IServices;
using BLL.Services;
using DTO.Stock;
using DTO.User;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyCTDT.Controllers
{
    public class ReportManagementController : BaseController
    {
        private readonly IStockService _stockService;
        public ReportManagementController(IStockService stockService)
        {
            _stockService = stockService;
        }
        // GET: TrainingManagerment
        public async Task<ActionResult> Index()
        {
            var models = _stockService.GetAll();
            return View(models);
        }
        public ActionResult PrintViewToPdf()
        {
            var report = new Rotativa.ViewAsPdf("Index");
            return report;
        }

        public async Task<ActionResult> PrintPartialViewToPdf()
        {
            var models = _stockService.GetAll();
            var report = new Rotativa.ViewAsPdf("~/Views/ReportManagement/report.cshtml", models);
            return report;

        }
    }
}