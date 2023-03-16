namespace Unity.Resolution;



/// <summary>
/// An interface exposing resolver pipeline. 
/// </summary>
public interface IResolve
{
    /// <summary>
    /// GetOrDefault the value
    /// </summary>
    /// <param name="context">Current build context.</param>
    /// <returns>The value for the dependency.</returns>
    object? Resolve<TContext>(ref TContext context)
        where TContext : IResolveContext;
}

