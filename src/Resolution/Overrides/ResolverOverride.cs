﻿using System;

namespace Unity.Resolution;



/// <summary>
/// Base class for all override objects passed in the
/// <see cref="IUnityContainer.Resolve"/> method.
/// </summary>
public abstract class ResolverOverride : IEquatable<MatchRank>,
                                         IResolve
{
    #region Fields

    protected Type?              Target;
    protected readonly string?   Name;
    protected readonly object?   Value;
    private   readonly MatchRank _rank;

    #endregion


    #region Constructors

    /// <summary>
    /// This constructor is used when no target is required
    /// </summary>
    /// <param name="name">Name of the dependency</param>
    /// <param name="value">Value to pass to resolver</param>
    /// <param name="rank">Minimal required rank to override</param>
    protected ResolverOverride(string? name, object? value, MatchRank rank)
    {
        Name = name;
        Value = value;
        _rank = rank;
    }

    /// <summary>
    /// This constructor is used with targeted overrides
    /// </summary>
    /// <param name="target"><see cref="Type"/> of the target</param>
    /// <param name="name">Name of the dependency</param>
    /// <param name="value">Value to pass to resolver</param>
    /// <param name="rank">Minimal required rank to override</param>
    protected ResolverOverride(Type? target, string? name, object? value, MatchRank rank)
    {
        Name = name;
        Value = value;
        Target = target;
        _rank = rank;
    }

    #endregion


    #region Type Based Override

    /// <summary>
    /// This method adds target information to the override. Only targeted
    /// <see cref="Type"/> will be overridden even if other dependencies match
    /// the type of the name of the override.
    /// </summary>
    /// <typeparam name="T">Type to constrain the override to.</typeparam>
    /// <returns>The new override.</returns>
    public ResolverOverride OnType<T>()
    {
        Target = typeof(T);
        return this;
    }

    /// <summary>
    /// This method adds target information to the override. Only targeted
    /// <see cref="Type"/> will be overridden even if other dependencies match
    /// the type of the name of the override.
    /// </summary>
    /// <param name="targetType">Type to constrain the override to.</param>
    /// <returns>The new override.</returns>
    public ResolverOverride OnType(Type targetType)
    {
        Target = targetType;
        return this;
    }

    #endregion


    #region IEquatable<MatchRank>

    public bool Equals(MatchRank other) 
        => other >= _rank;

    #endregion


    #region IResolve

    /// <inheritdoc />
    public virtual object? Resolve<TContext>(ref TContext context) 
        where TContext : IResolveContext 
        => Value;

    #endregion
}

