namespace TheHiddenSquirrel.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        // child reference to Products (1 Category can have Many Products)
        // Products list must be nullable so we can first create a Category with no Products, then add Products later
        public List<Product>? Products { get; set; }
    }
}
