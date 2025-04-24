using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DB.DTO.UsersDTO
{
    public  class RolsDTO
    {
        [Required(ErrorMessage ="Role Name Is Required")]
        public string  RoleName{ get; set; }

    }
}
