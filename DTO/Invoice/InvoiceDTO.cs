using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Invoice
{
    public class InvoiceDTO : BaseDTO
    {
        public DateTime CreateAt { get; set; }
        public long VenderId { get; set; }
        public string VendorName { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public double Quantity { get; set; }
        public decimal Price { get; set; }
        public long ResourceId { get; set; }
        public int Status { get; set; }
        public List<InvoiceDetailDTO> InvoiceDetails { get; set; } = new List<InvoiceDetailDTO>();
    }
}
