﻿using System;
using System.Reflection;

namespace Unity.Resolution
{
    public static partial class Matching
    {
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

        public static MatchRank MatchTo(this object value, ParameterInfo parameter)
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
    }
}
