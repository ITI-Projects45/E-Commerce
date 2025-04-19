using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Models;

namespace E_Commerce.Repos.DTOs
{
    public class OrderItemDTO
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int ProductId { get; set; }

    }
}
