using KeyShopGame.Domain.ValueObjects;

namespace KeyShopGame.Domain.Exceptions;

public class InsufficientFundsException : InvalidOperationException
{
    public Price RequiredAmount { get; }
    public Price CurrentBalance { get; }

    public InsufficientFundsException(Price required, Price balance)
        : base($"Insufficient funds. Required: {required.Value}, Available: {balance.Value}")
    {
        RequiredAmount = required;
        CurrentBalance = balance;
    }
}