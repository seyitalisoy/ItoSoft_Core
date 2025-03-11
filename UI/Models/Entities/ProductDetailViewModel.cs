namespace UI.Models.Entities
{
    public class ProductDetailViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string Picture { get; set; }
        public int UnitsInStock { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
