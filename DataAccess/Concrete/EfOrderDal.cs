using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;

namespace DataAccess.Concrete
{
    public class EfOrderDal : EfEntityRepositoryBase<Order, ECommerceContext>, IOrderDal
    {
        public List<OrderListDto> GetOrderDetails()
        {
            using (ECommerceContext context = new ECommerceContext())
            {
                var result = from o in context.Orders
                             join p in context.Products
                             on o.ProductId equals p.ProductId
                             join c in context.Categories 
                             on p.CategoryId equals c.CategoryId

                             select new OrderListDto
                             {
                                 UserId = o.UserId,
                                 OrderId = o.OrderId,
                                 ProductName = p.ProductName,
                                 CustomerEmail = o.Email,
                                 CustomerFirstName = o.FirstName,
                                 CustomerLastName = o.LastName,
                                 CustomerPhone = o.Phone,
                                 OrderDate = o.OrderDate,
                                 ProductAmount = o.ProductAmount,
                                 TotalPrice = o.Price,
                                 Adress = o.Adress,
                                 CategoryName = c.CategoryName,
                                 ProductPicture = p.Picture,
                                 ProductStock = p.UnitsInStock,
                             };

                return result.ToList();
            }
        }

    }
}
