namespace UI.Models.Cart
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; } // Ürünün değişmeyen birim fiyatı
        public decimal Price { get; set; } // Toplam fiyat (UnitPrice * Quantity)
        public int Quantity { get; set; }
    }


}
