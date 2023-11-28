using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Product
{
    public class ProductDTO : BaseDTO
    {
        public string Name { get; set; }
        public long CategoryId { get; set; }
        public long CategoryName { get; set; }
        public long VendorId { get; set; }
        public long VendorName { get; set; }
    }
}
