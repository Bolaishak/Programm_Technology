using DomainLayer.Base;

namespace DomainLayer.Entities
{
    public class OrderItem : Entity
    {
        public int OrderId { get; private set; }
        public Order Order { get; private set; }
        public int GameId { get; private set; }
        public Game Game { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal TotalPrice => UnitPrice * Quantity;

        public OrderItem(Order order, Game game, int quantity)
        {
            Order = order ?? throw new ArgumentNullException(nameof(order));
            Game = game ?? throw new ArgumentNullException(nameof(game));
            Quantity = quantity > 0 ? quantity : throw new ArgumentException("Quantity must be positive");
            UnitPrice = game.Price;
            OrderId = order.Id;
            GameId = game.Id;
        }

        public void IncreaseQuantity(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive");
            Quantity += amount;
            UpdateTimestamp();
        }
    }
}