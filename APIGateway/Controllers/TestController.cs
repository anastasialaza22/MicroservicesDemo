using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TestController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok("API Gateway is running.");
        }

        [HttpGet("order-service-status")]
        public async Task<IActionResult> GetOrderServiceStatus()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://localhost:5068/api/order/status");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok($"Order Service is running: {content}");
            }

            return StatusCode((int)response.StatusCode, "Order Service is unavailable.");
        }

        [HttpGet("product-service-status")]
        public async Task<IActionResult> GetProductServiceStatus()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://localhost:5168/api/product/status");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok($"Product Service is running: {content}");
            }

            return StatusCode((int)response.StatusCode, "Product Service is unavailable.");
        }
    }
}