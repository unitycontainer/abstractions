using System;

namespace Unity.Dependency;



/// <summary>
/// Calculates how much member matches the provided value
/// </summary>
/// <typeparam name="TOther"><see cref="Type"/>One of
/// <see cref="System.Reflection.FieldInfo"/>, <see cref="System.Reflection.PropertyInfo"/>, or 
/// <see cref="System.Reflection.ParameterInfo"/> of the target to match to</typeparam>
public interface IMatchInfo<in TOther>
{
    public MatchRank RankMatch(TOther other);
}
