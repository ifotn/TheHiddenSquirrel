using System.ComponentModel.DataAnnotations;

namespace TheHiddenSquirrel.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]  // display as currency: 0 is the value, c is the currency format
        [Range(0,10000)]
        public decimal Price { get; set; }


        public int? Age { get; set; }
        public string? Image { get; set; }
        public int? Rating { get; set; }

        // FK: each product belongs to 1 Category
        public int CategoryId { get; set; }

        // parent reference to Category object.  Used to JOIN a Product to its parent Category.
        public Category? Category { get; set; }

        // child ref to CartItem: 1 product => Many cart items
        public List<CartItem>? CartItems { get; set; }
    }
}
