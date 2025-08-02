using System;

namespace CleanArchitectureTemplate.Domain.Common;

public abstract class Entity
{
    public int Id { get; set; }
}

public abstract class Entity<TId>(TId id) : IEquatable<Entity<TId>> where TId : notnull
{
    public TId Id { get; protected set; } = id;

    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && Id.Equals(entity.Id);
    }

    public bool Equals(Entity<TId>? other)
    {
        return Equals((object?)other);
    }

    public static bool operator ==(Entity<TId> left, Entity<TId> right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity<TId> left, Entity<TId> right)
    {
        return !Equals(left, right);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}