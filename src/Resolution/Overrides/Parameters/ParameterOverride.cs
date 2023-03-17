using System;
using System.Reflection;
using Unity.Import;

namespace Unity.Resolution;



/// <summary>
/// A <see cref="ResolverOverride"/> class that lets you
/// override a named parameter passed to a constructor.
/// </summary>
public class ParameterOverride : ResolverOverride, 
                                 IMatchInfo<ParameterInfo>
{
    #region Fields

    protected readonly Type? Type;

    #endregion


    #region Constructors

    /// <summary>
    /// Construct a new <see cref="ParameterOverride"/> object that will
    /// override the given named constructor parameter, and pass the given
    /// value.
    /// </summary>
    /// <param name="name">Name of the constructor parameter.</param>
    /// <param name="value">InjectionParameterValue to pass for the constructor.</param>
    public ParameterOverride(string name, object? value)
        : base(name, value)
    {
    }

    /// <summary>
    /// Construct a new <see cref="ParameterOverride"/> object that will
    /// override the given named constructor parameter, and pass the given
    /// value.
    /// </summary>
    /// <param name="type">Type of the parameter.</param>
    /// <param name="value">Value to pass for the MethodBase.</param>
    public ParameterOverride(Type type, object? value)
        : base(null, value) 
        => Type = type;

    /// <summary>
    /// Construct a new <see cref="ParameterOverride"/> object that will
    /// override the given named constructor parameter, and pass the given
    /// value.
    /// </summary>
    /// <param name="type">Type of the parameter.</param>
    /// <param name="name">Name of the parameter.</param>
    /// <param name="value">Value to pass for the MethodBase.</param>
    public ParameterOverride(string? name, Type type, object? value)
        : base(name, value) 
        => Type = type;

    /// <summary>
    /// Construct a new <see cref="ParameterOverride"/> object that will
    /// override the given named constructor parameter, and pass the given
    /// value.
    /// </summary>
    /// <param name="parameterType">Type of the parameter.</param>
    /// <param name="parameterName">Name of the constructor parameter.</param>
    /// <param name="value">Value to pass for the MethodBase.</param>
    public ParameterOverride(Type parameterType, string parameterName, object value)
        : base(null, parameterName, value) 
        => Type = parameterType;

    #endregion


    #region IMatchInfo<ParameterInfo>

    /// <inheritdoc />
    public MatchRank RankMatch(ParameterInfo other)
    {
        return (Target is null || other.Member.DeclaringType == Target) &&
               (Type is null || other.ParameterType == Type) &&
               (Name is null || other.Name == Name)
            ? MatchRank.ExactMatch
            : MatchRank.NoMatch;
    }

    #endregion
}
