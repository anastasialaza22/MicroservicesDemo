﻿namespace ProductService.DTOs
{
    public class ProductUpdateDto
    {
        public string Name { get; set; }
        
        public decimal Price { get; set; }
        
        public string Description { get; set; }
        
        public string Category { get; set; }
    }
}