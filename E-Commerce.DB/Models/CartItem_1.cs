namespace E_Commerce.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string? Street { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public String? ZipCode { get; set; }
        public bool IsDeleted { get; set; }

        public List<Order>?Orders{get;set;}
    }
}
