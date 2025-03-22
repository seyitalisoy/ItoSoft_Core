using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("orderdetails")]
        public IActionResult GetOrderDetails()
        {
            var result = _orderService.GetOrderDetails();
            if (!result.Success)
            {
                return NotFound("Ürünler bulunamadı");
            }
            return Ok(result);
        }
    }
}
