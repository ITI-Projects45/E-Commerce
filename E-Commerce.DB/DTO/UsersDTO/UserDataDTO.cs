using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.DB.DTO.UsersDTO
{
    public class UserDataDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Imageurl { get; set; }
        public string? PhoneNumber { get; set; }

        public List<string>? Roles {  get; set; }

    }
}
