using Business.Abstract;
using Business.ValidationRules;
using Entities.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class CategoryController : Controller
    {
        ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ActionResult Index()
        {
            //ModelState.Clear();
            var model = _categoryService.GetAll().Data;
            return View(model);
        }

        //[HttpPost]
        //public ActionResult Add(Category category)
        //{
        //    var model = new Category { CategoryName = category.CategoryName, Description = category.Description };
        //    var result = _categoryService.Add(model);
        //    if (!result.Success)
        //    {
        //        ViewBag.ErrorMessage = result.Message;
        //        return View("Index", _categoryService.GetAll().Data);
        //    }
        //    ViewBag.SuccessMessage = result.Message;
        //    return View("Index", _categoryService.GetAll().Data);
        //}

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Category category)
        {
            //category.Picture = "Örnek Url";
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
            }
            return RedirectToAction("Index");
        }
        //[HttpPost]
        //public ActionResult Add(Category category)
        //{
        //    var validator = new CategoryValidator();
        //    ValidationResult validationResult = validator.Validate(category);

        //    if (!validationResult.IsValid)
        //    {
        //        foreach (var error in validationResult.Errors)
        //        {
        //            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        //        }

        //        return View("Index", _categoryService.GetAll().Data);
        //    }

        //    var result = _categoryService.Add(category);
        //    if (result.Success)
        //    {
        //        ViewBag.SuccessMessage = result.Message;
        //    }
        //    return View("Index", _categoryService.GetAll().Data);
        //}

    }
}
