using KeyShopGame.Domain.Base;
using KeyShopGame.Domain.Exceptions;
using KeyShopGame.Domain.ValueObjects;

namespace KeyShopGame.Domain.Entities;

public enum UserRole
{
    Customer,
    Seller
}

public class User : Entity<Guid>
{
    private readonly List<Order> _orders = [];
    private readonly List<Game> _gamesForSale = [];
    private Price _balance = Price.Zero;

    public Username Username { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public UserRole Role { get; private set; }
    public Price Balance => _balance;
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();
    public IReadOnlyCollection<Game> GamesForSale => _gamesForSale.AsReadOnly();

    private User() { }

    public User(Guid id, Username username, Email email, UserRole role) : base(id)
    {
        Username = username ?? throw new ArgumentNullValueException(nameof(username));
        Email = email ?? throw new ArgumentNullValueException(nameof(email));
        Role = role;
    }

    internal bool ChangeUsername(Username newUsername)
    {
        if (newUsername == null) throw new ArgumentNullValueException(nameof(newUsername));
        if (Username == newUsername) return false;
        Username = newUsername;
        return true;
    }

    public void DepositFunds(Price amount)
    {
        if (amount <= Price.Zero)
            throw new ArgumentException("Deposit amount must be positive", nameof(amount));
        
        _balance += amount;
    }

    public void WithdrawFunds(Price amount)
    {
        if (amount <= Price.Zero)
            throw new ArgumentException("Withdrawal amount must be positive", nameof(amount));
        
        if (_balance < amount)
            throw new InsufficientFundsException(amount, _balance);
        
        _balance -= amount;
    }

    public Game AddGameForSale(GameTitle title, Price price, int quantity)
    {
        if (Role != UserRole.Seller)
            throw new InvalidOperationException("Only sellers can add games for sale");

        var game = new Game(this, title, price, quantity);
        _gamesForSale.Add(game);
        return game;
    }

    public void EditGame(Game game, GameTitle newTitle, Price newPrice)
    {
        if (Role != UserRole.Seller)
            throw new InvalidOperationException("Only sellers can edit games");

        if (game.Seller.Id != Id)
            throw new AnotherUserEditGameException(game, this);

        if (!_gamesForSale.Any(g => g.Id == game.Id))
            throw new GameNotBelongSellerException(game, this);

        game.UpdateInfo(newTitle, newPrice);
    }

    public void DeleteGame(Game game)
    {
        if (Role != UserRole.Seller)
            throw new InvalidOperationException("Only sellers can delete games");

        if (game.Seller.Id != Id)
            throw new AnotherUserDeleteGameException(game, this);

        if (!_gamesForSale.Any(g => g.Id == game.Id))
            throw new GameNotBelongSellerException(game, this);

        _gamesForSale.Remove(game);
    }

    public Cart CreateCart()
    {
        if (Role != UserRole.Customer)
            throw new InvalidOperationException("Only customers can create carts");

        return new Cart(this);
    }

    public Order CreateOrder(Cart cart, string? shippingAddress = null)
    {
        if (Role != UserRole.Customer)
            throw new InvalidOperationException("Only customers can create orders");

        if (cart.User.Id != Id)
            throw new InvalidOperationException("Cart belongs to another user");

        if (!cart.Items.Any())
            throw new InvalidOperationException("Cannot create order from empty cart");

        var totalPrice = cart.GetTotalPrice();
        
        if (_balance < totalPrice)
            throw new InsufficientFundsException(totalPrice, _balance);

        var orderItems = cart.Items.Select(item => new OrderItem(
            _gamesForSale.First(g => g.Id == item.GameId)
        )).ToList();

        var order = new Order(this, orderItems, totalPrice, shippingAddress);
        
        _balance -= totalPrice;
        _orders.Add(order);
        cart.Clear();
        
        return order;
    }

    public void AddReview(Game game, string comment, int rating)
    {
        if (Role != UserRole.Customer)
            throw new InvalidOperationException("Only customers can leave reviews");

        var hasPurchased = _orders.Any(o => o.Items.Any(i => i.GameId == game.Id));
        
        if (!hasPurchased)
            throw new InvalidOperationException($"Cannot review game '{game.Title}' because you haven't purchased it");

        game.AddReview(this, comment, rating);
    }
}