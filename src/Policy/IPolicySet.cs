﻿using System;

namespace Unity.Policy
{
    /// <summary>
    /// This interface for manipulation of policies
    /// </summary>
    public interface IPolicySet
    {
        /// <summary>
        /// Get policy
        /// </summary>
        /// <param name="type"><see cref="Type"/> of policy to retrieve</param>
        /// <returns>The current policy; returns null if policy has not been set</returns>
        object? Get(Type type);

        /// <summary>
        /// Set policy
        /// </summary>
        /// <param name="type"><see cref="Type"/> of policy to set</param>
        /// <param name="policy">Policy instance</param>
        void Set(Type type, object policy);

        /// <summary>
        /// Remove policy
        /// </summary>
        /// <param name="type"><see cref="Type"/> of policy to be removed</param>
        /// <param name="unsubscribe">Indicates if clean should unsubscribe all notification handlers</param>
        void Clear(Type type);
    }
}
