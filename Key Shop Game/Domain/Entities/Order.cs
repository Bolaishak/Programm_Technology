using DomainLayer.Base;
using DomainLayer.Enums;

namespace DomainLayer.Entities
{
    public class Order : Entity
    {
        public int BuyerId { get; private set; }
        public User Buyer { get; private set; }
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;
        public DateTime? PaidAt { get; private set; }
        public decimal TotalAmount { get; private set; }

        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        private readonly List<GameKey> _deliveredKeys = new();
        public IReadOnlyCollection<GameKey> DeliveredKeys => _deliveredKeys.AsReadOnly();

        public Order(User buyer)
        {
            Buyer = buyer ?? throw new ArgumentNullException(nameof(buyer));
            BuyerId = buyer.Id;
        }

        public void AddItem(Game game, int quantity)
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException($"Cannot modify order with status {Status}");
            if (game == null)
                throw new ArgumentNullException(nameof(game));
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive");
            if (!game.IsAvailable)
                throw new InvalidOperationException($"Game '{game.Title}' is not available");
            if (game.AvailableKeysCount < quantity)
                throw new InvalidOperationException($"Not enough keys for '{game.Title}'. Available: {game.AvailableKeysCount}");

            var existingItem = _items.FirstOrDefault(i => i.GameId == game.Id);
            if (existingItem != null)
                existingItem.IncreaseQuantity(quantity);
            else
                _items.Add(new OrderItem(this, game, quantity));

            RecalculateTotal();
        }

        public void MarkAsPaid()
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException($"Cannot pay order with status {Status}");
            if (!_items.Any())
                throw new InvalidOperationException("Cannot pay empty order");
                
            Status = OrderStatus.Paid;
            PaidAt = DateTime.UtcNow;
            UpdateTimestamp();
        }

        public void DeliverKeys(IEnumerable<GameKey> keys)
        {
            if (Status != OrderStatus.Paid)
                throw new InvalidOperationException($"Cannot deliver keys for order with status {Status}");
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));
                
            var keysList = keys.ToList();
            int expectedCount = _items.Sum(i => i.Quantity);
            if (keysList.Count != expectedCount)
                throw new InvalidOperationException($"Expected {expectedCount} keys but got {keysList.Count}");

            foreach (var key in keysList)
            {
                key.MarkAsSold(this);
                _deliveredKeys.Add(key);
            }
            
            Status = OrderStatus.KeysDelivered;
            UpdateTimestamp();
        }

        public void Complete()
        {
            if (Status != OrderStatus.KeysDelivered)
                throw new InvalidOperationException($"Cannot complete order with status {Status}");
            Status = OrderStatus.Completed;
            UpdateTimestamp();
        }

        public void Cancel()
        {
            if (Status != OrderStatus.Pending && Status != OrderStatus.Paid)
                throw new InvalidOperationException($"Cannot cancel order with status {Status}");
            Status = OrderStatus.Cancelled;
            UpdateTimestamp();
        }

        private void RecalculateTotal() => TotalAmount = _items.Sum(i => i.TotalPrice);
    }
}