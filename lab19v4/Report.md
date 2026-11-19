# Звіт до лабораторної роботи №13 (Проєкт `lab19v4`)

**Тема:** Власні винятки та патерн Result.  
**Варіант:** 4 (Сценарій: Валідація даних).  
**Мета роботи:** Навчитися створювати власні типи винятків для специфічних помилок, а також реалізовувати патерн `Result` для більш контрольованої обробки помилок без використання винятків у «нормальних» сценаріях невдачі.

---

## 1. Структура та код проєкту

### `Result.cs`
```csharp
using System;

namespace lab19v4
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string Error { get; }

        private Result(T? value, bool isSuccess, string error)
        {
            Value = value;
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }

        public static Result<T> Failure(string error)
        {
            return new Result<T>(default, false, error);
        }

        public static Result<T> Failure(Exception ex)
        {
            return new Result<T>(default, false, ex.Message);
        }
    }
}
```

### `ValidationException.cs`
```csharp
using System;
using System.Collections.Generic;

namespace lab19v4
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; }

        public ValidationException() 
            : base("Виникла помилка валідації даних.")
        {
            Errors = new List<string>();
        }

        public ValidationException(string message) 
            : base(message)
        {
            Errors = new List<string> { message };
        }

        public ValidationException(string message, Exception innerException) 
            : base(message, innerException)
        {
            Errors = new List<string> { message };
        }

        public ValidationException(List<string> errors) 
            : base("Об'єкт не пройшов валідацію. Перевірте список помилок.")
        {
            Errors = errors ?? new List<string>();
        }
    }
}
```

### `Product.cs`
```csharp
namespace lab19v4
{
    public class Product
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
```

### `ValidatorService.cs`
```csharp
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
```

### `Program.cs`
```csharp
using System;
using System.Text;

namespace lab19v4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var validator = new ValidatorService();

            Console.WriteLine("=== 1. Демонстрація роботи з власним винятком (ValidateProduct) ===\n");

            var validProduct = new Product { Name = "Ноутбук ASUS", Price = 25000m, StockQuantity = 5 };
            try
            {
                validator.ValidateProduct(validProduct);
                Console.WriteLine($"[Успіх] Продукт '{validProduct.Name}' успішно пройшов валідацію.\n");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"[Помилка] {ex.Message}");
            }

            var invalidProduct = new Product { Name = "ПК", Price = -100m, StockQuantity = -2 };
            try
            {
                validator.ValidateProduct(invalidProduct);
                Console.WriteLine($"[Успіх] Продукт '{invalidProduct.Name}' пройшов валідацію.");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"[Перехоплено ValidationException]: {ex.Message}");
                foreach (var error in ex.Errors)
                {
                    Console.WriteLine($"  - {error}");
                }
            }

            Console.WriteLine("\n=== 2. Демонстрація роботи з патерном Result<T> (IsEmailValid) ===\n");

            string[] emailsToTest = { "student@university.edu.ua", "invalid-email.com", "" };

            foreach (var email in emailsToTest)
            {
                Result<bool> result = validator.IsEmailValid(email);

                if (result.IsSuccess)
                {
                    Console.WriteLine($"[Успіх] Email '{email}' валідний (Value: {result.Value}).");
                }
                else
                {
                    Console.WriteLine($"[Невдача] {result.Error}");
                }
            }
        }
    }
}
```

---

## 2. Результат роботи програми (Консольний вивід)

```text
=== 1. Демонстрація роботи з власним винятком (ValidateProduct) ===

[Успіх] Продукт 'Ноутбук ASUS' успішно пройшов валідацію.

[Перехоплено ValidationException]: Об'єкт не пройшов валідацію. Перевірте список помилок.
  - Назва продукту повинна містити щонайменше 3 символи.
  - Ціна продукту повинна бути більшою за 0.
  - Кількість на складі не може бути від'ємною.

=== 2. Демонстрація роботи з патерном Result<T> (IsEmailValid) ===

[Успіх] Email 'student@university.edu.ua' валідний (Value: True).
[Невдача] Некоректний формат email: 'invalid-email.com'. Очікується формат user@domain.com.
[Невдача] Email не може бути порожнім.
```

