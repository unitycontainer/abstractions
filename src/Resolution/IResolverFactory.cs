using System;

namespace Unity.Resolution;


public delegate ResolveDelegate<TContext> ResolverFactory<TContext>(Type type)
    where TContext : IResolveContext;


public interface IResolverFactory { }


public interface IResolverFactory<in TMemberInfo> : IResolverFactory
{
    ResolveDelegate<TContext> GetResolver<TContext>(TMemberInfo info)
        where TContext : IResolveContext;
}
