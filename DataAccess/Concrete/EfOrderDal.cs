using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;

namespace DataAccess.Concrete
{
    public class EfOrderDal : EfEntityRepositoryBase<Order, ECommerceContext>, IOrderDal
    {
        //public List<OrderListDto> GetOrderDetails()
        //{
        //    using (MarketContext context = new MarketContext())
        //    {
        //        var result = from o in context.Orders
        //                     join c in context.Customers
        //                     on o.CustomerId equals c.Id
        //                     join p in context.Products
        //                     on o.ProductId equals p.Id

        //                     select new OrderListDto
        //                     {
        //                         OrderId = o.OrderId,
        //                         ProductName = p.Name,
        //                         CustomerEmail = c.Email,
        //                         CustomerPhone = c.Phone,
        //                         OrderDate = o.OrderDate,
        //                         Amount = o.ProductAmount,
        //                         Price = o.Price,
        //                         Adress = o.Adress
        //                     };

        //        return result.ToList();
        //    }
        //}

    }
}
