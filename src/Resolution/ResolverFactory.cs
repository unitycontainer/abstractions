using System;

namespace Unity.Resolution;


public delegate ResolveDelegate<TContext> ResolverFactory<TContext>(Type type)
    where TContext : IResolveContext;
