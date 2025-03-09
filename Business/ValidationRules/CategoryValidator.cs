using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.CategoryName).NotEmpty().WithMessage("Kategori adı boş geçilemez.");
            RuleFor(c => c.CategoryName).MinimumLength(3).WithMessage("Kategori en az 3 karakter olmalı.");
            RuleFor(c => c.Description).NotEmpty().WithMessage("Açıklama boş geçilemez.");
            RuleFor(c => c.Description).MinimumLength(5).WithMessage("Açıklama en az 10 karakter olmalı.");
        }
    }
}
