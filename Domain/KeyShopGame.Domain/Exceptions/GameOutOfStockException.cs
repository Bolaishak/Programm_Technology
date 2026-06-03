using KeyShopGame.Domain.Entities;

namespace KeyShopGame.Domain.Exceptions;

public class GameOutOfStockException : InvalidOperationException
{
    public Game Game { get; }

    public GameOutOfStockException(Game game)
        : base($"Game '{game.Title}' is out of stock. Available keys: {game.AvailableKeysCount}")
    {
        Game = game;
    }
}