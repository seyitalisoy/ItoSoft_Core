
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentValidation;
using Core.Aspects.Autofac.Validation;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product entity)
        {            
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

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product entity)
        {
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
