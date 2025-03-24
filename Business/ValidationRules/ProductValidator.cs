using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.ProductName).NotEmpty().WithMessage("Ürün adı boş geçilemez");
            RuleFor(p => p.ProductName).MinimumLength(3).WithMessage("Ürün adı en az 3 karakter olmalı.");
            RuleFor(p => p.ProductName).MaximumLength(50).WithMessage("Ürün adı en fazla 50 karakter olmalı.");

            RuleFor(p => p.CategoryId).NotEmpty().WithMessage("Kategori boş geçilemez");

            RuleFor(p => p.UnitsInStock).NotEmpty().WithMessage("Stok miktarı boş geçilemez");
            RuleFor(p => p.UnitsInStock).LessThan(100).WithMessage("Stok miktarı 100'den az olmalı.");
            RuleFor(p => p.UnitsInStock).GreaterThan(0).WithMessage("Stok miktarı 0'dan fazla olmalı.");

            RuleFor(p => p.Price).NotEmpty().WithMessage("Ürün fiyatı boş geçilemez");
            RuleFor(p => p.Price).GreaterThan(0).WithMessage("Ürün fiyatı boş geçilemez");
            RuleFor(p => p.Price).LessThan(100000).WithMessage("Ürün fiyatı 100000'den az olmalı.");

            RuleFor(p => p.Description).NotEmpty().WithMessage("Açıklama boş geçilemez");
            RuleFor(p => p.Description).MinimumLength(3).WithMessage("Açıklama en az 3 karakter olmalı.");
            RuleFor(p => p.Description).MaximumLength(500).WithMessage("Açıklama en fazla 50 karakter olmalı.");
        }
    }
}
