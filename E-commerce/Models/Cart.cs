using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [ForeignKey("Id")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public List<CartItems> cartItems { get; set; } = new List<CartItems>();

        public double TotalPrice { get; set; }
    }
}
