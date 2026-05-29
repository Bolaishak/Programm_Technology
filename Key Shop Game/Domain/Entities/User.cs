using DomainLayer.Base;
using DomainLayer.Enums;

namespace DomainLayer.Entities
{
    public class User : Entity
    {
        public string Email { get; private set; }
        public string Username { get; private set; }
        public UserRole Role { get; private set; }

        private readonly List<Order> _orders = new();
        public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

        private readonly List<Review> _reviews = new();
        public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();

        private ShoppingCart? _shoppingCart;
        public ShoppingCart? ShoppingCart => _shoppingCart;

        private readonly List<Game> _gamesForSale = new();
        public IReadOnlyCollection<Game> GamesForSale => _gamesForSale.AsReadOnly();

        public User(string email, string username, UserRole role)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Role = role;
        }

        public void AddOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            _orders.Add(order);
            UpdateTimestamp();
        }

        public void AddReview(Review review)
        {
            if (Role != UserRole.Buyer)
                throw new InvalidOperationException("Only buyers can leave reviews.");
            if (review == null)
                throw new ArgumentNullException(nameof(review));
            _reviews.Add(review);
            UpdateTimestamp();
        }

        public void AddGameForSale(Game game)
        {
            if (Role != UserRole.Seller)
                throw new InvalidOperationException("Only sellers can add games.");
            if (game == null)
                throw new ArgumentNullException(nameof(game));
            if (_gamesForSale.Any(g => g.Id == game.Id))
                throw new InvalidOperationException("Game already added for sale.");
            _gamesForSale.Add(game);
            UpdateTimestamp();
        }

        public void InitializeCart()
        {
            if (Role != UserRole.Buyer)
                throw new InvalidOperationException("Only buyers can have a shopping cart.");
            if (_shoppingCart != null)
                throw new InvalidOperationException("Cart already initialized.");
            _shoppingCart = new ShoppingCart(this);
            UpdateTimestamp();
        }
    }
}