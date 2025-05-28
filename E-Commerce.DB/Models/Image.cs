using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string? URL { get; set; }
        public string? AltText { get; set; }

        public  bool IsDeleted { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}
