
using Business.Abstract;
using Business.Constants;
using Business.ValidationRules;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentValidation;
using Core.Aspects.Autofac.Validation;
using Business.BusinessAspects.Autofac;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Performance;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]        
        public IResult Add(Product entity)
        {            
            var result = BusinessRules.Run(CheckIfProductNameExist(entity.ProductName));
            if (result != null)
            {
                //return result;
                // Use 'throw' instead of 'return' to trigger rollback in methods using TransactionScopeAspect
                throw new Exception("Ürün ekleme başarısız."); 
            }
            _productDal.Add(entity);
            return new SuccessResult(Messages.ProductAdded);
        }

        [CacheRemoveAspect("IProductService.Get")]
        public IResult Delete(Product entity)
        {
            _productDal.Delete(entity);
            return new SuccessResult("Ürün silindi.");
        }

        [CacheAspect]
        public IDataResult<Product> GetById(int id)
        {
            var result = _productDal.Get(p => p.ProductId == id);
            if (result!=null)
            {
                return new SuccessDataResult<Product>(result);
            }
            return new ErrorDataResult<Product>("Ürün bulunamadı");
        }

        [CacheAspect]
        [PerformanceAspect(3)]        
        public IDataResult<List<Product>> GetAll()
        {
            //Thread.Sleep(4000);  --> PerformenceAspect testing

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(),"ürünler yüklendi");
        }

        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        //[TransactionScopeAspect]
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

        [CacheRemoveAspect("IProductService.Get")]
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
