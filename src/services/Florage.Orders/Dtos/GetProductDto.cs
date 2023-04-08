﻿namespace Florage.Orders.Dtos
{
    public class GetProductDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public float Price { get; set; }
    }
}
