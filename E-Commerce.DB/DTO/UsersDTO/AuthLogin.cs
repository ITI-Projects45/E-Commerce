using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DB.DTO.UsersDTO
{
    public class AuthLogin
    {


        [DefaultValue("hassan@app.com")]
        [Required(ErrorMessage = "ُEmail Is required")]
        public string Email { get; set; }

        [DefaultValue("Hh@12345678910")]
        [Required(ErrorMessage = "Password Is  required")]
        public string Password { get; set; } = "Hh@12345678910";
    }
}
