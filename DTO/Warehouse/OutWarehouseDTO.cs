using DTO.Customer;
using DTO.Stock;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DTO.Warehouse
{
    public class OutWarehouseDTO : BaseDTO
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public int Number { get; set; }
        public long CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<CustomerDTO> Customers { get; set; } = new List<CustomerDTO>();
        public List<OutWarehousDetailDTO> OutWarehousDetails { get; set; } = new List<OutWarehousDetailDTO>();
        public int Status { get; set; }
    }
    public class OutWarehousDetailDTO
    {
        public long StockId { get; set; }
        public long InWarehouseId { get; set; }
        public double Quantity { get; set; }
        public int Status { get; set; }
        public List<StockDTO> Stocks { get; set; } = new List<StockDTO>();
        public int Offset { get; set; }
    }
}
