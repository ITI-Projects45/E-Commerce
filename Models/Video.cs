namespace E_Commerce.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string? URL { get; set; }
        public string? AltText { get; set; }

        public bool IsDeleted { get; set; }

        public Product? Product { get; set; }
    }
}
