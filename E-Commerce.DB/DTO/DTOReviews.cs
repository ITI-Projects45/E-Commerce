using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Models;

namespace E_Commerce.DB.DTO
{
    public class DTOReviews
    {
        [Required]
        public int? ProductId { get; set; }
        [Required]
        public int? Rating { get; set; }
        
        public string? Comment { get; set; }


    }
}
