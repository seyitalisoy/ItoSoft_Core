using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Entities.Dtos
{
    public class ProductUpdateDto : IDto
    {
        //Adı, Fotosu, Kategorisi ve Idsi değişmeyecek
        public decimal Price { get; set; }
        public int UnitsInStock { get; set; }
        public string Description { get; set; }
    }
}
