using KeyShopGame.Domain.Base;
using KeyShopGame.Domain.Exceptions;
using KeyShopGame.Domain.ValueObjects;

namespace KeyShopGame.Domain.Entities;

public class Game : Entity<Guid>
{
    private readonly List<GameKey> _keys = [];
    private readonly List<Review> _reviews = [];

    public GameTitle Title { get; private set; } = null!;
    public Price Price { get; private set; } = null!;
    public User Seller { get; private set; } = null!;
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; private set; }
    public int AvailableKeysCount => _keys.Count(k => !k.IsUsed);
    public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();

    private Game() { }

    public Game(User seller, GameTitle title, Price price, int initialKeyCount) : base(Guid.NewGuid())
    {
        Seller = seller ?? throw new ArgumentNullValueException(nameof(seller));
        Title = title ?? throw new ArgumentNullValueException(nameof(title));
        Price = price ?? throw new ArgumentNullValueException(nameof(price));
        CreatedAt = DateTime.UtcNow;
        
        for (int i = 0; i < initialKeyCount; i++)
        {
            _keys.Add(new GameKey(this, GenerateKey()));
        }
    }

    public void UpdateInfo(GameTitle newTitle, Price newPrice)
    {
        if (newTitle == null) throw new ArgumentNullValueException(nameof(newTitle));
        if (newPrice == null) throw new ArgumentNullValueException(nameof(newPrice));
        
        Title = newTitle;
        Price = newPrice;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddKeys(int count)
    {
        if (count <= 0)
            throw new ArgumentException("Key count must be positive", nameof(count));
        
        for (int i = 0; i < count; i++)
        {
            _keys.Add(new GameKey(this, GenerateKey()));
        }
        UpdatedAt = DateTime.UtcNow;
    }

    public GameKey ReserveKey()
    {
        var availableKey = _keys.FirstOrDefault(k => !k.IsUsed);
        
        if (availableKey == null)
            throw new GameOutOfStockException(this);
        
        availableKey.Reserve();
        return availableKey;
    }

    public void AddReview(User customer, string comment, int rating)
    {
        if (customer == null) throw new ArgumentNullValueException(nameof(customer));
        if (rating < 1 || rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5", nameof(rating));
        
        var review = new Review(this, customer, comment, rating);
        _reviews.Add(review);
    }

    public double GetAverageRating()
    {
        if (!_reviews.Any()) return 0;
        return _reviews.Average(r => r.Rating);
    }

    private static KeyCode GenerateKey()
    {
        var random = new Random();
        var part1 = random.Next(1000, 9999).ToString();
        var part2 = random.Next(1000, 9999).ToString();
        var part3 = random.Next(1000, 9999).ToString();
        return new KeyCode($"{part1}-{part2}-{part3}");
    }
}