using KeyShopGame.Domain.Base;
using KeyShopGame.Domain.Exceptions;
using KeyShopGame.Domain.ValueObjects;

namespace KeyShopGame.Domain.Entities;

public class CartItem
{
    public Guid GameId { get; }
    public GameTitle Title { get; }
    public Price Price { get; }
    
    public CartItem(Game game)
    {
        GameId = game.Id;
        Title = game.Title;
        Price = game.Price;
    }
}

public class Cart : Entity<Guid>
{
    private readonly List<CartItem> _items = [];

    public User User { get; private set; } = null!;
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();
    public DateTime CreatedAt { get; }
    public DateTime? LastUpdatedAt { get; private set; }

    private Cart() { }

    public Cart(User user) : base(Guid.NewGuid())
    {
        User = user ?? throw new ArgumentNullValueException(nameof(user));
        CreatedAt = DateTime.UtcNow;
    }

    public void AddGame(Game game)
    {
        if (game == null) throw new ArgumentNullValueException(nameof(game));
        
        if (_items.Any(i => i.GameId == game.Id))
            throw new GameAlreadyInCartException(game);
        
        _items.Add(new CartItem(game));
        LastUpdatedAt = DateTime.UtcNow;
    }

    public void RemoveGame(Guid gameId)
    {
        var item = _items.FirstOrDefault(i => i.GameId == gameId);
        if (item != null)
        {
            _items.Remove(item);
            LastUpdatedAt = DateTime.UtcNow;
        }
    }

    public Price GetTotalPrice()
    {
        return _items.Aggregate(Price.Zero, (total, item) => total + item.Price);
    }

    public void Clear()
    {
        _items.Clear();
        LastUpdatedAt = DateTime.UtcNow;
    }

    public bool IsEmpty() => !_items.Any();
}