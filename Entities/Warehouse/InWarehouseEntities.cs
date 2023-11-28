using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Warehouse
{
    [Table("InWarehouse")]
    public class InWarehouseEntities : BaseEntities
    {
        public DateTime CreateAt { get; set; }
        public int Number { get; set; }
        public long VenderId { get; set; }
        public int Status { get; set; }
    }
}
