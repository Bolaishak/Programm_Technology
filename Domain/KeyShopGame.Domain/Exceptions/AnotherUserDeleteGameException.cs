using KeyShopGame.Domain.Entities;

namespace KeyShopGame.Domain.Exceptions;

public class AnotherUserDeleteGameException : InvalidOperationException
{
    public Game Game { get; }
    public User User { get; }

    public AnotherUserDeleteGameException(Game game, User user)
        : base($"The user {user.Username} can't delete the game '{game.Title}' owned by the seller (game id = {game.Id}).")
    {
        Game = game;
        User = user;
    }
}