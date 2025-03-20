using AutoMapper;
using Business.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet("getall")]
        public IActionResult GetProducts()
        {
            var result = _productService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest("Ürün listelenmedi");
        }

        [HttpGet("get")]
        public IActionResult GetProductById(int id)
        {
            var result = _productService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result.Message);
        }

        [HttpPost("add")]
        public IActionResult AddProduct(Product product)
        {
            var result = _productService.Add(product);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpDelete("delete")]
        public IActionResult DeleteProduct( int id)
        {
            var result = _productService.DeleteById(id);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpPut("update")]
        public IActionResult UpdateProduct([FromBody] ProductUpdateDto productDto, [FromQuery] int id)
        {
            var productResult = _productService.GetById(id);

            if (!productResult.Success)
            {
                return NotFound(productResult.Message);
            }

            _mapper.Map(productDto, productResult.Data);

            var updateResult = _productService.Update(productResult.Data);

            if (!updateResult.Success)
            {
                return BadRequest(updateResult.Message);
            }
            return Ok(updateResult.Message);
        }
    }
}
