using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DB.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name must not exceed 100 characters")]
        public string? Name { get; set; }

        [MaxLength(250, ErrorMessage = "Description must not exceed 250 characters")]
        public string? Description { get; set; }
    }
}
