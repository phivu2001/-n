using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.User
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "The field is required")]
        //[EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "The field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}
