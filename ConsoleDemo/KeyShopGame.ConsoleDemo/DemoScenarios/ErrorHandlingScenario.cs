using KeyShopGame.Domain.Entities;
using KeyShopGame.Domain.Exceptions;
using KeyShopGame.Domain.ValueObjects;

namespace KeyShopGame.ConsoleDemo.DemoScenarios;

public static class ErrorHandlingScenario
{
    public static void Run()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("                СЦЕНАРИЙ ОБРАБОТКИ ОШИБОК");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("Демонстрация обработки доменных исключений:");
        Console.WriteLine();

        // 1. Попытка создать пользователя с некорректным email
        Console.WriteLine("❌ Тест 1: Некорректный email");
        Console.WriteLine("----------------------------------------");
        try
        {
            var invalidUser = new User(
                Guid.NewGuid(),
                new Username("TestUser"),
                new Email("invalid-email"),
                UserRole.Customer
            );
        }
        catch (ArgumentException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"   Исключение: {ex.Message}");
            Console.ResetColor();
        }
        Console.WriteLine();

        // 2. Попытка создать игру с отрицательной ценой
        Console.WriteLine("❌ Тест 2: Отрицательная цена игры");
        Console.WriteLine("----------------------------------------");
        try
        {
            var seller = new User(
                Guid.NewGuid(),
                new Username("Seller"),
                new Email("seller@test.com"),
                UserRole.Seller
            );
            var invalidGame = seller.AddGameForSale(
                new GameTitle("Test Game"),
                new Price(-100m),
                5
            );
        }
        catch (ArgumentException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"   Исключение: {ex.Message}");
            Console.ResetColor();
        }
        Console.WriteLine();

        // 3. Попытка купить игру при недостатке средств
        Console.WriteLine("❌ Тест 3: Недостаточно средств");
        Console.WriteLine("----------------------------------------");
        try
        {
            var seller = new User(
                Guid.NewGuid(),
                new Username("Seller2"),
                new Email("seller2@test.com"),
                UserRole.Seller
            );
            var game = seller.AddGameForSale(
                new GameTitle("Expensive Game"),
                new Price(1000m),
                1
            );
            
            var customer = new User(
                Guid.NewGuid(),
                new Username("PoorCustomer"),
                new Email("poor@test.com"),
                UserRole.Customer
            );
            customer.DepositFunds(new Price(100m));
            
            var cart = customer.CreateCart();
            cart.AddGame(game);
            var order = customer.CreateOrder(cart);
        }
        catch (InsufficientFundsException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"   Исключение: {ex.Message}");
            Console.ResetColor();
        }
        Console.WriteLine();

        // 4. Попытка редактировать чужую игру
        Console.WriteLine("❌ Тест 4: Редактирование чужой игры");
        Console.WriteLine("----------------------------------------");
        try
        {
            var seller1 = new User(
                Guid.NewGuid(),
                new Username("Seller1"),
                new Email("seller1@test.com"),
                UserRole.Seller
            );
            var game = seller1.AddGameForSale(
                new GameTitle("Original Game"),
                new Price(500m),
                3
            );
            
            var seller2 = new User(
                Guid.NewGuid(),
                new Username("Seller2"),
                new Email("seller2@test.com"),
                UserRole.Seller
            );
            seller2.EditGame(game, new GameTitle("Hacked Game"), new Price(100m));
        }
        catch (AnotherUserEditGameException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"   Исключение: {ex.Message}");
            Console.ResetColor();
        }
        Console.WriteLine();

        // 5. Попытка добавить в корзину игру, которая уже там есть
        Console.WriteLine("❌ Тест 5: Дублирование игры в корзине");
        Console.WriteLine("----------------------------------------");
        try
        {
            var seller = new User(
                Guid.NewGuid(),
                new Username("Seller3"),
                new Email("seller3@test.com"),
                UserRole.Seller
            );
            var game = seller.AddGameForSale(
                new GameTitle("Unique Game"),
                new Price(300m),
                2
            );
            
            var customer = new User(
                Guid.NewGuid(),
                new Username("Customer"),
                new Email("customer@test.com"),
                UserRole.Customer
            );
            customer.DepositFunds(new Price(1000m));
            
            var cart = customer.CreateCart();
            cart.AddGame(game);
            cart.AddGame(game);
        }
        catch (GameAlreadyInCartException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"   Исключение: {ex.Message}");
            Console.ResetColor();
        }
        Console.WriteLine();

        // 6. Попытка оставить отзыв без покупки
        Console.WriteLine("❌ Тест 6: Отзыв без покупки");
        Console.WriteLine("----------------------------------------");
        try
        {
            var seller = new User(
                Guid.NewGuid(),
                new Username("Seller4"),
                new Email("seller4@test.com"),
                UserRole.Seller
            );
            var game = seller.AddGameForSale(
                new GameTitle("New Game"),
                new Price(100m),
                1
            );
            
            var customer = new User(
                Guid.NewGuid(),
                new Username("Reviewer"),
                new Email("reviewer@test.com"),
                UserRole.Customer
            );
            customer.AddReview(game, "Great game!", 5);
        }
        catch (InvalidOperationException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"   Исключение: {ex.Message}");
            Console.ResetColor();
        }
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
        Console.WriteLine("  Все доменные исключения успешно перехвачены и обработаны!");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();
        
        Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
        Console.ReadKey();
    }
}