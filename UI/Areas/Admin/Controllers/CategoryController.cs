using Business.Abstract;
using Business.ValidationRules;
using Entities.Concrete;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Areas.Admin.Models.Categories;
using UI.Models.Entities;

namespace UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ActionResult Index()
        {
            var model = _categoryService.GetAll().Data;
            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Category category)
        {

            category.Picture = "Örnek Url";
            var validator = new CategoryValidator();
            ValidationResult validationResult = validator.Validate(category);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View();
            }

            var result = _categoryService.Add(category);
            if (result.Success)
            {
                ViewBag.SuccessMessage = result.Message;
                return RedirectToAction("Index");
            }
            ViewBag.ErrorMessage = result.Message;
            return View();
        }

        public ActionResult Update(int id)
        {
            var category = _categoryService.GetById(id).Data;
            var model = new CategoryUpdateViewModel()
            {
                CategoryId = category.CategoryId,
                Name = category.CategoryName,
                Description = category.Description
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Update(CategoryUpdateViewModel category)
        {
            var updatedCategory = _categoryService.GetById(category.CategoryId).Data;
            updatedCategory.CategoryName = category.Name;
            updatedCategory.Description = category.Description;

            var validator = new CategoryValidator();
            ValidationResult validationResult = validator.Validate(updatedCategory);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(category);
            }

            var result = _categoryService.Update(updatedCategory);
            if (result.Success)
            {
                TempData["UpdateSuccessMessage"] = result.Message;
                return RedirectToAction("Index");
            }
            ViewBag.ErrorMessage = result.Message;
            return View(category);

        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _categoryService.GetById(id)?.Data;

            if (category == null)
            {
                TempData["DeleteError"] = "Kategori bulunamadı.";
                return RedirectToAction("Index");
            }

            var result = _categoryService.Delete(category);

            if (result.Success)
            {
                TempData["DeleteSuccess"] = "Kategori başarıyla silindi.";
            }
            else
            {
                TempData["DeleteError"] = "Kategori silinemedi.";
            }

            return RedirectToAction("Index");
        }

    }
}
