using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.AutoMapper.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {            
            //CreateMap<Product, ProductUpdateDto>();
            CreateMap<ProductUpdateDto, Product>(); // first coppied to second
        }
    }
}
