using System.ComponentModel.DataAnnotations;

namespace TheHiddenSquirrel.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Price { get; set; }

        [Required]
        public string CustomerId { get; set; }

        // FK
        [Required]
        public int ProductId { get; set; }

        // parent ref
        public Product? Product { get; set; }
    }
}
