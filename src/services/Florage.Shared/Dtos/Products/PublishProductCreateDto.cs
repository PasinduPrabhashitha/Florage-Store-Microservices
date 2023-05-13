﻿namespace Florage.Shared.Dtos.Products
{
    public class PublishProductCreateDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public double Price { get; set; }
        public double BuyPrice { get; set; }
        public int StockCount { get; set; }
    }
}
