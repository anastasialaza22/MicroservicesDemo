using ProductService.Repositories;
using ProductService.Models;

namespace ProductService.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repository;
        
        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }
        
        public IEnumerable<Product> GetAllProducts()
        {
            return _repository.GetAllProducts();
        }
        
        public Product GetProductById(int id)
        {
            return _repository.GetProductById(id);
        }
        
        public void CreateProduct(Product product)
        {
            _repository.CreateProduct(product);
            _repository.SaveChanges();
        }
        
        public void UpdateProduct(Product product)
        {
            _repository.UpdateProduct(product);
            _repository.SaveChanges();
        }
        
        public void DeleteProduct(int id)
        {
            var product = _repository.GetProductById(id);
            if (product != null)
            {
                _repository.DeleteProduct(product);
                _repository.SaveChanges();
            }
        }
    }
}