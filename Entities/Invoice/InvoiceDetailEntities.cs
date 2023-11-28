using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Invoice
{
    [Table("InvoiceDetail")]
    public class InvoiceDetailEntities : BaseEntities
    {
        public long InvoiceId { get; set; }
        public long StockId { get; set; }
        public long ResourceId { get; set; }
        public double Quantity { get; set; }
        public int Status { get; set; }
    }
}
