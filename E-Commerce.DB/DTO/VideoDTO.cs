using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DB.DTO
{
    public class VideoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "URL is required")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string URL { get; set; }

        [Required(ErrorMessage = "AltText is required")]
        [MaxLength(200, ErrorMessage = "AltText should not exceed 200 characters")]
        public string AltText { get; set; }

        [Required(ErrorMessage = "Product ID is required")]
        public int ProductId { get; set; }

    }
}
