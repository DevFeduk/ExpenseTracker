using System;
using ExpenseList;

ExpenseTrackerService tracker = new ExpenseTrackerService();
bool StartProgramm = true;

while (StartProgramm)
{
    Console.WriteLine("\n---МЕНЮ ПРИЛОЖЕНИЯ---");
    Console.WriteLine("1. Показать все расходы");
    Console.WriteLine("2. Найти расход по ID");
    Console.WriteLine("3. Фильтр по категории");
    Console.WriteLine("4. Общая статистика");
    Console.WriteLine("5. Сохрание JSON-файла");
    Console.WriteLine("6. Дабавить расход");
    Console.WriteLine("7. Удалить расход");
    Console.WriteLine("8. Изменить расход");
    Console.WriteLine("9. Выход");
    Console.Write("Выберите действие: ");


    string choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            tracker.ShowAllExpenses();//Выввод всех расходов
            break;
        case "2":
            tracker.SearchById();//Поиск по Id
            break;
        case "3":
            tracker.SearchByCategory();//Поиск по категории
            break;
        case "4":
            tracker.AllStatistic();
            break;
        case "5":
            tracker.SaveJson();
            break;
        case "6":
            tracker.AddExpense();
            break;
        case "7":
            tracker.DeleteExpense();
            break;
        case "8":
            tracker.UpdateExpense();
            break;
        case "9":
            StartProgramm = false;
            Console.WriteLine("Выбрано: Выход из приложения");
            break;
        default:
            Console.WriteLine("Неверный ввод, попробуйте снова.");
            break;
    }
}
Console.WriteLine("Программа завершена.");
