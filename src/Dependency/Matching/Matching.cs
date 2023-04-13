using System;
using System.Reflection;
using Unity.Resolution;

namespace Unity.Dependency;



public static partial class Matching
{
    /// <summary>
    /// Match a type to target type
    /// </summary>
    /// <param name="target">Type to be matched</param>
    /// <param name="type">Type to match to</param>
    /// <returns>Rank of the match</returns>
    public static MatchRank MatchTo(this Type target, Type type)
    {
        if (typeof(Type).Equals(type))
            return MatchRank.ExactMatch;

        if (target == type || Nullable.GetUnderlyingType(target) == type)
            return MatchRank.HigherProspect;

        if (type.IsAssignableFrom(target))
            return MatchRank.Compatible;

        if (typeof(Array) == target && type.IsArray)
            return MatchRank.HigherProspect;

        if (target.IsGenericType && target.IsGenericTypeDefinition && type.IsGenericType &&
            target.GetGenericTypeDefinition() == type.GetGenericTypeDefinition())
            return MatchRank.ExactMatch;

        return MatchRank.NoMatch;
    }

    /// <summary>
    /// Match am array to target type
    /// </summary>
    /// <param name="array">Array to match to type</param>
    /// <param name="type">Type to match to</param>
    /// <returns>Rank of the match</returns>
    public static MatchRank MatchTo(this Array array, Type type)
    {
        var target = array.GetType();

        if (type == target) return MatchRank.ExactMatch;
        if (type == typeof(Array)) return MatchRank.HigherProspect;

        return type.IsAssignableFrom(target)
            ? MatchRank.Compatible
            : MatchRank.NoMatch;
    }

    /// <summary>
    /// Match an object to specified type
    /// </summary>
    /// <param name="value">The object</param>
    /// <param name="type">Type to match to</param>
    /// <returns>Rank of the match</returns>
    public static MatchRank MatchValue(this object? value, Type type)
    {
        switch (value)
        {
            case null:
                return !type.IsValueType || null != Nullable.GetUnderlyingType(type)
                     ? MatchRank.ExactMatch : MatchRank.NoMatch;

            case Array array:
                return array.MatchTo(type);

            case IMatch<Type> iMatchType:
                return iMatchType.RankMatch(type);

            case Type target:
                return target.MatchTo(type);

            case IResolve:
            case IResolverFactory:
                return MatchRank.HigherProspect;
        }

        var objectType = value.GetType();

        if (objectType == type)
            return MatchRank.ExactMatch;

        return type.IsAssignableFrom(objectType)
            ? MatchRank.Compatible : MatchRank.NoMatch;
    }

    private static MatchRank MatchValue(this object value, ParameterInfo parameter)
    {
        switch (value)
        {
            case null:
                return !parameter.ParameterType.IsValueType || null != Nullable.GetUnderlyingType(parameter.ParameterType)
                     ? MatchRank.ExactMatch : MatchRank.NoMatch;

            case Array array:
                return array.MatchTo(parameter.ParameterType);

            case IMatch<ParameterInfo> iMatchParam:
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

    /// <summary>
    /// Calculates how much data matches the <see cref="MethodBase"/>
    /// </summary>
    /// <param name="data">Array of objects to validate against the 
    /// <see cref="MethodBase"/></param>
    /// <param name="info">The <see cref="MethodBase"/> to match to</param>
    /// <returns>
    /// -1 if no match found
    /// 0 - if exact match or
    /// positive number ranking the match
    /// </returns>
    public static int MatchData(object[]? data, MethodBase info)
    {
        System.Diagnostics.Debug.Assert(null != info);

        var length = data?.Length ?? 0;
        var parameters = info.GetParameters();

        if (length != parameters.Length) return -1;

        int rank = 0;
        for (var i = 0; i < length; i++)
        {
            var compatibility = (int)data![i].MatchValue(parameters[i]);

            if (0 > compatibility) return -1;
            rank += compatibility;
        }

        return (int)MatchRank.ExactMatch * parameters.Length == rank ? 0 : rank;
    }
}
