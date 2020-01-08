using System;
using Unity.Resolution;

namespace Unity.Lifetime
{
    public abstract partial class LifetimeManager 
    {
        #region Fields

        private IUnityContainer? _scope;

        #endregion


        #region Owner Container

        public IUnityContainer? Scope
        {
            get => _scope;
            set 
            {
                if (null != _scope && _scope != value) 
                    throw new InvalidOperationException($"This manager already registered with {_scope} scope");

                _scope = value;
            }
        }

        #endregion


        #region Lifetime Factory

        /// <summary>
        /// Creates a new lifetime manager of the same type as this Lifetime Manager
        /// </summary>
        /// <returns>A new instance of the appropriate lifetime manager</returns>
        public LifetimeManager CreateLifetimePolicy()
        {
            var manager = OnCreateLifetimeManager();
            manager.Scope = _scope;
            return manager;
        }

        #endregion


        #region Implementation

        /// <summary>
        /// Implementation of <see cref="CreateLifetimePolicy"/> policy.
        /// </summary>
        /// <returns>A new instance of the same type as this lifetime manager</returns>
        protected abstract LifetimeManager OnCreateLifetimeManager();

        #endregion


        #region Internal Use

        internal Delegate? PipelineDelegate;

        internal virtual object? Pipeline<TContext>(ref TContext context) where TContext : IResolveContext
        {
            return ((ResolveDelegate<TContext>)(PipelineDelegate ?? throw new InvalidOperationException("Pipeline is not initialized")))(ref context);
        }

        #endregion
    }
}
