﻿namespace UI.Models.Entities
{
    public class ProductUpdateViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string Picture { get; set; }
        public int UnitsInStock { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}

