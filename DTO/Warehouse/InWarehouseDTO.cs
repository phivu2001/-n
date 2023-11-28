using DTO.Stock;
using DTO.Vendor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Warehouse
{
    public class InWarehouseDTO : BaseDTO
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public int Number { get; set; }
        public long VenderId { get; set; }
        public string VenderName { get; set; }
        public List<VendorDTO> Vendors { get; set; } = new List<VendorDTO>();
        public List<InWarehousDetailDTO> InWarehousDetails { get; set; } = new List<InWarehousDetailDTO>();
        public int Status { get; set; }
    }

    public class InWarehousDetailDTO
    {
        public long StockId { get; set; }
        public long InWarehouseId { get; set; }
        public double Quantity { get; set; }
        public int Status { get; set; }
        public List<StockDTO> Stocks { get; set; } = new List<StockDTO>();
        public int Offset { get; set; }
    }
}
