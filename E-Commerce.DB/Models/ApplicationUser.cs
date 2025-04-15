using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.DB.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string imageurl { get; set; }
    }
}
