﻿namespace Unity.Dependency;



/// <summary>
/// Levels of how members are matching against each other
/// </summary>
public enum MatchRank : int
{
    /// <summary>
    /// No match
    /// </summary>
    NoMatch = -1,

    /// <summary>
    /// The value is assignable
    /// </summary>
    Compatible = 1,

    /// <summary>
    /// High probability of a match
    /// </summary>
    HigherProspect = 2,

    /// <summary>
    /// Value matches exactly
    /// </summary>
    ExactMatch = 3,
}
