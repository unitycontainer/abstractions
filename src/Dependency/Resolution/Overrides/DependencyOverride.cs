using System;

namespace Unity.Resolution
{
    /// <summary>
    /// A <see cref="ResolverOverride"/> class that overrides
    /// the value injected whenever there is a dependency of the
    /// given type, regardless of where it appears in the object graph.
    /// </summary>
    public class DependencyOverride : ResolverOverride, 
                                      IEquatable<NamedType>,
                                      IResolve
    {
        #region Fields

        protected readonly object? Value;

        #endregion


        #region Constructors

        /// <summary>
        /// Create an instance of <see cref="DependencyOverride"/> to override
        /// the given contract type with the given value.
        /// </summary>
        /// <param name="contractType">Type to override</param>
        /// <param name="value">Value to override with</param>
        public DependencyOverride(Type contractType, object? value)
            : base(null, contractType, null)
        {
            Value = value;
        }

        /// <summary>
        /// Create an instance of <see cref="DependencyOverride"/> to override
        /// dependencies matching the given name
        /// </summary>
        /// <param name="contractName">Name of the registration</param>
        /// <param name="value">Value to override with</param>
        public DependencyOverride(string contractName, object? value)
            : base(null, null, contractName)
        {
            Value = value;
        }

        /// <summary>
        /// Create an instance of <see cref="DependencyOverride"/> to override
        /// dependencies matching the given type and a name
        /// </summary>
        /// <param name="contractName">Name of the registration</param>
        /// <param name="contractType">Type of the registration</param>
        /// <param name="value">Value to override with</param>
        public DependencyOverride(Type contractType, string contractName, object? value)
            : base(null, contractType, contractName)
        {
            Value = value;
        }

        /// <summary>
        /// Create an instance of <see cref="DependencyOverride"/> to override
        /// dependency on specific type matching the given type and a name
        /// </summary>
        /// <param name="targetType">Target <see cref="Type"/> to override dependency on</param>
        /// <param name="contractName">Name of the registration</param>
        /// <param name="contractType">Type of the registration</param>
        /// <param name="value">Value to override with</param>
        public DependencyOverride(Type? targetType, Type contractType, string? contractName, object? value)
            : base(targetType, contractType, contractName)
        {
            Value = value;
        }

        #endregion


        #region IEquatable

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object? other)
        {
            switch (other)
            {
                case DependencyOverride dependency:
                    return (dependency.Type == Type) &&
                           (dependency.Name == Name);
                
                case NamedType type:
                    return Equals(type);

                default:
                    return false;
            }
        }

        public bool Equals(NamedType other)
        {
            return (other.Type == Type) &&
                   (other.Name == Name);
        }

        #endregion


        #region IResolverPolicy

        public object? Resolve<TContext>(ref TContext context) 
            where TContext : IResolveContext
        {
            if (Value is IResolve policy)
                return policy.Resolve(ref context);

            if (Value is IResolverFactory<Type> factory)
            {
                var resolveDelegate = factory.GetResolver<TContext>(Type!);
                return resolveDelegate(ref context);
            }

            return Value;
        }

        #endregion
    }

    /// <summary>
    /// A convenience version of <see cref="DependencyOverride"/> that lets you
    /// specify the dependency type using generic syntax.
    /// </summary>
    /// <typeparam name="T">Type of the dependency to override.</typeparam>
    public class DependencyOverride<T> : DependencyOverride
    {
        /// <summary>
        /// Create an instance of <see cref="DependencyOverride"/> to override
        /// dependencies matching the given type and a name
        /// </summary>
        /// <remarks>
        /// This constructor creates an override that will match with any
        /// target type as long as the dependency type and name match. To 
        /// target specific type use <see cref="ResolverOverride.OnType(Type)"/> 
        /// method.
        /// </remarks>
        /// <param name="target">Target type to override dependency on</param>
        /// <param name="name">Name of the dependency</param>
        /// <param name="value">Override value</param>
        public DependencyOverride(Type target, string name, object? value)
            : base(target, typeof(T), name, value)
        {
        }

        /// <summary>
        /// Create an instance of <see cref="DependencyOverride"/> to override
        /// dependencies matching the given type and a name
        /// </summary>
        /// <remarks>
        /// This constructor creates an override that will match with any
        /// target type as long as the dependency type and name match. To 
        /// target specific type use <see cref="ResolverOverride.OnType(Type)"/> 
        /// method.
        /// </remarks>
        /// <param name="name">Name of the dependency</param>
        /// <param name="value">Override value</param>
        public DependencyOverride(string name, object? value)
            : base(null, typeof(T), name, value)
        {
        }


        /// <summary>
        /// Construct a new <see cref="DependencyOverride{T}"/> object that will
        /// override the given dependency, and pass the given value.
        /// </summary>
        public DependencyOverride(object? value)
            : base(null, typeof(T), null, value)
        {
        }
    }
}
