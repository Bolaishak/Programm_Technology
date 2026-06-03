using KeyShopGame.Domain.Exceptions;

namespace KeyShopGame.Domain.ValueObjects;

public class Username : IEquatable<Username>
{
    public string Value { get; }

    public Username(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullValueException(nameof(value));
        
        if (value.Length < 3 || value.Length > 50)
            throw new ArgumentException("Username must be between 3 and 50 characters", nameof(value));
        
        Value = value;
    }

    public override bool Equals(object? obj) => Equals(obj as Username);
    public bool Equals(Username? other) => other != null && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}