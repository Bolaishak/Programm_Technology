using KeyShopGame.ConsoleDemo.DemoScenarios;

namespace KeyShopGame.ConsoleDemo;

class Program
{
    static void Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║         KEY SHOP GAME - Демонстрация доменной логики         ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        while (true)
        {
            Console.WriteLine("\nВыберите сценарий для демонстрации:");
            Console.WriteLine("1. 👤 Сценарий покупателя (просмотр, корзина, покупка, отзывы)");
            Console.WriteLine("2. 🏪 Сценарий продавца (добавление, редактирование игр)");
            Console.WriteLine("3. ⚠️ Сценарий обработки ошибок (исключения домена)");
            Console.WriteLine("4. 🎯 Полный бизнес-сценарий (покупатель + продавец)");
            Console.WriteLine("0. Выход");
            Console.Write("\nВаш выбор: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CustomerScenario.Run();
                    break;
                case "2":
                    SellerScenario.Run();
                    break;
                case "3":
                    ErrorHandlingScenario.Run();
                    break;
                case "4":
                    FullBusinessScenario.Run();
                    break;
                case "0":
                    Console.WriteLine("\nСпасибо за использование! До свидания!");
                    return;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }
}