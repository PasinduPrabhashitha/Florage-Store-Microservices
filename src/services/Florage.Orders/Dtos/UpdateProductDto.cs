﻿namespace Florage.Orders.Dtos
{
    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public float Price { get; set; }
    }
}
