using System;
using System.Collections.Generic;

namespace Gram.Rpg.Client.Core.Domain
{
    public interface IEntity<TId> : IEquatable<IEntity<TId>>
    {
        TId Id { get; }
    }


    public abstract class Entity<TId> : GObject, IEntity<TId>
    {
        public static bool operator ==(Entity<TId> left, Entity<TId> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<TId> left, Entity<TId> right)
        {
            return !Equals(left, right);
        }

        protected Entity(TId id)
        {
            Id = id;
        }

        public TId Id { get; }

        public bool Equals(IEntity<TId> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<TId>.Default.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Entity<TId>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TId>.Default.GetHashCode(Id);
        }
    }
}
