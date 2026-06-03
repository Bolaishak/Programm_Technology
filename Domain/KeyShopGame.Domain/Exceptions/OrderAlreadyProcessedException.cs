using KeyShopGame.Domain.Entities;

namespace KeyShopGame.Domain.Exceptions;

public class OrderAlreadyProcessedException : InvalidOperationException
{
    public Order Order { get; }

    public OrderAlreadyProcessedException(Order order)
        : base($"Order {order.Id} has already been processed and cannot be modified.")
    {
        Order = order;
    }
}