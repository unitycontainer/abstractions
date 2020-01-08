using System;

namespace Unity.Lifetime
{
    public abstract partial class LifetimeManager 
    {
        #region Debugger
#if DEBUG
        public string ID { get; } = Guid.NewGuid().ToString();
#endif
        #endregion
    }
}
