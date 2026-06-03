using KeyShopGame.Domain.Base;
using KeyShopGame.Domain.Exceptions;
using KeyShopGame.Domain.ValueObjects;

namespace KeyShopGame.Domain.Entities;

public class OrderItem
{
    public Guid GameId { get; }
    public GameTitle Title { get; }
    public Price Price { get; }
    public KeyCode? ActivatedKey { get; private set; }

    public OrderItem(Game game)
    {
        GameId = game.Id;
        Title = game.Title;
        Price = game.Price;
    }

    public void SetActivatedKey(KeyCode key)
    {
        ActivatedKey = key ?? throw new ArgumentNullValueException(nameof(key));
    }
}

public enum OrderStatus
{
    Pending,
    Paid,
    Completed,
    Cancelled
}

public class Order : Entity<Guid>
{
    private readonly List<OrderItem> _items = [];

    public User Customer { get; private set; } = null!;
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public Price TotalPrice { get; private set; } = null!;
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; }
    public DateTime? PaidAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public string? ShippingAddress { get; private set; }

    private Order() { }

    public Order(User customer, List<OrderItem> items, Price totalPrice, string? shippingAddress) 
        : base(Guid.NewGuid())
    {
        Customer = customer ?? throw new ArgumentNullValueException(nameof(customer));
        _items = items ?? throw new ArgumentNullValueException(nameof(items));
        TotalPrice = totalPrice ?? throw new ArgumentNullValueException(nameof(totalPrice));
        ShippingAddress = shippingAddress;
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException($"Cannot pay order with status {Status}");
        
        Status = OrderStatus.Paid;
        PaidAt = DateTime.UtcNow;
    }

    public void CompleteOrder()
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOperationException($"Cannot complete order with status {Status}");
        
        Status = OrderStatus.Completed;
        CompletedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Completed)
            throw new InvalidOperationException("Cannot cancel completed order");
        
        Status = OrderStatus.Cancelled;
    }
}