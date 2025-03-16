using Business.Abstract;
using Business.ValidationRules;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Models.Entities;
using UI.Models;
using Microsoft.AspNetCore.Authorization;
using FluentValidation.Results;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        IProductService _productService;
        ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var model = _productService.GetAll().Data;
            return View(model);
        }

        public ActionResult Add()
        {
            ViewBag.Categories = new SelectList(_categoryService.GetAll().Data, "CategoryId", "CategoryName");
            return View(new ProductViewModel());
        }

        [HttpPost]
        public ActionResult Add(ProductViewModel product)
        {
            var entity = new Product
            {
                ProductName = product.ProductName,
                CategoryId = product.CategoryId,
                Picture = product.Picture,
                UnitsInStock = product.UnitsInStock,
                Price = product.Price,
                Description = product.Description
            };
            var validator = new ProductValidator();
            ValidationResult validationResult = validator.Validate(entity);
            ViewBag.Categories = new SelectList(_categoryService.GetAll().Data, "CategoryId", "CategoryName");
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View();
            }

            var result = _productService.Add(entity);
            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("Index");
            }
            ViewBag.ErrorMessage = result.Message;
            return View();

        }

        public ActionResult Update(int id)
        {
            var product = _productService.GetById(id).Data;
            ViewBag.Categories = new SelectList(_categoryService.GetAll().Data, "CategoryId", "CategoryName");
            var model = new ProductUpdateViewModel()
            {
                ProductName = product.ProductName,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Picture = product.Picture,
                Price = product.Price,
                ProductId = product.ProductId,
                UnitsInStock = product.UnitsInStock

            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(ProductUpdateViewModel product)
        {
            var updatedProduct = _productService.GetById(product.ProductId).Data;
            updatedProduct.ProductName = product.ProductName;
            updatedProduct.CategoryId = product.CategoryId;
            updatedProduct.Picture = product.Picture;
            updatedProduct.UnitsInStock = product.UnitsInStock;
            updatedProduct.Price = product.Price;
            updatedProduct.Description = product.Description;

            var validator = new ProductValidator();
            ValidationResult validationResult = validator.Validate(updatedProduct);
            ViewBag.Categories = new SelectList(_categoryService.GetAll().Data, "CategoryId", "CategoryName");
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(product);
            }

            var result = _productService.Update(updatedProduct);
            if (result.Success)
            {
                TempData["UpdateSuccessMessage"] = result.Message;
                return RedirectToAction("Index");
            }
            ViewBag.ErrorMessage = result.Message;
            return View(product);

        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _productService.GetById(id)?.Data;

            if (product == null)
            {
                TempData["DeleteError"] = "Ürün bulunamadı.";
                return RedirectToAction("Index");
            }

            var result = _productService.Delete(product);

            if (result.Success)
            {
                TempData["DeleteSuccess"] = "Ürün başarıyla silindi.";
            }
            else
            {
                TempData["DeleteError"] = "Ürün silinemedi.";
            }

            return RedirectToAction("Index");
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
