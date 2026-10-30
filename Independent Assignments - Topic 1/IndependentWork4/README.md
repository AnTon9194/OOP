# Звіт з аналізу інкапсуляції в Open-Source проєкті

**Дисципліна:** Об'єктно-орієнтоване програмування  
**Тема:** Аналіз інкапсуляції в open-source проєктах. Практики валідації полів  
**Виконав:** Студент групи (вказати групу)  

---

## 1. Обраний проєкт

- **Назва:** Newtonsoft.Json (Json.NET)
- **Посилання на GitHub:** [https://github.com/JamesNK/Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
- **Опис проєкту:** Популярна бібліотека високої продуктивності для роботи з форматом JSON у середовищі .NET.

---

## 2. Аналіз інкапсуляції

### Клас 1: `JsonSerializerSettings`
- **Посилання на файл:** [Src/Newtonsoft.Json/JsonSerializerSettings.cs](https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/JsonSerializerSettings.cs)
- **Опис класу:** Відповідає за конфігурацію процесів серіалізації та десеріалізації об'єктів (налаштування форматування, обробка помилок, дати тощо).
- **Поля:** 
  Приватні закриті поля (backing fields), що забезпечують збереження внутрішнього стану та дефолтних значень:
  ```csharp
  internal Formatting? _formatting;
  internal NullValueHandling? _nullValueHandling;
  internal CultureInfo _culture;
  ```
- **Властивості:** 
  Властивості типу `read-write` із явно визначеними аксесорами або авто-властивості з дефолтними значеннями:
  ```csharp
  public Formatting Formatting
  {
      get => _formatting ?? Formatting.None;
      set => _formatting = value;
  }

  public CultureInfo Culture
  {
      get => _culture ?? CultureInfo.CurrentCulture;
      set => _culture = value;
  }
  ```
- **Індексатори/Оператори:** Відсутні, оскільки клас виконує виключно роль конфігураційного DTO/Settings об'єкта.

---

### Клас 2: `JsonProperty`
- **Посилання на файл:** [Src/Newtonsoft.Json/Serialization/JsonProperty.cs](https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Serialization/JsonProperty.cs)
- **Опис класу:** Представляє метадані та правила відображення окремої властивості або поля C# класу у відповідне JSON-поле.
- **Поля:** 
  Використовуються приватні поля з префіксом `_` для запобігання несанкціонованій зміні стану поза межами класу:
  ```csharp
  private string? _propertyName;
  private Type? _propertyType;
  private Required? _required;
  ```
- **Властивості:** 
  Поєднує звичайні публічні авто-властивості та властивості із захищеними аксесорами (наприклад, `get; set;` з валідацією):
  ```csharp
  public string? PropertyName
  {
      get => _propertyName;
      set => _propertyName = value;
  }

  public Required Required
  {
      get => _required ?? Required.Default;
      set => _required = value;
  }
  ```
- **Індексатори/Оператори:** Не використовуються.

---

### Клас 3: `JValue`
- **Посилання на файл:** [Src/Newtonsoft.Json/Linq/JValue.cs](https://github.com/JamesNK/Newtonsoft.Json/blob/master/Src/Newtonsoft.Json/Linq/JValue.cs)
- **Опис класу:** Представляє скалярне значення в LINQ to JSON (число, рядок, булеве значення, дата тощо).
- **Поля:** 
  Приватні поля для збереження типу JSON-значення та безпосередньо об'єкта даних:
  ```csharp
  private JTokenType _valueType;
  private object? _value;
  ```
- **Властивості:** 
  Захищені властивості тільки для читання або з контрольованим доступом:
  ```csharp
  public override JTokenType Type => _valueType;

  public new object? Value
  {
      get => _value;
      set
      {
          // Логіка перевірки та зміни внутрішнього типу даних
          Type currentType = _value?.GetType();
          Type newType = value?.GetType();
          if (currentType != newType)
          {
              _valueType = GetValueType(_valueType, value);
          }
          _value = value;
      }
  }
  ```
- **Індексатори/Оператори:** 
  Перевантажено неявні (`implicit`) та явні (`explicit`) оператори приведення типів для зручної роботи з JSON-скалярами:
  ```csharp
  public static implicit operator JValue(string? value) => new JValue(value);
  public static implicit operator JValue(long value) => new JValue(value);
  public static explicit operator string?(JValue value) => (string?)value.Value;
  ```

---

## 3. Практики валідації у властивостях

### Приклад 1: Перевірка на Null та викидання винятків (`ArgumentNullException`)
У класі `JsonSerializer` або допоміжних сервісах валідація вхідних даних здійснюється під час встановлення значень властивостей або через конструктор:
```csharp
private string _propertyName;

public string PropertyName
{
    get => _propertyName;
    set
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "PropertyName cannot be null.");
        }
        _propertyName = value;
    }
}
```

### Приклад 2: Валідація діапазонів значень та граничних умов
У властивостях, які приймають числові значення (наприклад, `MaxDepth` у налаштуваннях зчитування JSON):
```csharp
private int? _maxDepth;

public int? MaxDepth
{
    get => _maxDepth;
    set
    {
        if (value <= 0)
        {
            throw new ArgumentException("Value must be positive.", nameof(value));
        }
        _maxDepth = value;
    }
}
```

### Висновки щодо валідації:
1. **Захист інваріантів:** Валідація безпосередньо в `set`-аксесорах гарантує, що об'єкт ніколи не опиниться в некоректному або зламаному стані.
2. **Швидке виявлення помилок (Fail-Fast):** Використання `ArgumentNullException` або `ArgumentException` дозволяє розробнику негайно дізнатися про помилку на етапі виклику.
3. **Гнучкість:** Для комплексних об'єктів Newtonsoft.Json комбінує перевірки в setter з атрибутами та кастомними конвертерами.

---

## 4. Загальні висновки

1. Проект Newtonsoft.Json демонструє класичний та високоефективний підхід до інкапсуляції в .NET.
2. Закриття полів (`private` / `internal`) та надання доступу через властивості дозволяє приховати внутрішню реалізацію та зберегти зворотну сумісність.
3. Перевантаження операторів у класах на зразок `JValue` значно спрощує синтаксис використання бібліотеки без порушення безпеки типів.

---

# Контрольні запитання та відповіді

### 1. За якими ознаками можна зробити висновок, що в класі дотримано інкапсуляції?
- **Приватність полів:** Всі дані (стан об'єкта) оголошені як `private` або `protected`. Публічні поля відсутні або зведені до констант (`const` / `readonly`).
- **Контрольований доступ:** Доступ до даних здійснюється виключно через публічні властивості (`properties`) або методи-гетери/сетери.
- **Захист цілісності (інваріанту):** Аксесор `set` містить перевірки/валідацію, що унеможливлює встановлення некоректних даних.
- **Приховування деталей реалізації:** Внутрішня логіка збереження чи обчислення даних прихована від зовнішнього користувача класу.

### 2. Які підходи до валідації властивостей ви зустріли в реальному коді?
- **Перевірка у `set`-аксесорі:** Використання умов `if` з викиданням винятків (`ArgumentNullException`, `ArgumentOutOfRangeException`, `InvalidOperationException`).
- **Guards / Guard Clauses:** Використання системних методів валідації на кшталт `ArgumentNullException.ThrowIfNull(value)`.
- **Атрибути валідації:** Використання `[Required]`, `[Range]`, `[StringLength]` з `System.ComponentModel.DataAnnotations`.
- **Fluent Validation / Валідатори:** Винесення логіки валідації в окремі класи-валідатори для збереження чистоти доменної моделі.

### 3. Чому для аналізу важливо наводити посилання на конкретні класи та фрагменти коду?
- **Верифікованість:** Посилання на GitHub підтверджують, що аналіз проведено на основі діючого продуктового коду, а не теоретичних прикладів.
- **Наочність:** Фрагменти коду ілюструють практичне застосування паттернів інкапсуляції у реальних архітектурних умовах.
- **Точність:** Дозволяє оцінити контекст (наприклад, чому обрано саме `internal` замість `private` чи чому використано `read-only` властивість).

### 4. Які висновки щодо якості проєктування ви зробили після порівняння 2-3 класів?
- **Розподіл відповідальностей (SRP):** Кожен клас має чітку зону відповідальності (наприклад, `JsonSerializerSettings` — конфігурація, `JsonProperty` — метадані, `JValue` — узгодження типів даних).
- **Баланс між гнучкістю та захистом:** Класи надають зручний API (через авто-властивості та оператори), але зберігають строгий контроль над внутрішнім станом.
- **Оптимізація:** У високопродуктивних бібліотеках інкапсуляція допомагає ледаче (lazy) обчислювати значення або кешувати результати без зміни публічного API.
