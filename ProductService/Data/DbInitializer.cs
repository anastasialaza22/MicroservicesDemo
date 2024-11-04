using ProductService.Models;

namespace ProductService.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ProductContext context)
        {
            if (context.Products.Any())
            {
                return; // Данные уже инициализированы
            }
            
            var products = new Product[]
            {
                new Product
                {
                    Name = "Товар 1",
                    Price = 100,
                    Description = "Описание 1",
                    Category = "Категория A"
                },
                new Product
                {
                    Name = "Товар 2",
                    Price = 200,
                    Description = "Описание 2",
                    Category = "Категория B"
                },
            };
            
            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}