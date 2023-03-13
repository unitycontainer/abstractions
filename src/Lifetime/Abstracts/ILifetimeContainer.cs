using System;
using System.Collections.Generic;

namespace Unity.Lifetime
{
    /// <summary>
    /// Interface to a container holding refereces to all the objects 
    /// it is responsible to keep alive.
    /// </summary>
    public interface ILifetimeContainer : IEnumerable<object>, IDisposable
    {
        /// <summary>
        /// The container that this context is associated with.
        /// </summary>
        /// <value>The <see cref="IUnityContainer"/> object.</value>
        IUnityContainer? Container { get; }

        /// <summary>
        /// Gets the number of references in the lifetime container
        /// </summary>
        /// <value>
        /// The number of references in the lifetime container
        /// </value>
        int Count { get; }

        /// <summary>
        /// Adds an object to the lifetime container.
        /// </summary>
        /// <param name="item">The item to be added to the lifetime container.</param>
        void Add(object item);

        /// <summary>
        /// Removes an item from the lifetime container. The item is
        /// not disposed.
        /// </summary>
        /// <param name="item">The item to be removed.</param>
        void Remove(object item);

        /// <summary>
        /// Determine if a given object is in the lifetime container.
        /// </summary>
        /// <param name="item">
        /// The item to locate in the lifetime container.
        /// </param>
        /// <returns>
        /// Returns true if the object is contained in the lifetime
        /// container; returns false otherwise.
        /// </returns>
        bool Contains(object item);
    }
}
