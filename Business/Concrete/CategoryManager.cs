using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentValidation.Results;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public IResult Add(Category entity)
        {
            var validator = new CategoryValidator();
            ValidationResult validationResult = validator.Validate(entity);
            if (!validationResult.IsValid)
            {
                //string errorMessages = string.Join("\n", validationResult.Errors.Select(e => e.ErrorMessage));
                return new ErrorResult();
            }

            IResult result = BusinessRules.Run(CheckIfCategoryNameExist(entity.CategoryName), CheckIfCategoryLimitExceded());
            if (result!=null)
            {
                return result;
            }
            _categoryDal.Add(entity);
            return new SuccessResult(Messages.CategoryAdded);
        }

        public IResult Delete(Category entity)
        {
            _categoryDal.Delete(entity);
            return new SuccessResult();
        }

        public IDataResult<List<Category>> GetAll()
        {
            return new SuccessDataResult<List<Category>>(_categoryDal.GetAll());
        }

        public IDataResult<Category> GetById(int id)
        {
            return new SuccessDataResult<Category>(_categoryDal.Get(c => c.CategoryId == id));
        }

        public IResult Update(Category entity)
        {
            _categoryDal.Update(entity);
            return new SuccessResult();
        }

        private IResult CheckIfCategoryNameExist(string categoryName)
        {
            var result = _categoryDal.GetAll(c=>c.CategoryName==categoryName).Any();
            if (result)
            {
                return new ErrorResult(Messages.CategoryNameAlreadyExists);
            }
            return new SuccessResult();
        }
        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryDal.GetAll().Count();
            if (result>=6)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }
    }
}
