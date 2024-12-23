﻿using System;

namespace CleanArchitectureTemplate.Domain.Common;

public abstract class Entity
{
    public int Id { get; set; }
}

public abstract class Entity<Tid>(Tid id) : IEquatable<Entity<Tid>> where Tid : notnull
{
    public Tid Id { get; protected set; } = id;

    public override bool Equals(object? obj)
    {
        return obj is Entity<Tid> entity && Id.Equals(entity.Id);
    }

    public bool Equals(Entity<Tid>? other)
    {
        return Equals((object?)other);
    }

    public static bool operator ==(Entity<Tid> left, Entity<Tid> right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity<Tid> left, Entity<Tid> right)
    {
        return !Equals(left, right);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}