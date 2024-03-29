using Microsoft.AspNetCore.Mvc;
using Order.Infrastructure.Model;
using Order.Infrastructure.Service;

namespace Order.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpPost(Name = "Create")]
        public async Task<IActionResult> Create(CreateOrderRequestModel requestModel)
        {
            return Ok(await _orderService.CreateOrder(requestModel));
        }

        [HttpGet(Name = "OrderDetails")]
        public async Task<IActionResult> GetOrderDetails()
        {
            return Ok(await _orderService.GetOrderDetails());
        }

        [HttpGet]
        [Route("InitiateClientStreaming")]
        public async Task<IActionResult> InitiateClientStreaming()
        {
            return Ok(await _orderService.InitiateClientStreaming());
        }

        [HttpGet]
        [Route("InitiateDuplexStreaming")]
        public async Task<IActionResult> InitiateDuplexStreaming()
        {
            return Ok(await _orderService.InitiateClientStreaming());
        }
    }
}
