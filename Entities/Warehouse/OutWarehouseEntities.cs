using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Warehouse
{
    [Table("OutWarehouse")]
    public class OutWarehouseEntities : BaseEntities
    {
        public DateTime CreateAt { get; set; }
        public int Number { get; set; }
        public long CustomerId { get; set; }
        public int Status { get; set; }
    }
}