---

## 3. Висновок

У ході виконання лабораторної роботи було досліджено та реалізовано два підходи до обробки помилок у C# на прикладі сервісу валідації `ValidatorService`:

1. **Власний виняток `ValidationException` у методі `ValidateProduct(Product)`:** Метод має тип повернення `void` і виступає захисним бар'єром перед виконанням критичної бізнес-логіки (наприклад, збереженням товару в БД). Якщо об'єкт `Product` невалідний, подальша робота з ним є неможливою. Викидання `ValidationException` дозволяє негайно перервати потік виконання та передати повний перелік помилок (`List<string> Errors`) на вищий рівень для обробки.
2. **Патерн `Result<bool>` у методі `IsEmailValid(string)`:** Помилка введення електронної пошти користувачем є очікуваним, рутинним сценарієм, який виникає часто. Використання патерну `Result<T>` дозволяє уникнути накладних витрат продуктивності, пов'язаних із генеруванням винятків та збором стек-трейсу, а також робить контракт методу прозорим — викликаючий код зобов'язаний перевірити властивість `IsSuccess` перед використанням результату.

---

## 4. Відповіді на контрольні запитання

### 1. Навіщо створювати власні типи винятків? Які переваги це дає?
Власні винятки створюють для представлення специфічних помилок предметної області (бізнес-логіки), коли стандартних класів винятків .NET (`ArgumentException`, `InvalidOperationException` тощо) недостатньо для точного опису проблеми.
* **Переваги:**
  * **Точне перехоплення:** Можливість обробляти специфічні помилки в окремих блоках `catch (ValidationException ex)` без ризику перехопити системні збої.
  * **Додатковий контекст:** Можливість розширити клас власними властивостями (наприклад, `List<string> Errors`, код помилки, ідентифікатор сутності).
  * **Покращена читабельність:** Назва класу винятку одразу вказує на причину збою.

### 2. Які стандартні конструктори рекомендується реалізовувати для власних винятків?
Згідно з рекомендаціями Microsoft, власний клас винятку повинен містити три стандартні конструктори, які викликають відповідні конструктори базового класу `Exception`:
1. Конструктор без параметрів: `public MyException()`
2. Конструктор із текстовим повідомленням: `public MyException(string message) : base(message)`
3. Конструктор із повідомленням та посиланням на внутрішній виняток (для збереження першопричини помилки): `public MyException(string message, Exception innerException) : base(message, innerException)`

### 3. Поясніть патерн Result. У яких випадках його використання є більш доцільним, ніж викидання винятків?
**Патерн Result** — це об'єкт-обгортка (`Result<T>`), який повертається методом замість викидання винятку та містить прапорець успішності (`IsSuccess`), корисне навантаження (`Value`) у разі успіху або опис помилки (`Error`) у разі невдачі.
Його використання доцільне в **очікуваних, штатних сценаріях невдачі** (невалідне введення користувача, помилка авторизації, відсутність елемента під час пошуку), де невдача є частиною звичайного потоку виконання програми, а не аварійною ситуацією.

### 4. Які сценарії краще обробляти через винятки, а які через патерн Result?

| Критерій | Винятки (`Exception`) | Патерн `Result<T>` |
| :--- | :--- | :--- |
| **Характер ситуації** | Непередбачувані, критичні або технічні збої (втрата з'єднання з БД, відсутність файлу, `NullReferenceException`). | Очікувані помилки валідації або бізнес-правил (неправильний формат email, недостатньо коштів на рахунку). |
| **Частота виникнення** | Рідкісні, виняткові події. | Часті події (рутинні перевірки користувацьких даних). |
| **Вплив на виконання** | Потрібно аварійно перервати поточну операцію та передати керування вгору по стеку викликів. | Потрібно плавно обробити помилку на місці без втрати продуктивності на збір стек-трейсу. |
