namespace LINQtask;
class Program
{
    static string dbPath = "museum.dat";
    static List<Exhibit> database = new List<Exhibit>();

    static void Main(string[] args)
    {
        database = MuseumService.LoadDatabase(dbPath);
        
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Управление Музеем ===");
            Console.WriteLine("1. Просмотр БД");
            Console.WriteLine("2. Добавить экспонат");
            Console.WriteLine("3. Удалить по инвентарному номеру");
            Console.WriteLine("4. Запросы (LINQ)");
            Console.WriteLine("0. Выход");
            Console.Write("\nВыберите действие: ");

            string choice = Console.ReadLine();
            try
            {
                switch (choice)
                {
                    case "1": PrintDb(); break;
                    case "2": AddExhibit(); break;
                    case "3": RemoveExhibit(); break;
                    case "4": RunQueries(); break;
                    case "0": return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Детали: " + ex.InnerException.Message);
                }
                Console.ReadKey();
            }
        }
    }

    static void PrintDb()
    {
        Console.WriteLine("\n--- Список экспонатов ---");
        database.ForEach(e => Console.WriteLine(e));
        Console.WriteLine("\nНажмите любую клавишу...");
        Console.ReadKey();
    }

    static void AddExhibit()
    {
        Console.Write("Инв. номер: "); 
        string id = Console.ReadLine();
        if (!(database.Any(e => e.InventoryN == id)))
        {
            Console.Write("Название: "); 
            string title = Console.ReadLine();
            Console.Write("Год: "); 
            int year = int.Parse(Console.ReadLine());
            if ((year > 0) && (year < 2027))
            {
                Console.Write("Цена: "); 
                double price = double.Parse(Console.ReadLine());
                if (price > 0)
                {
                    Console.Write("Отреставрирован: ");
                    bool rest = bool.Parse(Console.ReadLine());
                    if (rest == true || rest == false)
                    {
                        database.Add(new Exhibit
                            (id, title, year, price, rest));
                        MuseumService.SaveDatabase(dbPath, database);
                        Console.WriteLine("Добавлено!");
                        Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Экспонат с таким статутом реставрации не может существовать");
                        Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Экспонат с такой ценой не может существовать");
                    Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Экспонат с таким годом не может существовать");
                Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
                Console.ReadKey();
            }
        }
        else
        {
            Console.WriteLine("Экспонат с таким id уже есть и не будет добавлен");
            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }
    }

    static void RemoveExhibit()
    {
        Console.Write("Введите инв. номер для удаления: ");
        string id = Console.ReadLine();
        database.RemoveAll(e => e.InventoryN == id);
        MuseumService.SaveDatabase(dbPath, database);
        Console.WriteLine("Удалено (если существовал).");
        Console.ReadKey();
    }

    static void RunQueries()
    {
        Console.WriteLine("\n1. Антикварные экспонаты (до 1800 г.):");
        var old = MuseumService.GetAntiqueExhibits
            (database, 1800);
        if (old.Any())
        {
            old.ForEach(e => Console.WriteLine(e));
        }
        else
        {
            Console.WriteLine("Таких экспонатов не найдено.");
        }
        
        Console.WriteLine("\n2. Самые ценне отреставрированные объекты:");
        var topRestored = MuseumService.GetTopExpensiveRestored
            (database);
        if (topRestored.Any())
        {
            foreach (var item in topRestored)
            {
                Console.WriteLine(item);
            }
        }
        else
        {
            Console.WriteLine("Отреставрированные экспонаты отсутствуют.");
        }
        
        double avg = MuseumService.GetAverageValue(database);
        Console.WriteLine("\n3. Средняя оценочная стоимость фонда: {0:C}", avg);
        
        int ancientCount = MuseumService.CountAncientExhibits(database);
        Console.WriteLine("\n4. Количество экспонатов старше 1500 года: {0} шт.",
            ancientCount);

        Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
        Console.ReadKey();
    }
}