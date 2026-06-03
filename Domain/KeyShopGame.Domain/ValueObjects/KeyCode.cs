using KeyShopGame.Domain.Exceptions;

namespace KeyShopGame.Domain.ValueObjects;

public class KeyCode : IEquatable<KeyCode>
{
    public string Value { get; }

    public KeyCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullValueException(nameof(value));
        
        var parts = value.Split('-');
        if (parts.Length != 3 || parts.Any(p => p.Length != 4 || !p.All(char.IsLetterOrDigit)))
            throw new ArgumentException("Invalid key format. Expected: XXXX-XXXX-XXXX", nameof(value));
        
        Value = value.ToUpperInvariant();
    }

    public override bool Equals(object? obj) => Equals(obj as KeyCode);
    public bool Equals(KeyCode? other) => other != null && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}