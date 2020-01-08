using System;

namespace Unity.Lifetime
{
    /// <summary>
    /// Base class for all lifetime managers - classes that control how
    /// and when instances are created by the Unity container.
    /// </summary>
    public abstract partial class LifetimeManager 
    {
        /// <summary>
        /// This value represents Invalid Value. Lifetime manager must return this
        /// unless value is set with a valid object. Null is a value and is not equal 
        /// to NoValue 
        /// </summary>
        public static readonly object NoValue = new InvalidValue();

        /// <summary>
        /// A <see cref="Boolean"/> indicating if this manager is being used in 
        /// one of the registrations.
        /// </summary>
        /// <remarks>
        /// The Unity container requires that each registration used its own, unique
        /// lifetime manager. This property is being used to track that condition.
        /// </remarks>
        /// <value>True is this instance already in use, False otherwise.</value>
        public virtual bool InUse { get; set; }

        
        #region Constructors

        public LifetimeManager()
        {
            Set    = SetValue;
            Get    = GetValue;
            TryGet = TryGetValue;
        }
        
        #endregion


        #region  Optimizers

        public virtual Func<ILifetimeContainer?, object?> TryGet { get; protected set; }

        public virtual Func<ILifetimeContainer?, object?> Get { get; protected set; }

        public virtual Action<object?, ILifetimeContainer?> Set { get; protected set; }

        #endregion


        #region   LifetimeManager Members

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


        #region Invalid Value

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
    }
}
