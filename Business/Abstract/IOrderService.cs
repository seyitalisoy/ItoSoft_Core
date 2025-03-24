using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface IOrderService
    {
        IDataResult<List<Order>> GetAll();

        IDataResult<Order> GetById(int id);
        IResult Add(Order entity);
        IResult Delete(Order entity);
        IResult Update(Order entity);
        IDataResult<List<OrderListDto>> GetOrderDetails();

        IDataResult<List<OrderListDto>> GetByOrderId(int id);
        IDataResult<List<OrderListDto>> GetByOrderDate(DateTime date);
        IDataResult<List<OrderListDto>> GetByEmail(string email);
    }
}
