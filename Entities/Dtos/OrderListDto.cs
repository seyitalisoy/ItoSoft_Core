using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;

namespace Entities.Dtos
{
    public class OrderListDto
    {
        public string UserId { get; set; }
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string ProductPicture { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductAmount { get; set; }
        public decimal TotalPrice { get; set; } 
        public string Adress { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerPhone { get; set; }
        public int ProductStock { get; set; }
    }
}
