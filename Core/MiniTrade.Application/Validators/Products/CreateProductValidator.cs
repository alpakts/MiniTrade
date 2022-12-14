using FluentValidation;
using MiniTrade.Domain.Entities;

//Validasyon işlemleri için fluentvalidation,fluentvalidation asp.net,fluentvalidaitondependecyinejction kütüphanesi yüklenmelidir 
namespace MiniTrade.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<Product>
  {
    public CreateProductValidator()
    {
      RuleFor(u => u.Name).NotEmpty().MinimumLength(2).NotNull().
        WithMessage("Ürün adı boş olamaz!").MaximumLength(150).WithMessage("ürün adı en fazla 150 karakter olabilir");

      RuleFor(u => u.Price).NotEmpty().NotNull().WithMessage("Fiyat Bilgisi gereklidir.")
        .Must(u=>u>=0).WithMessage("fiyat sıfırdan büyük olamlırıdr");
      RuleFor(u => u.Stock).NotEmpty()
        .NotNull().WithMessage("Stock Bilgisi gereklidir.").Must(u => u >= 0).WithMessage("stock sıfırdan büyük olmalıdır");
    }
  }
}
