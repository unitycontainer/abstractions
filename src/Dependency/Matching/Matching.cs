using System;
using System.Reflection;
using Unity.Resolution;

namespace Unity.Dependency;



public static partial class Matching
{
    public static int MatchMethod(object[]? data, MethodBase[] members)
    {
        int position = -1;
        int bestSoFar = -1;

        for (var index = 0; index < members.Length; index++)
        {
            var compatibility = MatchData(data, members[index]);

            if (0 == compatibility) return index;

            if (bestSoFar < compatibility)
            {
                position = index;
                bestSoFar = compatibility;
            }
        }

        return position;
    }

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


    #region Implementation

    public static int MatchData(object[]? data, MethodBase other)
    {
        System.Diagnostics.Debug.Assert(null != other);

        var length = data?.Length ?? 0;
        var parameters = other.GetParameters();

        if (length != parameters.Length) return -1;

        int rank = 0;
        for (var i = 0; i < length; i++)
        {
            var compatibility = (int)data![i].MatchTo(parameters[i]);

            if (0 > compatibility) return -1;
            rank += compatibility;
        }

        return (int)MatchRank.ExactMatch * parameters.Length == rank ? 0 : rank;
    }

    private static MatchRank MatchTo(this object value, ParameterInfo parameter)
    {
        switch (value)
        {
            case null:
                return !parameter.ParameterType.IsValueType || null != Nullable.GetUnderlyingType(parameter.ParameterType)
                     ? MatchRank.ExactMatch : MatchRank.NoMatch;

            case Array array:
                return array.MatchTo(parameter.ParameterType);

            case IMatchInfo<ParameterInfo> iMatchParam:
                return iMatchParam.RankMatch(parameter);

            case Type type:
                return type.MatchTo(parameter.ParameterType);

            case IResolve:
            case IResolverFactory:
                return MatchRank.HigherProspect;
        }

        var objectType = value.GetType();

        if (objectType == parameter.ParameterType)
            return MatchRank.ExactMatch;

        return parameter.ParameterType.IsAssignableFrom(objectType)
            ? MatchRank.Compatible : MatchRank.NoMatch;
    }

    #endregion
}
