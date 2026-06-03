using KeyShopGame.Domain.Entities;
using KeyShopGame.Domain.ValueObjects;

namespace KeyShopGame.ConsoleDemo.DemoScenarios;

public static class FullBusinessScenario
{
    public static void Run()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("              ПОЛНЫЙ БИЗНЕС-СЦЕНАРИЙ (E2E)");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("Полный цикл: Продавец → Каталог → Покупатель → Корзина → Оплата → Ключи → Отзыв");
        Console.WriteLine();

        try
        {
            // ========== ЭТАП 1: РЕГИСТРАЦИЯ ==========
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("🏪 ЭТАП 1: РЕГИСТРАЦИЯ ПОЛЬЗОВАТЕЛЕЙ");
            Console.ResetColor();
            
            var seller = new User(
                Guid.NewGuid(),
                new Username("OfficialGameStore"),
                new Email("store@games.com"),
                UserRole.Seller
            );
            Console.WriteLine($"   Продавец: {seller.Username}");

            var customer = new User(
                Guid.NewGuid(),
                new Username("AvidGamer"),
                new Email("gamer@email.com"),
                UserRole.Customer
            );
            Console.WriteLine($"   Покупатель: {customer.Username}");
            
            customer.DepositFunds(new Price(10000m));
            Console.WriteLine($"   Баланс покупателя: {customer.Balance}");
            Console.WriteLine();

            // ========== ЭТАП 2: ФОРМИРОВАНИЕ КАТАЛОГА ==========
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("🎮 ЭТАП 2: ПРОДАВЕЦ ФОРМИРУЕТ КАТАЛОГ");
            Console.ResetColor();
            
            var games = new List<Game>();
            games.Add(seller.AddGameForSale(new GameTitle("Baldur's Gate 3"), new Price(1799.99m), 50));
            games.Add(seller.AddGameForSale(new GameTitle("Alan Wake 2"), new Price(1499.99m), 30));
            games.Add(seller.AddGameForSale(new GameTitle("Starfield"), new Price(1299.99m), 45));
            games.Add(seller.AddGameForSale(new GameTitle("Spider-Man 2"), new Price(1699.99m), 25));
            games.Add(seller.AddGameForSale(new GameTitle("Super Mario Wonder"), new Price(999.99m), 60));
            
            Console.WriteLine($"   Добавлено игр: {games.Count}");
            foreach (var game in games)
            {
                Console.WriteLine($"   • {game.Title} - {game.Price} (в наличии: {game.AvailableKeysCount} шт.)");
            }
            Console.WriteLine();

            // ========== ЭТАП 3: ПОКУПАТЕЛЬ ВЫБИРАЕТ ИГРЫ ==========
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("🛒 ЭТАП 3: ПОКУПАТЕЛЬ ФОРМИРУЕТ КОРЗИНУ");
            Console.ResetColor();
            
            var cart = customer.CreateCart();
            var selectedGames = games.Take(3).ToList();
            
            foreach (var game in selectedGames)
            {
                cart.AddGame(game);
                Console.WriteLine($"   + {game.Title}");
            }
            
            var total = cart.GetTotalPrice();
            Console.WriteLine($"   Итого: {total}");
            Console.WriteLine();

            // ========== ЭТАП 4: ОФОРМЛЕНИЕ И ОПЛАТА ==========
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("💰 ЭТАП 4: ОФОРМЛЕНИЕ И ОПЛАТА ЗАКАЗА");
            Console.ResetColor();
            
            var order = customer.CreateOrder(cart, "г. Санкт-Петербург, Невский пр., д. 1");
            Console.WriteLine($"   Заказ №{order.Id.ToString().Substring(0, 8)} создан на сумму {order.TotalPrice}");
            
            order.MarkAsPaid();
            Console.WriteLine($"   Заказ оплачен");
            Console.WriteLine($"   Остаток на балансе: {customer.Balance}");
            Console.WriteLine();

            // ========== ЭТАП 5: ВЫДАЧА КЛЮЧЕЙ ==========
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("🔑 ЭТАП 5: ПРОДАВЕЦ ВЫДАЕТ КЛЮЧИ АКТИВАЦИИ");
            Console.ResetColor();
            
            foreach (var item in order.Items)
            {
                var game = seller.GamesForSale.First(g => g.Id == item.GameId);
                var key = game.ReserveKey();
                item.SetActivatedKey(key.Key);
                key.Use(order.Id);
                Console.WriteLine($"   Ключ для {item.Title}: {key.Key}");
            }
            
            order.CompleteOrder();
            Console.WriteLine($"   Заказ выполнен! Статус: {order.Status}");
            Console.WriteLine();

            // ========== ЭТАП 6: ОТЗЫВЫ ==========
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("⭐ ЭТАП 6: ПОКУПАТЕЛЬ ОСТАВЛЯЕТ ОТЗЫВЫ");
            Console.ResetColor();
            
            customer.AddReview(selectedGames[0], "Лучшая RPG года! Невероятный сюжет и свобода действий", 5);
            customer.AddReview(selectedGames[1], "Атмосферный хоррор с отличной графикой", 4);
            customer.AddReview(selectedGames[2], "Хорошая игра, но есть баги", 4);
            
            foreach (var game in selectedGames)
            {
                Console.WriteLine($"   {game.Title}: рейтинг {game.GetAverageRating():F1}/5 (отзывов: {game.Reviews.Count})");
            }
            Console.WriteLine();

            // ========== ФИНАЛЬНЫЙ ОТЧЕТ ==========
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine("                    ФИНАЛЬНЫЙ ОТЧЕТ");
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.ResetColor();
            
            Console.WriteLine($"\n📊 СТАТИСТИКА ПРОДАВЦА:");
            Console.WriteLine($"   • Игр в каталоге: {seller.GamesForSale.Count}");
            Console.WriteLine($"   • Всего продано ключей: {order.Items.Count}");
            Console.WriteLine($"   • Средний рейтинг каталога: {seller.GamesForSale.Average(g => g.GetAverageRating()):F1}/5");
            
            Console.WriteLine($"\n📊 СТАТИСТИКА ПОКУПАТЕЛЯ:");
            Console.WriteLine($"   • Куплено игр: {order.Items.Count}");
            Console.WriteLine($"   • Потрачено: {order.TotalPrice}");
            Console.WriteLine($"   • Остаток на балансе: {customer.Balance}");
            Console.WriteLine($"   • Оставлено отзывов: {selectedGames.Count}");
            
            Console.WriteLine($"\n✅ БИЗНЕС-СЦЕНАРИЙ УСПЕШНО ЗАВЕРШЕН!");
            
            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n❌ КРИТИЧЕСКАЯ ОШИБКА: {ex.Message}");
            Console.ResetColor();
            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }
    }
}