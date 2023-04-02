﻿namespace Unity.Resolution;



public interface IResolverFactory { }


public interface IResolverFactory<in TMemberInfo> : IResolverFactory
{
    ResolveDelegate<TContext> GetResolver<TContext>(TMemberInfo info)
        where TContext : IResolveContext;
}
