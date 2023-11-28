using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Stock
{
    [Table("Stock")]
    public class StockEntities : BaseEntities
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }
        public long CategoryId { get; set; }
    }
}
