using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Models;

namespace E_Commerce.DB.DTO
{
    public class DTOAddress
    {
        public int ID { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public bool IsDeleted { get; set; } = false;

        public List<Order>? Orders { get; set; }
    }
}
