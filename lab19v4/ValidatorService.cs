using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace lab19v4
{
    public class ValidatorService
    {
        public void ValidateProduct(Product? product)
        {
            if (product == null)
            {
                throw new ValidationException("Продукт не може бути null.");
            }

            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                errors.Add("Назва продукту не може бути порожньою.");
            }
            else if (product.Name.Length < 3)
            {
                errors.Add("Назва продукту повинна містити щонайменше 3 символи.");
            }

            if (product.Price <= 0)
            {
                errors.Add("Ціна продукту повинна бути більшою за 0.");
            }

            if (product.StockQuantity < 0)
            {
                errors.Add("Кількість на складі не може бути від'ємною.");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }
        }

        public Result<bool> IsEmailValid(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Result<bool>.Failure("Email не може бути порожнім.");
            }

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, pattern))
            {
                return Result<bool>.Failure($"Некоректний формат email: '{email}'. Очікується формат user@domain.com.");
            }

            return Result<bool>.Success(true);
        }
    }
}
