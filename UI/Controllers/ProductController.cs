using Azure.Core;
using Business.Abstract;
using Business.ValidationRules;
using Entities.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Models;
using UI.Models.Entities;

namespace UI.Controllers
{
    
    public class ProductController : Controller
    {
        IProductService _productService;
        ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult List(int? id)
        {
            ViewBag.Id = id;
            var products = _productService.GetAll().Data;
            if (id != null)
            {
                products = _productService.GetAll().Data.Where(p => p.CategoryId == id).ToList();
            }
            var model = new ProductListViewModel()
            {
                Categories = _categoryService.GetAll().Data,
                Products = products
            };
            return View(model);
        }

       
        public IActionResult GetProductDetails(int id)
        {
            var result = _productService.GetById(id).Data;
            var categoryName = _categoryService.GetById(result.CategoryId).Data.CategoryName;
            var model = new ProductDetailViewModel()
            {
                ProductId = id, 
                CategoryName = categoryName,
                Picture = result.Picture,
                Description = result.Description,
                Price = result.Price,
                ProductName = result.ProductName,
                UnitsInStock = result.UnitsInStock
            };
            return View(model);
        }
       

    }
}
