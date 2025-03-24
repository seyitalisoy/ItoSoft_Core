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

        // Girilen değere göre Arama metodu değişecek 

        [HttpGet("getbyorderid")]
        public IActionResult GetOrdersByOrderId([FromQuery] int id)
        {
            var result = _orderService.GetByOrderId(id);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpGet("getbyorderdate")]
        public IActionResult GetOrdersByOrderDate([FromQuery] DateTime date)
        {
            var result = _orderService.GetByOrderDate(date);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

        [HttpGet("getbyemail")]
        public IActionResult GetOrdersByEmail([FromQuery] string email)
        {
            var result = _orderService.GetByEmail(email);
            if (!result.Success)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }

    }
}
