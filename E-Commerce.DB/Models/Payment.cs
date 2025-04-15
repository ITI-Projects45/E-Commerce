using System.ComponentModel.DataAnnotations.Schema;
using static E_Commerce.Models.Enums.Enums;

namespace E_Commerce.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime PaidAt { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order? Order { get; set; }
       


    }
}
