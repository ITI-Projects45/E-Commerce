using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DB.DTO.UsersDTO
{
    public class AuthLogin
    {
     
        
        [Required(ErrorMessage ="ُEmail Is required")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password Is  required")]
        public string  Password { get; set; }
    }
}
