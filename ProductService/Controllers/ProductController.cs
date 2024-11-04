using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ProductService.Services;
using ProductService.DTOs;
using ProductService.Models;
using Microsoft.AspNetCore.Authorization;

namespace ProductService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly Services.ProductService _service;
        private readonly IMapper _mapper;
        
        public ProductController(Services.ProductService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<ProductReadDto>> GetAllProducts()
        {
            var products = _service.GetAllProducts();
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
        }
        
        [HttpGet("{id}", Name = "GetProductById")]
        public ActionResult<ProductReadDto> GetProductById(int id)
        {
            var product = _service.GetProductById(id);
            if (product == null) return NotFound();
            return Ok(_mapper.Map<ProductReadDto>(product));
        }
        
        [HttpPost]
        public ActionResult<ProductReadDto> CreateProduct(ProductCreateDto productCreateDto)
        {
            var productModel = _mapper.Map<Product>(productCreateDto);
            _service.CreateProduct(productModel);
            var productReadDto = _mapper.Map<ProductReadDto>(productModel);
            
            return CreatedAtRoute(nameof(GetProductById), 
                new { Id = productReadDto.Id }, productReadDto);
        }
        
        [HttpPut("{id}")]
        public ActionResult UpdateProduct(int id, ProductUpdateDto productUpdateDto)
        {
            var productModelFromRepo = _service.GetProductById(id);
            if (productModelFromRepo == null) return NotFound();
            
            _mapper.Map(productUpdateDto, productModelFromRepo);
            _service.UpdateProduct(productModelFromRepo);
            
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var productModelFromRepo = _service.GetProductById(id);
            if (productModelFromRepo == null) return NotFound();
            
            _service.DeleteProduct(id);
            
            return NoContent();
        }
    }
}
