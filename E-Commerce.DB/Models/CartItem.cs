using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal SubtotalPric { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public Cart? Cart { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

    }
}
