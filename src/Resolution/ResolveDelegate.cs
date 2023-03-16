namespace Unity.Resolution;


/// <summary>
/// Resolution pipeline. This type is used by the container to hold 
/// compiled pipelines.
/// </summary>
/// <typeparam name="TContext">Type of resolution context</typeparam>
/// <param name="context"><see cref="IResolveContext"/> of currently
/// resolved dependency</param>
/// <returns></returns>
public delegate object? ResolveDelegate<TContext>(ref TContext context)
    where TContext : IResolveContext;
