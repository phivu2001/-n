using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.User
{
    public class UserDTO : BaseDTO
    {
        [Required(ErrorMessage = "Thông tin bắt buộc.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Thông tin bắt buộc.")]
        public string UserName { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessage = "Thông tin bắt buộc.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Thông tin bắt buộc.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDay { get; set; } = DateTime.Now;
        public string Password { get; set; }
        [Required(ErrorMessage = "Thông tin bắt buộc.")]
        public string Address { get; set; }
        public int Role { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDay { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDay { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public UserDTO()
        {
        }
    }
}
