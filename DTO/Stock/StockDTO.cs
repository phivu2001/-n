using DTO.Category;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DTO.Stock
{
    public class StockDTO : BaseDTO
    {
        [Required(ErrorMessage = "Thông tin bắt buộc.")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Thông tin bắt buộc.")]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageURL { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<CategoryDTO> Categorys { get; set; } = new List<CategoryDTO>();
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PictureUpload { get; set; }
        public byte[] PictureByte { get; set; }
        public double? Qty { get; set; }
    }
}
