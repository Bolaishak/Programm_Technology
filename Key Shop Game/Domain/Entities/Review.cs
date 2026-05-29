using DomainLayer.Base;

namespace DomainLayer.Entities
{
    public class Review : Entity
    {
        public int Rating { get; private set; }
        public string Comment { get; private set; }
        public int GameId { get; private set; }
        public Game Game { get; private set; }
        public int BuyerId { get; private set; }
        public User Buyer { get; private set; }

        public Review(int rating, string comment, Game game, User buyer)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5");
            if (string.IsNullOrWhiteSpace(comment))
                throw new ArgumentException("Comment cannot be empty");
                
            Rating = rating;
            Comment = comment;
            Game = game ?? throw new ArgumentNullException(nameof(game));
            Buyer = buyer ?? throw new ArgumentNullException(nameof(buyer));
            GameId = game.Id;
            BuyerId = buyer.Id;
        }
        
        public void UpdateComment(string newComment)
        {
            if (string.IsNullOrWhiteSpace(newComment))
                throw new ArgumentException("Comment cannot be empty");
            Comment = newComment;
            UpdateTimestamp();
        }
    }
}