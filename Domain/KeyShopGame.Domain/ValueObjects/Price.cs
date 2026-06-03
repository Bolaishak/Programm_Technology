namespace KeyShopGame.Domain.ValueObjects;

public class Price : IEquatable<Price>
{
    public decimal Value { get; }

    public Price(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Price cannot be negative", nameof(value));
        
        if (value > 10000)
            throw new ArgumentException("Price cannot exceed 10000", nameof(value));
        
        Value = Math.Round(value, 2);
    }

    public static Price Zero => new(0);
    
    public Price Add(Price other) => new(Value + other.Value);
    public Price Subtract(Price other) => new(Value - other.Value);
    
    public override bool Equals(object? obj) => Equals(obj as Price);
    public bool Equals(Price? other) => other != null && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => $"{Value:C}";
    
    public static bool operator >(Price left, Price right) => left.Value > right.Value;
    public static bool operator <(Price left, Price right) => left.Value < right.Value;
    public static bool operator <=(Price left, Price right) => left.Value <= right.Value;
    public static bool operator >=(Price left, Price right) => left.Value >= right.Value;
    public static Price operator +(Price a, Price b) => a.Add(b);
    public static Price operator -(Price a, Price b) => a.Subtract(b);
}