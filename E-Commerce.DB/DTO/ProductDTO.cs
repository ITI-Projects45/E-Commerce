using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Models;
using static E_Commerce.Models.Enums.Enums;

namespace E_Commerce.Repos.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }

        public List<string>? ImageUrls { get; set; } 

    }
}
