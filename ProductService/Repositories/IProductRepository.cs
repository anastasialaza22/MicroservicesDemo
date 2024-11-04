using ProductService.Models;

namespace ProductService.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        
        Product GetProductById(int id);
        
        void CreateProduct(Product product);
        
        void UpdateProduct(Product product);
        
        void DeleteProduct(Product product);
        
        bool SaveChanges();
    }
}