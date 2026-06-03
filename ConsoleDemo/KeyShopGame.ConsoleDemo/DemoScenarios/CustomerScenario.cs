using KeyShopGame.Domain.Entities;
using KeyShopGame.Domain.ValueObjects;

namespace KeyShopGame.ConsoleDemo.DemoScenarios;

public static class CustomerScenario
{
    public static void Run()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("                    СЦЕНАРИЙ ПОКУПАТЕЛЯ");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        Console.WriteLine();

        try
        {
            // 1. Создаем продавца и покупателя
            Console.WriteLine("📝 Шаг 1: Регистрация пользователей");
            Console.WriteLine("----------------------------------------");
            
            var seller = new User(
                Guid.NewGuid(),
                new Username("GameMaster"),
                new Email("seller@keyshop.com"),
                UserRole.Seller
            );
            Console.WriteLine($"✅ Создан продавец: {seller.Username} (Email: {seller.Email})");

            var customer = new User(
                Guid.NewGuid(),
                new Username("Gamer123"),
                new Email("gamer@example.com"),
                UserRole.Customer
            );
            Console.WriteLine($"✅ Создан покупатель: {customer.Username} (Email: {customer.Email})");
            Console.WriteLine();

            // 2. Продавец добавляет игры
            Console.WriteLine("🎮 Шаг 2: Продавец добавляет игры в каталог");
            Console.WriteLine("----------------------------------------");
            
            var cyberpunk = seller.AddGameForSale(
                new GameTitle("Cyberpunk 2077"),
                new Price(199.99m),
                5
            );
            Console.WriteLine($"✅ Добавлена игра: {cyberpunk.Title} - {cyberpunk.Price} (Доступно ключей: {cyberpunk.AvailableKeysCount})");

            var witcher = seller.AddGameForSale(
                new GameTitle("The Witcher 3"),
                new Price(89.99m),
                3
            );
            Console.WriteLine($"✅ Добавлена игра: {witcher.Title} - {witcher.Price} (Доступно ключей: {witcher.AvailableKeysCount})");

            var eldenRing = seller.AddGameForSale(
                new GameTitle("Elden Ring"),
                new Price(149.99m),
                2
            );
            Console.WriteLine($"✅ Добавлена игра: {eldenRing.Title} - {eldenRing.Price} (Доступно ключей: {eldenRing.AvailableKeysCount})");
            Console.WriteLine();

            // 3. Покупатель пополняет баланс
            Console.WriteLine("💰 Шаг 3: Покупатель пополняет баланс");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Текущий баланс: {customer.Balance}");
            
            customer.DepositFunds(new Price(500.00m));
            Console.WriteLine($"✅ Пополнение на 500.00");
            Console.WriteLine($"Новый баланс: {customer.Balance}");
            Console.WriteLine();

            // 4. Покупатель создает корзину и добавляет игры
            Console.WriteLine("🛒 Шаг 4: Формирование корзины");
            Console.WriteLine("----------------------------------------");
            
            var cart = customer.CreateCart();
            cart.AddGame(cyberpunk);
            Console.WriteLine($"✅ Добавлена в корзину: {cyberpunk.Title}");
            
            cart.AddGame(witcher);
            Console.WriteLine($"✅ Добавлена в корзину: {witcher.Title}");
            
            Console.WriteLine($"\n💰 Общая стоимость корзины: {cart.GetTotalPrice()}");
            Console.WriteLine($"📦 Товаров в корзине: {cart.Items.Count}");
            Console.WriteLine();

            // 5. Покупатель оформляет заказ
            Console.WriteLine("📦 Шаг 5: Оформление заказа");
            Console.WriteLine("----------------------------------------");
            
            var order = customer.CreateOrder(cart, "г. Москва, ул. Игровая, д. 1");
            Console.WriteLine($"✅ Заказ #{order.Id.ToString().Substring(0, 8)} создан!");
            Console.WriteLine($"   Статус: {order.Status}");
            Console.WriteLine($"   Сумма: {order.TotalPrice}");
            Console.WriteLine($"   Адрес: {order.ShippingAddress}");
            Console.WriteLine($"\n💰 Новый баланс покупателя: {customer.Balance}");
            Console.WriteLine();

            // 6. Оплата заказа
            Console.WriteLine("💳 Шаг 6: Оплата заказа");
            Console.WriteLine("----------------------------------------");
            
            order.MarkAsPaid();
            Console.WriteLine($"✅ Заказ оплачен! Статус: {order.Status}");
            Console.WriteLine($"   Дата оплаты: {order.PaidAt}");
            Console.WriteLine();

            // 7. Продавец выдает ключи
            Console.WriteLine("🔑 Шаг 7: Выдача ключей активации");
            Console.WriteLine("----------------------------------------");
            
            foreach (var item in order.Items)
            {
                var game = seller.GamesForSale.First(g => g.Id == item.GameId);
                var key = game.ReserveKey();
                item.SetActivatedKey(key.Key);
                key.Use(order.Id);
                Console.WriteLine($"✅ Ключ для {item.Title}: {key.Key}");
            }
            
            order.CompleteOrder();
            Console.WriteLine($"\n✅ Заказ выполнен! Статус: {order.Status}");
            Console.WriteLine();

            // 8. Покупатель оставляет отзыв
            Console.WriteLine("⭐ Шаг 8: Отзыв о покупке");
            Console.WriteLine("----------------------------------------");
            
            customer.AddReview(cyberpunk, "Отличная игра! Графика на высоте, сюжет захватывает!", 5);
            Console.WriteLine($"✅ Отзыв для {cyberpunk.Title}: Рейтинг 5/5");
            
            customer.AddReview(witcher, "Шедевр! Лучшая RPG в моей коллекции", 5);
            Console.WriteLine($"✅ Отзыв для {witcher.Title}: Рейтинг 5/5");
            
            Console.WriteLine($"\n⭐ Средний рейтинг {cyberpunk.Title}: {cyberpunk.GetAverageRating():F1}/5");
            Console.WriteLine($"⭐ Средний рейтинг {witcher.Title}: {witcher.GetAverageRating():F1}/5");
            Console.WriteLine();

            // Итог
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine("                    ИТОГИ СЦЕНАРИЯ");
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.ResetColor();
            Console.WriteLine($"\n✅ Покупатель {customer.Username} приобрел {order.Items.Count} игр");
            Console.WriteLine($"💰 Потрачено: {order.TotalPrice}");
            Console.WriteLine($"💰 Остаток на балансе: {customer.Balance}");
            Console.WriteLine($"⭐ Оставлено отзывов: 2");
            Console.WriteLine($"🎮 Ключи активации получены: {order.Items.Count}");
            
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