using DomainLayer.Base;
using DomainLayer.Enums;

namespace DomainLayer.Entities
{
    public class Game : Entity
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public string Publisher { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public string? CoverImageUrl { get; private set; }

        public int SellerId { get; private set; }
        public User Seller { get; private set; }

        private readonly List<GameKey> _keys = new();
        public IReadOnlyCollection<GameKey> Keys => _keys.AsReadOnly();

        private readonly List<Review> _reviews = new();
        public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();

        public double AverageRating => _reviews.Any() ? _reviews.Average(r => r.Rating) : 0;

        public Game(string title, string description, decimal price, string publisher, DateTime releaseDate, User seller)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Price = price > 0 ? price : throw new ArgumentException("Price must be positive");
            Publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            ReleaseDate = releaseDate;
            Seller = seller ?? throw new ArgumentNullException(nameof(seller));
            SellerId = seller.Id;
        }

        public void UpdateInfo(string? description = null, decimal? price = null, string? coverImageUrl = null)
        {
            if (description != null) 
                Description = description;
            if (price.HasValue && price.Value > 0) 
                Price = price.Value;
            if (coverImageUrl != null) 
                CoverImageUrl = coverImageUrl;
            UpdateTimestamp();
        }

        public void AddKey(GameKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (_keys.Any(k => k.KeyCode == key.KeyCode))
                throw new InvalidOperationException($"Key {key.KeyCode} already exists.");
            _keys.Add(key);
            UpdateTimestamp();
        }

        public void AddReview(Review review)
        {
            if (review == null)
                throw new ArgumentNullException(nameof(review));
            _reviews.Add(review);
            UpdateTimestamp();
        }

        public int AvailableKeysCount => _keys.Count(k => k.Status == KeyStatus.Available);
        public bool IsAvailable => AvailableKeysCount > 0;
    }
}