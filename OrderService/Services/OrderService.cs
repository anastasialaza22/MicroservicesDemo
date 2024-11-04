using OrderService.Repositories;
using OrderService.Models;
using System.Text.Json;
using System.Net.Http.Headers;

namespace OrderService.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _repository;
        private readonly HttpClient _httpClient;

        public OrderService(IOrderRepository repository, IHttpClientFactory httpClientFactory)
        {
            _repository = repository;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5000"); // Адрес ProductService
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _repository.GetAllOrders();
        }

        public Order GetOrderById(int id)
        {
            return _repository.GetOrderById(id);
        }

        public async Task CreateOrderAsync(Order order, string token)
        {
            // Получение информации о продукте из ProductService
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/Product/{order.ProductId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Не удалось получить информацию о продукте.");
            }

            var productContent = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<Product>(productContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            order.TotalPrice = product.Price * order.Quantity;
            order.OrderDate = DateTime.UtcNow;

            _repository.CreateOrder(order);
            _repository.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            _repository.UpdateOrder(order);
            _repository.SaveChanges();
        }

        public void DeleteOrder(int id)
        {
            var order = _repository.GetOrderById(id);
            if (order != null)
            {
                _repository.DeleteOrder(order);
                _repository.SaveChanges();
            }
        }
    }

    // Модель продукта для получения данных из ProductService
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
