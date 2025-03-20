using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrete
{
    //database temizle
    public class Order : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductAmount { get; set; }
        public decimal Price { get; set; } //total
        public string Adress { get; set; }
        public string AdressTitle { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

    }
}
