namespace KeyShopGame.Domain.Base
{
    /// <summary>
    /// Represents an entity in the system.
    /// </summary>
    /// <typeparam name="TId">The type of the entity's ID.</typeparam>
    public abstract class Entity<TId>(TId id) where TId : struct, IEquatable<TId>
    {
        public TId Id { get; } = id;

        protected Entity() : this(default!) { }

        public override bool Equals(object? obj)
        {
            return obj is Entity<TId> other && Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}