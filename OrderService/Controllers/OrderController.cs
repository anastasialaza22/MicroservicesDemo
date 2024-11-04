using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using OrderService.Services;
using OrderService.DTOs;
using OrderService.Models;
using Microsoft.AspNetCore.Authorization;

namespace OrderService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly Services.OrderService _service;
        private readonly IMapper _mapper;

        public OrderController(Services.OrderService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<OrderReadDto>> GetAllOrders()
        {
            var orders = _service.GetAllOrders();
            return Ok(_mapper.Map<IEnumerable<OrderReadDto>>(orders));
        }

        [HttpGet("{id}", Name = "GetOrderById")]
        public ActionResult<OrderReadDto> GetOrderById(int id)
        {
            var order = _service.GetOrderById(id);
            if (order == null) return NotFound();
            return Ok(_mapper.Map<OrderReadDto>(order));
        }

        [HttpPost]
        public async Task<ActionResult<OrderReadDto>> CreateOrder(OrderCreateDto orderCreateDto)
        {
            var orderModel = _mapper.Map<Order>(orderCreateDto);

            // Получение токена из заголовка Authorization
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            try
            {
                await _service.CreateOrderAsync(orderModel, token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            var orderReadDto = _mapper.Map<OrderReadDto>(orderModel);

            return CreatedAtRoute(nameof(GetOrderById),
                new { Id = orderReadDto.Id }, orderReadDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateOrder(int id, OrderUpdateDto orderUpdateDto)
        {
            var orderModelFromRepo = _service.GetOrderById(id);
            if (orderModelFromRepo == null) return NotFound();

            _mapper.Map(orderUpdateDto, orderModelFromRepo);
            _service.UpdateOrder(orderModelFromRepo);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteOrder(int id)
        {
            var orderModelFromRepo = _service.GetOrderById(id);
            if (orderModelFromRepo == null) return NotFound();

            _service.DeleteOrder(id);

            return NoContent();
        }
    }
}
