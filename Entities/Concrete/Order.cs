using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Concrete
{
    public class Order : IEntity
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductAmount { get; set; }
        public decimal Price { get; set; }
        public string Adress { get; set; }
        public string AdressTitle { get; set; }
        public string UserName { get; set; }


    }
}
