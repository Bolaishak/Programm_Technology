using KeyShopGame.Domain.Entities;

namespace KeyShopGame.Domain.Exceptions;

public class GameNotBelongSellerException : InvalidOperationException
{
    public Game Game { get; }
    public User Seller { get; }

    public GameNotBelongSellerException(Game game, User seller)
        : base($"The game '{game.Title}' does not belong to seller {seller.Username} (game id = {game.Id}).")
    {
        Game = game;
        Seller = seller;
    }
}