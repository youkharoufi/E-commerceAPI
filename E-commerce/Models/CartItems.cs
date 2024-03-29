﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models
{
    public class CartItems
    {
        [Key]
        public int ItemId { get; set; }

        [ForeignKey("CartId")]
        public int CartId { get; set; }

        public Cart Cart { get; set; }

        [ForeignKey("Id")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string ItemName { get; set; }

        public string Description { get; set; }

        public double ItemPrice { get; set; }

        public int ItemQuantity { get; set; }

        public string ItemPhotoUrl { get; set; }

        public double ItemTotal { get; set; }

    }
}
