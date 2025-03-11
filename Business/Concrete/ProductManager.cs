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
    public class ProductManager : IProductService
    {
        IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public IResult Add(Product entity)
        {
            var validator = new ProductValidator();
            ValidationResult validationResult = validator.Validate(entity);
            if (!validationResult.IsValid)
            {
                //string errorMessages = string.Join("\n", validationResult.Errors.Select(e => e.ErrorMessage));
                return new ErrorResult();
            }
            var result = BusinessRules.Run(CheckIfProductNameExist(entity.ProductName));
            if (result != null)
            {
                return result;
            }
            _productDal.Add(entity);
            return new SuccessResult(Messages.ProductAdded);
        }

        public IResult Delete(Product entity)
        {
            _productDal.Delete(entity);
            return new SuccessResult();
        }

        public IDataResult<Product> GetById(int id)
        {

            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == id));
        }

        public IDataResult<List<Product>> GetAll()
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll());
        }

        public IResult Update(Product entity)
        {
            var validator = new ProductValidator();
            ValidationResult validationResult = validator.Validate(entity);
            if (!validationResult.IsValid)
            {
                //string errorMessages = string.Join("\n", validationResult.Errors.Select(e => e.ErrorMessage));
                return new ErrorResult();
            }
            //var result = BusinessRules.Run(CheckIfProductNameExist(entity.ProductName));
            //if (result != null)
            //{
            //    return result;
            //}
            _productDal.Update(entity);
            return new SuccessResult("Ürün başarıyla güncellendi.");
        }

        private IResult CheckIfProductNameExist(string productName)
        {
            var result = _productDal.GetAll().Where(p=>p.ProductName==productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();

        }


    }
}
