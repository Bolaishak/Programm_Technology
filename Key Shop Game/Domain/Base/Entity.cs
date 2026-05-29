namespace DomainLayer.Base
{
    public abstract class Entity
    {
        public int Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; protected set; }

        protected void UpdateTimestamp() => UpdatedAt = DateTime.UtcNow;

        public override bool Equals(object? obj)
        {
            if (obj is not Entity other)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (GetType() != other.GetType())
                return false;
            return Id != 0 && Id == other.Id;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}