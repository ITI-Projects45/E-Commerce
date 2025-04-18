using System.ComponentModel.DataAnnotations.Schema;
using static E_Commerce.Models.Enums.Enums;

namespace E_Commerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        public decimal total { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("Address")]
        public int AddresId { get; set; }
        public CartItem? Address { get; set; }

        public Payment? Payment { get; set; }
        public List<OrderItem>? OrderItem { get; set; }

    }
}
