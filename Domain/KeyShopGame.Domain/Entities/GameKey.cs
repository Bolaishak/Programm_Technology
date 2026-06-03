using KeyShopGame.Domain.Base;
using KeyShopGame.Domain.Exceptions;
using KeyShopGame.Domain.ValueObjects;

namespace KeyShopGame.Domain.Entities;

public class GameKey : Entity<Guid>
{
    public Game Game { get; private set; } = null!;
    public KeyCode Key { get; private set; } = null!;
    public bool IsUsed { get; private set; }
    public bool IsReserved { get; private set; }
    public DateTime? ReservedAt { get; private set; }
    public DateTime? UsedAt { get; private set; }
    public Guid? OrderId { get; private set; }

    private GameKey() { }

    public GameKey(Game game, KeyCode key) : base(Guid.NewGuid())
    {
        Game = game ?? throw new ArgumentNullValueException(nameof(game));
        Key = key ?? throw new ArgumentNullValueException(nameof(key));
        IsUsed = false;
        IsReserved = false;
    }

    public void Reserve()
    {
        if (IsUsed)
            throw new InvalidOperationException("Key is already used");
        
        if (IsReserved)
            throw new InvalidOperationException("Key is already reserved");
        
        IsReserved = true;
        ReservedAt = DateTime.UtcNow;
    }

    public void Use(Guid orderId)
    {
        if (!IsReserved)
            throw new InvalidOperationException("Key must be reserved before use");
        
        if (IsUsed)
            throw new InvalidOperationException("Key is already used");
        
        IsUsed = true;
        UsedAt = DateTime.UtcNow;
        OrderId = orderId;
    }

    public void Release()
    {
        if (!IsReserved)
            throw new InvalidOperationException("Key is not reserved");
        
        if (IsUsed)
            throw new InvalidOperationException("Cannot release used key");
        
        IsReserved = false;
        ReservedAt = null;
    }
}