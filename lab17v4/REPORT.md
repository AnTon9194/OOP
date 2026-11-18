# Звіт до лабораторної роботи №11

**Тема:** LINQ: SelectMany, пагінація й custom extensions
**Варіант:** 4 — Компанії та їхні співробітники з проєктами
**Проєкт:** `OOP-<ВашеПрізвище>/lab17v4`

## Мета

Навчитися використовувати `SelectMany` для роботи з вкладеними колекціями, реалізовувати пагінацію за допомогою LINQ та створювати власні методи розширення для `IEnumerable<T>`.

## 1. Сутності та тестові дані

```csharp
public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
}

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Project> Projects { get; set; } = new();
}

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Employee> Employees { get; set; } = new();
}
```

Структура вкладеності: `Company` → `List<Employee>` → `List<Project>`.

**Тестові дані:** 5 компаній, 10 співробітників, 20 проєктів.

| Компанія | Співробітник | Проєкти |
|---|---|---|
| TechNova | Олександр Коваль | 1, 2 |
| TechNova | Марія Шевченко | 3 |
| SoftLine | Іван Бондар | 4, 5, 6 |
| SoftLine | Олена Мельник | 7 |
| DataHub | Андрій Ткачук | 8, 9 |
| DataHub | Наталія Кравчук | 10, 11 |
| CloudWorks | Сергій Олійник | 12, 13 |
| CloudWorks | Ірина Поліщук | 14, 15 |
| GreenSoft | Василь Гончар | 16, 17 |
| GreenSoft | Юлія Лисенко | 18, 19, 20 |

## 2. Запит SelectMany

Отримати всі проєкти всіх співробітників усіх компаній в одному списку.

```csharp
List<Project> allProjects = companies
    .SelectMany(c => c.Employees)   // компанії -> співробітники
    .SelectMany(e => e.Projects)    // співробітники -> проєкти
    .ToList();
```

**Вивід:**

```
=== 1. Усі проєкти всіх співробітників усіх компаній ===
  [1] Project 1
  [2] Project 2
  ...
  [20] Project 20
Разом проєктів: 20
```

Перший `SelectMany` розгортає компанії в єдиний список співробітників, другий — співробітників в єдиний список проєктів.

## 3. Пагінація

Вивести компанії по 2 на сторінку.

```csharp
const int pageSize = 2;
int totalPages = (int)Math.Ceiling(companies.Count / (double)pageSize);

for (int page = 1; page <= totalPages; page++)
{
    Console.WriteLine($"--- Сторінка {page} з {totalPages} ---");
    foreach (Company c in companies.Page(page, pageSize))
        Console.WriteLine($"  {c.Id}. {c.Name} (співробітників: {c.Employees.Count})");
}
```

Метод `Page<T>` всередині використовує `Skip((pageNumber - 1) * pageSize).Take(pageSize)`.

**Вивід (3 сторінки поспіль):**

```
--- Сторінка 1 з 3 ---
  1. TechNova (співробітників: 2)
  2. SoftLine (співробітників: 2)
--- Сторінка 2 з 3 ---
  3. DataHub (співробітників: 2)
  4. CloudWorks (співробітників: 2)
--- Сторінка 3 з 3 ---
  5. GreenSoft (співробітників: 2)
```

Остання сторінка неповна, оскільки 5 компаній не діляться навпіл без залишку.

## 4. Власний метод розширення

```csharp
public static class CompanyExtensions
{
    public static int TotalEmployees(this IEnumerable<Company> companies)
    {
        if (companies == null) throw new ArgumentNullException(nameof(companies));
        return companies.Sum(c => c.Employees.Count);
    }
}
```

**Використання та вивід:**

```csharp
Console.WriteLine($"Загальна кількість співробітників: {companies.TotalEmployees()}");
Console.WriteLine($"Для перших двох компаній: {companies.Take(2).TotalEmployees()}");
```

```
=== 3. Extension TotalEmployees() ===
Загальна кількість співробітників: 10
Для перших двох компаній: 4
```

Метод інкапсулює бізнес-логіку підрахунку і працює в ланцюжках LINQ разом із `Take`, `Where` тощо.

## 5. Відповіді на контрольні питання

### 1. Для чого використовується `SelectMany`? У чому його відмінність від `Select`?

`SelectMany` проєктує кожен елемент вхідної колекції в іншу колекцію, а потім об'єднує (flatten) усі ці колекції в одну плоску послідовність. `Select` повертає рівно один результат для кожного елемента, тому якщо результат — колекція, виходить колекція колекцій.

```csharp
employees.Select(e => e.Projects);      // IEnumerable<List<Project>>  (вкладена структура)
employees.SelectMany(e => e.Projects);  // IEnumerable<Project>        (плоский список)
```

### 2. Як реалізувати пагінацію за допомогою LINQ?

Використовуються методи `Skip()` і `Take()`:

```csharp
items.Skip((page - 1) * pageSize).Take(pageSize);
```

- `Skip` пропускає елементи всіх попередніх сторінок;
- `Take` бере елементи поточної сторінки.

Кількість сторінок: `(int)Math.Ceiling(count / (double)pageSize)`. Для стабільного порядку елементів між сторінками бажано попередньо відсортувати колекцію (`OrderBy`).

### 3. Що таке методи розширення і як вони інтегруються з LINQ?

Метод розширення — це статичний метод у статичному класі, перший параметр якого має модифікатор `this`. Він додає функціональність до існуючого типу без зміни його коду й викликається як звичайний метод екземпляра.

LINQ побудований саме на методах розширення: `Where`, `Select`, `SelectMany`, `Skip`, `Take` та інші визначені в класі `Enumerable` (простір імен `System.Linq`) як розширення для `IEnumerable<T>`. Тому власні методи розширення для `IEnumerable<T>` повністю сумісні з LINQ і вільно комбінуються з ним у ланцюжках викликів.

### 4. Приклад власного методу розширення для `IEnumerable<T>`

```csharp
public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
{
    return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
}
```

**Призначення:** повертає елементи вказаної сторінки для будь-якої колекції, прибираючи дублювання логіки `Skip/Take` у коді. Виклик: `companies.Page(2, 2)`.

Другий приклад — `TotalEmployees()` з розділу 4, який обчислює загальну кількість співробітників у послідовності компаній.

## Висновок

У ході роботи реалізовано сутності з вкладеними колекціями, запит із `SelectMany` для розгортання вкладених списків, пагінацію на основі `Skip`/`Take` та власні методи розширення `TotalEmployees()` і `Page<T>()`. Усі три механізми продемонстровано в `Main`.
