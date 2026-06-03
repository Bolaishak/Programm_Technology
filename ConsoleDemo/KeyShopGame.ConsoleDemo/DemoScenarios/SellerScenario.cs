using KeyShopGame.Domain.Entities;
using KeyShopGame.Domain.ValueObjects;

namespace KeyShopGame.ConsoleDemo.DemoScenarios;

public static class SellerScenario
{
    public static void Run()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("                    СЦЕНАРИЙ ПРОДАВЦА");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        Console.WriteLine();

        try
        {
            // 1. Создаем продавца
            Console.WriteLine("📝 Шаг 1: Регистрация продавца");
            Console.WriteLine("----------------------------------------");
            
            var seller = new User(
                Guid.NewGuid(),
                new Username("KeyShopOfficial"),
                new Email("admin@keyshop.com"),
                UserRole.Seller
            );
            Console.WriteLine($"✅ Создан продавец: {seller.Username}");
            Console.WriteLine();

            // 2. Добавление первой партии игр
            Console.WriteLine("➕ Шаг 2: Добавление игр в каталог");
            Console.WriteLine("----------------------------------------");
            
            var game1 = seller.AddGameForSale(
                new GameTitle("Hollow Knight"),
                new Price(299.99m),
                10
            );
            Console.WriteLine($"✅ Добавлена: {game1.Title} - {game1.Price} (Ключей: {game1.AvailableKeysCount})");

            var game2 = seller.AddGameForSale(
                new GameTitle("Stardew Valley"),
                new Price(199.99m),
                8
            );
            Console.WriteLine($"✅ Добавлена: {game2.Title} - {game2.Price} (Ключей: {game2.AvailableKeysCount})");

            var game3 = seller.AddGameForSale(
                new GameTitle("Celeste"),
                new Price(149.99m),
                5
            );
            Console.WriteLine($"✅ Добавлена: {game3.Title} - {game3.Price} (Ключей: {game3.AvailableKeysCount})");
            Console.WriteLine();

            // 3. Просмотр каталога продавца
            Console.WriteLine("📋 Шаг 3: Просмотр каталога игр продавца");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Игры продавца {seller.Username}:");
            foreach (var game in seller.GamesForSale)
            {
                Console.WriteLine($"   • {game.Title} - {game.Price} (Доступно: {game.AvailableKeysCount} шт.)");
            }
            Console.WriteLine();

            // 4. Редактирование информации об игре
            Console.WriteLine("✏️ Шаг 4: Редактирование информации об игре");
            Console.WriteLine("----------------------------------------");
            
            Console.WriteLine($"До редактирования: {game1.Title} - {game1.Price}");
            seller.EditGame(game1, new GameTitle("Hollow Knight: Voidheart Edition"), new Price(349.99m));
            Console.WriteLine($"После редактирования: {game1.Title} - {game1.Price}");
            Console.WriteLine();

            // 5. Добавление новых ключей для популярной игры
            Console.WriteLine("🔑 Шаг 5: Добавление дополнительных ключей");
            Console.WriteLine("----------------------------------------");
            
            Console.WriteLine($"До добавления: {game2.AvailableKeysCount} ключей");
            game2.AddKeys(15);
            Console.WriteLine($"После добавления: {game2.AvailableKeysCount} ключей");
            Console.WriteLine();

            // 6. Удаление игры с низкими продажами
            Console.WriteLine("🗑️ Шаг 6: Удаление игры из каталога");
            Console.WriteLine("----------------------------------------");
            
            Console.WriteLine($"Удаляем игру: {game3.Title}");
            seller.DeleteGame(game3);
            Console.WriteLine($"✅ Игра удалена. Осталось игр в каталоге: {seller.GamesForSale.Count}");
            Console.WriteLine();

            // Итог
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine("                    ИТОГИ СЦЕНАРИЯ");
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.ResetColor();
            Console.WriteLine($"\n✅ Продавец {seller.Username} управляет {seller.GamesForSale.Count} играми");
            Console.WriteLine($"🎮 Всего ключей в наличии: {seller.GamesForSale.Sum(g => g.AvailableKeysCount)}");
            Console.WriteLine($"💰 Общая стоимость каталога: {seller.GamesForSale.Sum(g => g.Price.Value):C}");
            
            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n❌ ОШИБКА: {ex.Message}");
            Console.ResetColor();
            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }
    }
}