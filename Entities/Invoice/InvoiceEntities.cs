using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Migrations.Builders;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Invoice
{
    [Table("Invoice")]
    public class InvoiceEntities : BaseEntities
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
    }
}
