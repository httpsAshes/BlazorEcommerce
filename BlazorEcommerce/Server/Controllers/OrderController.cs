using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderservice;

        public OrderController(IOrderService orderService)
        {
               _orderservice = orderService;
        }

        [HttpPost]
        public  async Task<ActionResult<ServiceResponse<bool>>> PlaceOrder()
        {
            var result = await _orderservice.PlaceOrder();
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<OrderOverviewResponse>>>> GetOrders()
        {
            var result = await _orderservice.GetOrders();
            return Ok(result);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<ServiceResponse<List<OrderDetailsResponse>>>> GetOrderDetails(int orderId)
        {
            var result = await _orderservice.GetOrderDetails(orderId);
            return Ok(result);
        }
    }
}
