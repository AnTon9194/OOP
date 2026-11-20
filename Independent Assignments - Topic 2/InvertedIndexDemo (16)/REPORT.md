# Самостійна робота №16 — Інвертований індекс і вимірювання продуктивності

## Реалізація
| Файл | Призначення |
|---|---|
| `Document.cs` | модель документа (`Id`, `Title`, `Content`) |
| `TextProcessor.cs` | `Tokenize` (regex `\w+#?`, нижній регістр) і `RemoveStopWords` |
| `InvertedIndex.cs` | `Dictionary<string, HashSet<int>>` + `Dictionary<int, Document>`; `AddDocument`, `SearchSingleWord`, `SearchMultipleWords` (**IntersectWith**, AND), `SearchAnyWord` (**UnionWith**, OR) |
| `PerformanceTest.cs` | порівняння лінійного пошуку та індексу |
| `Program.cs` | тестові дані, вимірювання (10 / 1000 документів), інтерактивний пошук |

Важливі рішення:
- **Однакова нормалізація.** І лінійний пошук, і індекс використовують `TextProcessor.Tokenize` — інакше порівняння некоректне.
- **Шаблон `\w+#?`** (а не просто `\w+`) зберігає токен `c#` окремо від `c`. Слова з дефісом/слешем (`high-level`, `async/await`) розбиваються на частини, тому запит `c# async` знаходить документ №4, як у очікуваному результаті.
- **Копія множини** в `SearchMultipleWords` (`new HashSet<int>(ids)`) — інакше `IntersectWith` зіпсував би сам індекс.
- Стоп-слова в індекс **не прибираються**, щоб можна було шукати й їх; `RemoveStopWords` готовий для використання за потреби (одним рядком у `AddDocument`).
- Тестові дані для вимірювань генеруються з фіксованим `Seed = 42` — результати відтворювані.
- Одиничний пошук виконується за мікросекунди, тому Stopwatch міряє **100 повторів** і виводить середнє.

## Вимірювання продуктивності

| Документів | Слово | Linear, мс/запит | Index, мс/запит | Побудова індексу, мс | Знайдено (linear / index) |
|---|---|---|---|---|---|
| 10 | `w42` | 0.0518 | 0.0004 | 0.275 | 0 / 0 |
| 1000 | `w42` | 2.2757 | 0.0027 | 8.007 | 26 / 26 |


**Що очікувати (якісно, не числа):** на 10 документах різниця мала, і лінійний пошук цілком прийнятний. На 1000 документах лінійний пошук росте приблизно в 100 разів (O(n) × вартість токенізації кожного документа), а пошук за індексом майже не змінюється (O(1) пошук слова + вибірка документів-результатів). Побудова індексу — одноразова ціна, яка окупається при багатьох запитах.

**Фактори, що впливають на вимірювання:** JIT-прогрів (тому повтори), Debug vs Release, збирач сміття (лінійний пошук створює багато тимчасових рядків), кеш процесора.

## Приклад виводу
```
Loaded 5 documents
Index contains 37 unique words

Enter search query (or 'exit'): C#

--- Search Results (AND) ---
[1] Introduction to C#
  C# is a modern object-oriented programming language developed by Microsoft....
[4] C# Advanced Features
  C# supports LINQ, async/await, and many modern programming paradigms....
[5] Comparing Languages
  C#, Java, and Python are among the most popular programming languages today....

Found 3 documents

Enter search query (or 'exit'): Java

--- Search Results (AND) ---
[2] Java Basics
  Java is a popular programming language known for its portability and performance....
[5] Comparing Languages
  C#, Java, and Python are among the most popular programming languages today....

Found 2 documents

Enter search query (or 'exit'): Python

--- Search Results (AND) ---
[3] Python Overview
  Python is a high-level programming language with simple syntax....
[5] Comparing Languages
  C#, Java, and Python are among the most popular programming languages today....

Found 2 documents

Enter search query (or 'exit'): 

```
Чому саме так: документ №4 містить `programming`, але не `language`; документ №5 містить `languages` (множина), а без стемінгу `language` ≠ `languages`. Це наочний приклад обмежень простого індексу (див. питання 4). Число унікальних слів (`N`) залежить від токенізації — візьміть його зі свого запуску, а не з методички.

## Відповіді на контрольні запитання
1. **Чому `Dictionary<string, HashSet<int>>`.** Словник дає пошук слова за O(1); `HashSet<int>` зберігає унікальні ID документів (слово може повторюватися багато разів, а ID — один раз) і швидко виконує операції над множинами.
2. **Переваги `HashSet` для AND/OR.** AND — `IntersectWith`, OR — `UnionWith`: готові, оптимізовані операції без ручних циклів; перевірка належності O(1); автоматична відсутність дублікатів у результаті.
3. **Чому однакова нормалізація.** Якщо один підхід приводить до нижнього регістру й прибирає пунктуацію, а інший — ні, вони знаходитимуть різні документи (`C#,` ≠ `c#`), і порівняння швидкості буде порівнянням різної роботи. Тому перед вимірюванням перевіряємо, що кількість знайдених документів збігається.
4. **Обмеження простої реалізації.** Немає ранжування (усі збіги рівнозначні; потрібен TF-IDF/BM25); немає стемінгу/лематизації (`language` ≠ `languages`); немає пошуку фраз (позиції слів не зберігаються); не обробляються синоніми й помилки в написанні; індекс повністю в пам'яті; не потокобезпечний; оновлення/видалення документа вимагає перебудови записів.
