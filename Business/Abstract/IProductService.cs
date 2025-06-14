﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IProductService
    {
        IDataResult<List<Product>> GetAll();
        
        IDataResult<Product> GetById(int id);
        IResult Add(Product entity);
        IResult Delete(Product entity);
        IResult Update(Product entity);
        IResult DeleteById(int id);
        IDataResult<List<Product>> GetByCategoryId(int categoryId);
    }
}
