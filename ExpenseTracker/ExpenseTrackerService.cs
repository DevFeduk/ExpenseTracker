using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ExpenseList
{
    public class ExpenseTrackerService
    {
        private readonly string _filePath = "expenses.json";
        private readonly List<Expense> _expenses;
        public ExpenseTrackerService()
        {
            //Проверяем, есть ли файл.
            if (File.Exists(_filePath))
            {
                //Читаем весь текст из файла
                string jsonText = File.ReadAllText(_filePath);
                _expenses = JsonSerializer.Deserialize<List<Expense>>(jsonText) ?? new List<Expense>();
                Console.WriteLine("Данные успешно загружены из файла.");
            }
            else
            {
                Console.WriteLine("Добро пожаловать! Список расходов пуст.");
                Console.WriteLine("Используйте пункт меню 'Добавить расход', чтобы начать учет.");
                _expenses = new List<Expense>();
            }
        }

        //Методы для поиска расходов: 
        public void ShowAllExpenses()
        {
            if (_expenses.Count == 0)
            {
                Console.WriteLine("\n [пусто] У вас пока нет расходов. Добавьте первый через меню!");
                return;
            }
            Console.WriteLine("\n--- Все расходы ---");
            Console.WriteLine($"{"Id",4} | {"Сумма",8} | {"Комментарий",-15} | {"Дата",-20}");
            Console.WriteLine(new string('-', 55));

            foreach (var e in _expenses)
            {
                Console.WriteLine($"{e.Id,4} | {e.Amount,8} | {e.Comment,-15} | {e.CreatedAt,-20}");
            }
        }

        // --- ФИЛЬТР ПО Id ---
        public void SearchById()
        {
            Console.Write("\nВведите Id расхода для поиска: ");
            string inputId = Console.ReadLine();

            if (int.TryParse(inputId, out int searchId))
            {
                Expense? foundId = null;
                foreach (var e in _expenses)
                {
                    if (e.Id == searchId)
                    {
                        foundId = e;
                        break;
                    }
                }
                if (foundId != null)
                {
                    Console.WriteLine($"\nНайденный расход: {foundId.Id} | {foundId.Amount} | {foundId.Category}");
                }
                else
                {
                    Console.WriteLine($"Расход с Id = {searchId} не найден");
                }
            }
            else
            {
                Console.WriteLine("Не похоже на число. Повторите попытку.");
            }
        }

        // --- ФИЛЬТР ПО КАТЕГОРИИ ---
        public void SearchByCategory()
        {
            Console.Write("\nВведите категорию расхода для поиска: ");
            string inputCategory = Console.ReadLine();

            List<Expense> filteredEcategory = new();// Список для фильтра по категориям
            decimal categoryTotal = 0m; //Сюда записываем общую суммe расходов по категории

            foreach (var c in _expenses)
            {
                if (c.Category.ToLower() == inputCategory.ToLower())  // Приводим введёную категорию к нижнему регистру. Сравниваем введёную категорию с категориями из списка
                {
                    filteredEcategory.Add(c);         // добавляем в список category этот расход
                    categoryTotal += c.Amount; //Записываем сумму расхода и/или прибавляем новый расход по этой категории
                }
            }

            if (filteredEcategory.Count > 0)
            {
                Console.WriteLine($"\nКоличество расходов по категории '{inputCategory}' = {filteredEcategory.Count} ");
                Console.WriteLine($"{"Id",4} | {"Сумма",8} | {"Комментарий",-15} | {"Дата",-20}");
                Console.WriteLine(new string('-', 55)); // Разделительная линия из 55 тире

                //Только по найденным расходам
                foreach (var s in filteredEcategory)
                {
                    Console.WriteLine($"{s.Id,4} | {s.Amount,8} | {s.Comment,-15} | {s.CreatedAt,-20}");
                }
                Console.WriteLine($"\nИтого сумма: {categoryTotal}");
            }
            else
            {
                Console.WriteLine("Расходы этой категории не найдены.");
            }
        }

        // --- Общая статистика ---
        public void AllStatistic()
        {
            decimal total = _expenses.Sum(e => e.Amount);
            Console.WriteLine($"\nВсего трат: {total}, записей: {_expenses.Count}");
        }

        //--- Просмотр JSON файла --- 
        public void SaveJson()
        {
            //Раскрываем JSON файл в красивый вид (Encoder заменяет системные символы на кирилицу или латиницу)
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping

            };
            string jsonString = JsonSerializer.Serialize(_expenses, options);
            File.WriteAllText(_filePath, jsonString);//Команда на сохранение JSON файла
            Console.WriteLine("Расходы сохранены в expenses.json");
        }

        //--- Добавление нового расхода ---
        public void AddExpense()
        {
            Console.WriteLine("\n--- Добавление нового расхода ---");
            Console.Write("Введите сумму расхода: ");
            string amountInput = Console.ReadLine();

            //превращаем строку в число. 
            if (!decimal.TryParse(amountInput, out decimal amount) || amount <= 0) //Если строчку не удалось перевести в число ИЛИ введёное число меньше или равно 0 значит =>
            {
                Console.WriteLine("Ошибка: введите корректную положительную сумму.");
                return;
            }
            Console.Write("Введите категорию: ");
            string category = Console.ReadLine();

            Console.Write("Введите комментарий: ");
            string comment = Console.ReadLine();

            //Вычисляем новый ID. 
            int newId = _expenses.Count == 0 ? 1 : _expenses.Max(e => e.Id) + 1;

            //Создаем новый объект расхода
            Expense newExpense = new Expense
            {
                Id = newId,
                Amount = amount,
                Category = category,
                Comment = comment,
                CreatedAt = DateTime.Now
            };

            //Добавляем расход в список
            _expenses.Add(newExpense);

            Console.WriteLine($"Расход '{comment}' на сумму {amount} успешно добавлен!");

            //Сразу сохраняем в файл, чтобы пользователь не забыл это сделать
            SaveJson();
        }
        //--- Удаление расхода из списка ---
        public void DeleteExpense()
        {
            Console.WriteLine("\n--- Удаление расхода ---");
            Console.Write("Введите Id расхода: ");
            string idInput = Console.ReadLine();

            if (int.TryParse(idInput, out int searchId))
            {
                Expense? idfound = null;
                foreach (var s in _expenses)
                {
                    if (s.Id == searchId)
                    {
                        idfound = s;
                        break;
                    }
                }
                if (idfound != null)
                {
                    Console.WriteLine($"\nНайденный расход: {idfound.Id} | {idfound.Amount} | {idfound.Category}");
                    Console.WriteLine("Удаление...");
                    Console.WriteLine("Успешно");
                    _expenses.Remove(idfound);
                    SaveJson();
                }
                else
                {
                    Console.WriteLine($"Расход с Id {idInput} - не найден");
                }
            }
            else
            {
                Console.WriteLine("Не похоже на число");
            }
        }
        //--- Обновление расхода ---
        public void UpdateExpense()
        {
            Console.WriteLine("\n--- Изменение расхода ---");
            Console.Write("Введите Id который хотите изменить:");
            string inputUpdateId = Console.ReadLine();

            if (int.TryParse(inputUpdateId, out int updateId))
            {
                Expense? idfound = null;
                foreach (var i in _expenses)
                {
                    if (i.Id == updateId)
                    {
                        idfound = i;
                        break;
                    }
                }
                if (idfound != null)
                {
                    Console.WriteLine("\nНайденный расход");
                    Console.WriteLine($"{"Id",4} | {"Сумма",8} | {"Комментарий",-15}");
                    Console.WriteLine(new string('-', 50));
                    Console.WriteLine($"{idfound.Id,4} | {idfound.Amount,8} | {idfound.Comment,-15}");

                    Console.WriteLine("\nКакую новую сумму ввести?");
                    string newAmountInput = Console.ReadLine();
                    if (!decimal.TryParse(newAmountInput, out decimal newAmount) || newAmount <= 0)
                    {
                        Console.WriteLine("Ошибка: введена некорректная сумма. Изменение отменено.");
                        return;
                    }

                    Console.WriteLine("Какую новую категорию ввести?");
                    string newCategory = Console.ReadLine();

                    Console.WriteLine("Какой новый комментарий ввести?");
                    string newComment = Console.ReadLine();

                    idfound.Amount = newAmount;
                    idfound.Category = newCategory;
                    idfound.Comment = newComment;

                    Console.WriteLine($"Расход успешно обновлён!");
                    SaveJson();
                }
                else
                {
                    Console.WriteLine($"Расход с Id {inputUpdateId} - не найден");
                }
            }
            else
            {
                Console.WriteLine("Не похоже на Id. Изменение отменено.");
            }
        }
        public Expense CreateExpense(decimal amount, string category, string comment)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(amount),
                    "Сумма расхода должна быть больше нуля.");
            }

            int newId = _expenses.Count == 0 ? 1 : _expenses.Max(e => e.Id) + 1;

            var expense = new Expense
            {
                Id = newId,
                Amount = amount,
                Category = category,
                Comment = comment,
                CreatedAt = DateTime.Now
            };

            _expenses.Add(expense);
            return expense;
        }
    }
}
