using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Warehouse
{
    [Table("OutWarehouseStock")]
    public class OutWarehouseStockEntities : BaseEntities
    {
        public long StockId { get; set; }
        public long OutWarehouseId { get; set; }
        public double Quantity { get; set; }
        public int Status { get; set; }
    }
}
