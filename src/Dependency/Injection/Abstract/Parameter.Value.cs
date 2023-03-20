﻿using System;
using System.Reflection;
using Unity.Dependency;

namespace Unity.Injection;



/// <summary>
/// Base type for objects that are used to configure parameters for
/// constructor or method injection, or for getting the value to
/// be injected into a property.
/// </summary>
public abstract class ParameterValue : IInjectionProvider,
                                       IMatchInfo<ParameterInfo>
{
    #region Import Description Provider

    /// <inheritdoc/>
    public virtual void ProvideInfo<TDescriptor>(ref TDescriptor descriptor)
        where TDescriptor : IInjectionInfo
    { 
    }

    #endregion


    #region IMatch

    /// <summary>
    /// Checks if this parameter is compatible with the <see cref="ParameterInfo"/>
    /// </summary>
    /// <param name="other"><see cref="ParameterInfo"/> to compare to</param>
    /// <returns>True if <see cref="ParameterInfo"/> is compatible</returns>
    public virtual MatchRank RankMatch(ParameterInfo parameter) 
        => RankMatch(parameter.ParameterType);

    /// <summary>
    /// Match the parameter with a <see cref="Type"/>
    /// </summary>
    /// <param name="type"><see cref="Type"/> to match to</param>
    /// <returns>Rank of the match</returns>
    protected abstract MatchRank RankMatch(Type type);

    #endregion
}
