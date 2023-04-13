using System;

namespace Unity.Dependency;



/// <summary>
/// Calculates how much member matches the provided value
/// </summary>
/// <typeparam name="TOther"><see cref="Type"/>One of
/// <see cref="System.Reflection.FieldInfo"/>, <see cref="System.Reflection.PropertyInfo"/>, or 
/// <see cref="System.Reflection.ParameterInfo"/> or any other type of the target</typeparam>
public interface IMatch<in TOther>
{
    /// <summary>
    /// Calculates how well this and other match
    /// </summary>
    /// <param name="other">The other item to match to</param>
    /// <returns>The rank of the match</returns>
    public MatchRank RankMatch(TOther other);
}
