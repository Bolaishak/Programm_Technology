using DomainLayer.Base;

namespace DomainLayer.Entities
{
    public class ShoppingCart : Entity
    {
        public int BuyerId { get; private set; }
        public User Buyer { get; private set; }

        private readonly List<CartItem> _items = new();
        public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

        public ShoppingCart(User buyer)
        {
            Buyer = buyer ?? throw new ArgumentNullException(nameof(buyer));
            BuyerId = buyer.Id;
        }

        public void AddItem(Game game, int quantity = 1)
        {
            if (game == null)
                throw new ArgumentNullException(nameof(game));
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive");
            if (!game.IsAvailable)
                throw new InvalidOperationException($"Game '{game.Title}' is not available");
                
            var existingItem = _items.FirstOrDefault(i => i.GameId == game.Id);
            if (existingItem != null)
                existingItem.IncreaseQuantity(quantity);
            else
                _items.Add(new CartItem(game, quantity));
                
            UpdateTimestamp();
        }

        public void RemoveItem(int gameId)
        {
            var item = _items.FirstOrDefault(i => i.GameId == gameId);
            if (item == null)
                throw new InvalidOperationException($"Item with game ID {gameId} not found in cart");
            _items.Remove(item);
            UpdateTimestamp();
        }

        public void UpdateQuantity(int gameId, int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Quantity cannot be negative");
                
            var item = _items.FirstOrDefault(i => i.GameId == gameId);
            if (item == null)
                throw new InvalidOperationException($"Item with game ID {gameId} not found in cart");
                
            if (quantity == 0)
                RemoveItem(gameId);
            else
                item.SetQuantity(quantity);
                
            UpdateTimestamp();
        }

        public void Clear()
        {
            _items.Clear();
            UpdateTimestamp();
        }

        public decimal TotalPrice => _items.Sum(i => i.TotalPrice);
        public int TotalItems => _items.Sum(i => i.Quantity);

        public Order CreateOrder()
        {
            if (!_items.Any())
                throw new InvalidOperationException("Cannot create order from empty cart");

            var order = new Order(Buyer);
            foreach (var item in _items)
                order.AddItem(item.Game, item.Quantity);

            Clear();
            return order;
        }
    }

    public class CartItem
    {
        public int GameId { get; private set; }
        public Game Game { get; private set; }
        public int Quantity { get; private set; }
        public decimal TotalPrice => Game.Price * Quantity;

        public CartItem(Game game, int quantity)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
            GameId = game.Id;
            Quantity = quantity;
        }

        public void IncreaseQuantity(int amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive");
            Quantity += amount;
        }
        
        public void SetQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive");
            Quantity = quantity;
        }
    }
}