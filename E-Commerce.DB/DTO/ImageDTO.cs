using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DB.DTO
{
    public class ImageDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "URL is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string URL { get; set; }

        [MaxLength(100, ErrorMessage = "AltText cannot exceed 100 characters")]
        public string AltText { get; set; }
    }
}
