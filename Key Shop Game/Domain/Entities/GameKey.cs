using DomainLayer.Base;
using DomainLayer.Enums;

namespace DomainLayer.Entities
{
    public class GameKey : Entity
    {
        public string KeyCode { get; private set; }
        public KeyStatus Status { get; private set; }
        public int GameId { get; private set; }
        public Game Game { get; private set; }
        public int? OrderId { get; private set; }
        public Order? Order { get; private set; }
        public DateTime? DeliveredAt { get; private set; }

        public GameKey(string keyCode, Game game)
        {
            KeyCode = keyCode ?? throw new ArgumentNullException(nameof(keyCode));
            Game = game ?? throw new ArgumentNullException(nameof(game));
            GameId = game.Id;
            Status = KeyStatus.Available;
        }

        public void MarkAsSold(Order order)
        {
            if (Status != KeyStatus.Available)
                throw new InvalidOperationException($"Cannot sell key with status {Status}");
            if (order == null)
                throw new ArgumentNullException(nameof(order));
                
            Status = KeyStatus.Sold;
            Order = order;
            OrderId = order.Id;
            DeliveredAt = DateTime.UtcNow;
            UpdateTimestamp();
        }

        public void Reserve()
        {
            if (Status != KeyStatus.Available)
                throw new InvalidOperationException($"Cannot reserve key with status {Status}");
            Status = KeyStatus.Reserved;
            UpdateTimestamp();
        }

        public void Release()
        {
            if (Status != KeyStatus.Reserved)
                throw new InvalidOperationException($"Cannot release key with status {Status}");
            Status = KeyStatus.Available;
            UpdateTimestamp();
        }
        
        public void Expire()
        {
            if (Status != KeyStatus.Available && Status != KeyStatus.Reserved)
                throw new InvalidOperationException($"Cannot expire key with status {Status}");
            Status = KeyStatus.Expired;
            UpdateTimestamp();
        }
    }
}