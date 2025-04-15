using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Brand { get; set; }
        public virtual List<Image>? Images { get; set; } = new List<Image>();
        public virtual List<Video>? Viedos { get; set; } = new List<Video>();
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<Review>? Reviews { get; set; }
        public List<CartItem>? CartItems { get; set; }
        public List<OrderItem>? OrderItems { get; set; }

    }
}
