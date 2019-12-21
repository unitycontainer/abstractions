using System;
using Unity.Policy;
using Unity.Resolution;

namespace Unity.Injection
{
    /// <summary>
    /// This interface allows injection of policies during registration.
    /// </summary>
    public interface IAddPolicies
    {
        /// <summary>
        /// Add policies to the <paramref name="policies"/> to configure the
        /// container to call this constructor with the appropriate parameter values.
        /// </summary>
        /// <remarks>
        /// This method adds any new policies that this injection member provides.
        /// Some of the injection members install new policies and do not participate
        /// in cration of objects. <see cref="InjectionConstructor"/>, for example, injects the selector
        /// policy and relies on it during resolution. The InjectionMembers like this could be discarded
        /// after registration is done by returning false from the method.
        /// </remarks>
        /// <param name="registeredType">Type of interface being registered. If no interface,
        /// this will be null.</param>
        /// <param name="mappedToType">Type of concrete type being registered.</param>
        /// <param name="name">Name used to resolve the type object.</param>
        /// <param name="policies">The registration where new policies to be injected.</param>
        /// <returns>
        /// Returns true if this member should be added to the registration or false if it 
        /// can be discarded.
        /// </returns>
        bool AddPolicies<TContext, TPolicySet>(Type registeredType, Type mappedToType, string name, ref TPolicySet policies)
                where TContext   : IResolveContext
                where TPolicySet : IPolicySet;
    }
}
