using System;
using Unity.Resolution;

namespace Unity.Lifetime
{
    /// <summary>
    /// Base class for all lifetime managers - classes that control how
    /// and when instances are created by the Unity container.
    /// </summary>
    public abstract class LifetimeManager : ICloneable
    {
        #region Fields

        /// <summary>
        /// This value represents unassigned value. 
        /// </summary>
        /// <remarks>
        /// Lifetime manager must return this instance unless value is set with a valid object. 
        /// This instance is a singleton that is used to identify and unassigned lifetime managers.
        /// </remarks>
        public static readonly object NoValue = new InvalidValue();

        private object? _scope;

        #endregion

        
        #region Constructors

        public LifetimeManager()
        {
            Set    = SetValue;
            Get    = GetValue;
            TryGet = TryGetValue;
        }

        #endregion


        #region Scope

        /// <summary>
        /// An <see cref="Object"/> holding reference to a owner container.
        /// </summary>
        /// <remarks>
        /// When registered with a container this property is set with the reference to
        /// the container.
        /// </remarks>
        public object? Scope
        {
            get => _scope; set
            {
                if (null != _scope && value != _scope) 
                    throw new InvalidOperationException($"Manager {this} is already registered with {_scope} scope");

                _scope = value;
            }
        }

        #endregion


        #region  Optimizers

        public virtual Func<ILifetimeContainer?, object?> TryGet { get; protected set; }

        public virtual Func<ILifetimeContainer?, object?> Get { get; protected set; }

        public virtual Action<object?, ILifetimeContainer?> Set { get; protected set; }

        #endregion


        #region [Try] Get/Set, Clear Value

        /// <summary>
        /// Retrieves a value from the backing store associated with this Lifetime policy.
        /// </summary>
        /// <remarks>
        /// This method does not block and does not acquire a lock on synchronization 
        /// primitives.
        /// </remarks>
        /// <param name="container">The container this lifetime is associated with</param>
        /// <returns>the object desired, or null if no such object is currently stored.</returns>
        public virtual object? TryGetValue(ILifetimeContainer? container = null) => GetValue(container);

        /// <summary>
        /// Retrieves a value from the backing store associated with this Lifetime policy.
        /// </summary>
        /// <param name="container">The container this lifetime is associated with</param>
        /// <returns>the object desired, or null if no such object is currently stored.</returns>
        public virtual object? GetValue(ILifetimeContainer? container = null) => NoValue;

        /// <summary>
        /// Stores the given value into backing store for retrieval later.
        /// </summary>
        /// <param name="newValue">The object being stored.</param>
        /// <param name="container">The container this lifetime is associated with</param>
        public virtual void SetValue(object? newValue, ILifetimeContainer? container = null) { }

        /// <summary>
        /// Remove the given object from backing store.
        /// </summary>
        /// <param name="container">The container this lifetime belongs to</param>
        public virtual void RemoveValue(ILifetimeContainer? container = null) { }

        #endregion


        #region ICloneable

        /// <summary>
        /// Creates a new lifetime manager that is a copy of the current type
        /// </summary>
        object ICloneable.Clone() => OnCreateLifetimeManager();

        /// <summary>
        /// Creates a new lifetime manager that is a copy of the current type
        /// </summary>
        /// <remarks>This overload returns <see cref="LifetimeManager"/> instead of object</remarks>
        public LifetimeManager Clone() => OnCreateLifetimeManager();

        #endregion


        #region Implementation

        /// <summary>
        /// Implementation of <see cref="CreateLifetimePolicy"/> policy.
        /// </summary>
        /// <returns>A new instance of the same lifetime manager of appropriate type</returns>
        protected abstract LifetimeManager OnCreateLifetimeManager();

        #endregion


        #region Nested Types

        public class InvalidValue
        {
            internal InvalidValue()
            {

            }

            public override bool Equals(object? obj)
            {
                return ReferenceEquals(this, obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        #endregion


        #region Internal Use

        internal Delegate? PipelineDelegate;

        internal virtual object? Pipeline<TContext>(ref TContext context) where TContext : IResolveContext
        {
            return ((ResolveDelegate<TContext>)(PipelineDelegate ?? throw new InvalidOperationException("Pipeline is not initialized")))(ref context);
        }

        #endregion


        #region Debugger
#if DEBUG
        public string ID { get; } = Guid.NewGuid().ToString();
#endif
        #endregion
    }
}
