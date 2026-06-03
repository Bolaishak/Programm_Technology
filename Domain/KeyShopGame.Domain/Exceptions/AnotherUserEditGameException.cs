using KeyShopGame.Domain.Entities;

namespace KeyShopGame.Domain.Exceptions;

public class AnotherUserEditGameException : InvalidOperationException
{
    public Game Game { get; }
    public User User { get; }

    public AnotherUserEditGameException(Game game, User user)
        : base($"The user {user.Username} can't edit the game '{game.Title}' owned by the seller (game id = {game.Id}).")
    {
        Game = game;
        User = user;
    }
}