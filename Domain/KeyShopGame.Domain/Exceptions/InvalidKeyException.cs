using KeyShopGame.Domain.ValueObjects;

namespace KeyShopGame.Domain.Exceptions;

public class InvalidKeyException : ArgumentException
{
    public KeyCode InvalidKey { get; }

    public InvalidKeyException(KeyCode key, string message)
        : base($"Invalid game key: {message}")
    {
        InvalidKey = key;
    }
}