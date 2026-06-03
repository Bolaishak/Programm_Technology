using KeyShopGame.Domain.Base;
using KeyShopGame.Domain.Exceptions;

namespace KeyShopGame.Domain.Entities;

public class Review : Entity<Guid>
{
    public Game Game { get; private set; } = null!;
    public User Customer { get; private set; } = null!;
    public string Comment { get; private set; } = null!;
    public int Rating { get; private set; }
    public DateTime CreatedAt { get; }

    private Review() { }

    public Review(Game game, User customer, string comment, int rating) : base(Guid.NewGuid())
    {
        Game = game ?? throw new ArgumentNullValueException(nameof(game));
        Customer = customer ?? throw new ArgumentNullValueException(nameof(customer));
        Comment = comment ?? throw new ArgumentNullValueException(nameof(comment));
        Rating = rating;
        CreatedAt = DateTime.UtcNow;
    }
}