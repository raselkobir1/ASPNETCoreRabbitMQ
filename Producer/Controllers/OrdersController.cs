using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Producer.Data;
using Producer.Dtos;
using Producer.RabbitMQ;

namespace Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMessageProducer _messageProducer;
        private readonly IOrderDbContext _orderDbContext;
        public OrdersController(IMessageProducer messageProducer, IOrderDbContext orderDbContext)
        {
            _messageProducer = messageProducer;
            _orderDbContext = orderDbContext;       
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto orderDto)
        {
            Order order = new()
            {
                ProductName = orderDto.ProductName,
                Price = orderDto.Price,
                Quantity = orderDto.Quantity, 
            };
            //_orderDbContext.Order.Add(order);
            //await _orderDbContext.SaveChangesAsync();

            _messageProducer.SendMessage(order);

            return Ok(new { id = order.Id });
        }   
    }
}
