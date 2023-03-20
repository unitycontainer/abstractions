using System;
using Unity.Resolution;

namespace Unity.Dependency;



public static partial class Matching
{
    /// <summary>
    /// Match an object to specified type
    /// </summary>
    /// <param name="value">The object</param>
    /// <param name="target">Type to match to</param>
    /// <returns>Rank of the match</returns>
    public static MatchRank MatchTo(this object? value, Type target)
    {
        switch (value)
        {
            case null:
                return !target.IsValueType || null != Nullable.GetUnderlyingType(target)
                     ? MatchRank.ExactMatch : MatchRank.NoMatch;

            case Array array:
                return array.MatchTo(target);

            case IMatchInfo<Type> iMatchType:
                return iMatchType.RankMatch(target);

            case Type type:
                return type.MatchTo(target);

            case IResolve:
            case IResolverFactory:
                return MatchRank.HigherProspect;
        }

        var objectType = value.GetType();

        if (objectType == target)
            return MatchRank.ExactMatch;

        return target.IsAssignableFrom(objectType)
            ? MatchRank.Compatible : MatchRank.NoMatch;
    }


    /// <summary>
    /// Match a type to target type
    /// </summary>
    /// <param name="type">Type to be matched</param>
    /// <param name="target">Type to match to</param>
    /// <returns>Rank of the match</returns>
    public static MatchRank MatchTo(this Type type, Type target)
    {
        if (typeof(Type).Equals(target))
            return MatchRank.ExactMatch;

        if (type == target || Nullable.GetUnderlyingType(type) == target)
            return MatchRank.HigherProspect;

        if (target.IsAssignableFrom(type))
            return MatchRank.Compatible;

        if (typeof(Array) == type && target.IsArray)
            return MatchRank.HigherProspect;

        if (type.IsGenericType && type.IsGenericTypeDefinition && target.IsGenericType &&
            type.GetGenericTypeDefinition() == target.GetGenericTypeDefinition())
            return MatchRank.ExactMatch;

        return MatchRank.NoMatch;
    }


    /// <summary>
    /// Match am array to target type
    /// </summary>
    /// <param name="array">Array to match to type</param>
    /// <param name="target">Type to match to</param>
    /// <returns>Rank of the match</returns>
    public static MatchRank MatchTo(this Array array, Type target)
    {
        var type = array.GetType();

        if (target == type) return MatchRank.ExactMatch;
        if (target == typeof(Array)) return MatchRank.HigherProspect;

        return target.IsAssignableFrom(type)
            ? MatchRank.Compatible
            : MatchRank.NoMatch;
    }
}
