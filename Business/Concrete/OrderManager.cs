using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Concrete
{
    public class OrderManager : IOrderService
    {
        IOrderDal _orderDal;

        public OrderManager(IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }

        public IResult Add(Order entity)
        {
            _orderDal.Add(entity);
            return new SuccessResult();
        }

        public IResult Delete(Order entity)
        {
            _orderDal.Add(entity);
            return new SuccessResult();
        }

        public IDataResult<List<Order>> GetAll()
        {
            return new SuccessDataResult<List<Order>>(_orderDal.GetAll());
        }

        public IDataResult<List<OrderListDto>> GetByEmail(string email)
        {
            var result = _orderDal.GetOrderDetails().Where(o=>o.CustomerEmail.Contains(email)).ToList();
            if (result.Any())
            {
                return new SuccessDataResult<List<OrderListDto>>(result);
            }
            return new ErrorDataResult<List<OrderListDto>>("Girdiğiniz mail ile uyuşan mail adresi bulunmuyor.");
        }

        public IDataResult<Order> GetById(int id)
        {
            return new SuccessDataResult<Order>(_orderDal.Get(o => o.OrderId == id));
        }

        public IDataResult<List<OrderListDto>> GetByOrderDate(DateTime date)
        {
            var result = _orderDal.GetOrderDetails()
                .Where(o => o.OrderDate.Day == date.Day &&
                            o.OrderDate.Month == date.Month &&
                            o.OrderDate.Year == date.Year)
                .ToList();
            if (result.Any())
            {
                return new SuccessDataResult<List<OrderListDto>>(result);
            }
            return new ErrorDataResult<List<OrderListDto>>("Bu tarihte ürün bulunamadı");

            
        }


        public IDataResult<List<OrderListDto>> GetByOrderId(int id)
        {
            var result = _orderDal.GetOrderDetails().Where(o => o.OrderId == id).ToList();
            if (result.Any())
            {
                return new SuccessDataResult<List<OrderListDto>>(result);
            }
            return new ErrorDataResult<List<OrderListDto>>("Girdiğiniz sipariş numarası bulunmuyor");
        }

        public IDataResult<List<OrderListDto>> GetOrderDetails()
        {
            return new SuccessDataResult<List<OrderListDto>>(_orderDal.GetOrderDetails().OrderBy(o=>o.OrderId).ToList());
        }

        public IResult Update(Order entity)
        {
            _orderDal.Add(entity);
            return new SuccessResult();
        }
    }
}
