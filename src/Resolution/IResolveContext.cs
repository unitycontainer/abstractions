using System;

namespace Unity.Resolution;



public interface IResolveContext
{
    /// <summary>
    /// Type of the currently resolved contract
    /// </summary>
    Type Type { get; }

    /// <summary>
    /// Name of the resolved contract
    /// </summary>
    string? Name { get; }

    /// <summary>
    /// Method to resolve any required dependencies
    /// </summary>
    /// <param name="type">Type of requested object</param>
    /// <param name="name">Name of registration</param>
    /// <returns></returns>
    object? Resolve(Type type, string? name);
}
