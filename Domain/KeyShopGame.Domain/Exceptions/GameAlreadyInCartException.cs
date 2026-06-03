using KeyShopGame.Domain.Entities;

namespace KeyShopGame.Domain.Exceptions;

public class GameAlreadyInCartException : InvalidOperationException
{
    public Game Game { get; }

    public GameAlreadyInCartException(Game game)
        : base($"Game '{game.Title}' is already in the cart.")
    {
        Game = game;
    }
}