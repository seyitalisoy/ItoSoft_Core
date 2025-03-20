using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules;
using Core.Entities;
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
                return new ErrorResult("Ürün Eklenmedi");
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
            return new SuccessResult("Ürün silindi.");
        }

        public IDataResult<Product> GetById(int id)
        {
            var result = _productDal.Get(p => p.ProductId == id);
            if (result!=null)
            {
                return new SuccessDataResult<Product>(result);
            }
            return new ErrorDataResult<Product>("Ürün bulunamadı");
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
                return new ErrorResult("Güncelleme başarısız.");
            }
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

        public IResult DeleteById(int id)
        {
            var result = _productDal.Get(p => p.ProductId == id);
            if (result!=null)
            {
                _productDal.Delete(result);
                return new SuccessResult("Ürün silindi.");
            }
            return new ErrorResult("Ürün bulunamadı.");
        }
    }
}
