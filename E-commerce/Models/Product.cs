using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public string PhotoUrl1 { get; set; }
        public string? PhotoUrl2 { get; set; }

        public string? PhotoUrl3 { get; set; }
        public string? PhotoUrl4 { get; set; }

    }
}
