using KeyShopGame.Domain.Exceptions;

namespace KeyShopGame.Domain.ValueObjects;

public class GameTitle : IEquatable<GameTitle>
{
    public string Value { get; }

    public GameTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullValueException(nameof(value));
        
        if (value.Length < 3 || value.Length > 100)
            throw new ArgumentException("Game title must be between 3 and 100 characters", nameof(value));
        
        Value = value;
    }

    public override bool Equals(object? obj) => Equals(obj as GameTitle);
    public bool Equals(GameTitle? other) => other != null && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
    public static bool operator ==(GameTitle? left, GameTitle? right) => Equals(left, right);
    public static bool operator !=(GameTitle? left, GameTitle? right) => !Equals(left, right);
}