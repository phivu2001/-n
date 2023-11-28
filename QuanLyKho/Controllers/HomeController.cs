using BLL.IServices;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuanLyCTDT.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IWarehouseService _warehouseService;
        private readonly IVendorService _vendorService;
        private readonly ICustomerService _customerService;
        public HomeController(IUserService userService, IWarehouseService warehouseService, IVendorService vendorService, ICustomerService customerService)
        {
            _userService = userService;
            _warehouseService = warehouseService;
            _vendorService = vendorService;
            _customerService = customerService;
        }
        // GET: UserManagerment
        public async Task<ActionResult> Index()
        {
            var ids = _warehouseService.GetInvoiceSendMail();
            if (ids != null)
            {
                var username = "";
                var email = "";
                string body = string.Empty;
                foreach (var invoiceId in ids)
                {
                    var invoiceDetail = _warehouseService.InVoiceDetail(invoiceId);
                    username = !string.IsNullOrEmpty(invoiceDetail.VendorName) ? invoiceDetail.VendorName : invoiceDetail.CustomerName;
                    if (invoiceDetail != null)
                    {
                        if(invoiceDetail.VenderId > 0)
                        {
                            email = _vendorService.GetById(invoiceDetail.VenderId)?.Email;
                        } else if(invoiceDetail.CustomerId > 0)
                        {
                            email = _customerService.GetById(invoiceDetail.CustomerId)?.Email;
                        }
                        if (!string.IsNullOrEmpty(email) && email.Contains("@gmail.com"))
                        {
                            //send mail
                            body = "<div>" + username + "</br>"
                                   + "<div><h3>Invoice information</h3></div>"
                                   + "<div><h5>Đơn hàng của bạn tới hạn thanh toán</h3></div>"
                                   + "</br></div>";
                            //body = body + "<div> Money:" + string.Format("{0:c0}", invoiceDetail.Price) + "</div>";
                            string smtpAddress = "smtp.gmail.com";
                            int portNumber = 587;
                            bool enableSSL = true;
                            string emailFrom = "sendmailstudy93@gmail.com";
                            string password = "skbvebdwjzxrianq";
                            try
                            {
                                using (MailMessage mail = new MailMessage())
                                {
                                    mail.From = new MailAddress(emailFrom);
                                    mail.To.Add(email);
                                    mail.Subject = "Send invoice customer";
                                    mail.Body = body;
                                    mail.IsBodyHtml = true;
                                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                                    {
                                        smtp.UseDefaultCredentials = false;
                                        smtp.Credentials = new NetworkCredential(emailFrom, password);
                                        smtp.EnableSsl = enableSSL;
                                        smtp.Send(mail);
                                    }
                                }
                            }
                            catch (Exception )
                            {
                                break;
                            }
                        }       
                       
                    }
                }
            }

            var models = _userService.GetAll();
            var data = models.ToList().GroupBy(g => new { g.Id, g.FullName }).Select(s => new UserDTO()
            {
                FullName = s.Key.FullName,
                Total = s.Count(),
            }).ToList();
            return View(data);
        }
    }
}