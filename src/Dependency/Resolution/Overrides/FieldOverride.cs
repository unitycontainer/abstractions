﻿using System;
using System.Reflection;

namespace Unity.Resolution
{
    /// <summary>
    /// A <see cref="ResolverOverride"/> that lets you override
    /// the value for a specified field.
    /// </summary>
    public class FieldOverride : ResolverOverride,
                                 IEquatable<FieldInfo>,
                                 IResolve
    {
        #region Fields

        protected readonly object Value;

        #endregion


        #region Constructors

        /// <summary>
        /// Create an instance of <see cref="FieldOverride"/>.
        /// </summary>
        /// <param name="name">The Field name.</param>
        /// <param name="value">InjectionParameterValue to use for the Field.</param>
        public FieldOverride(string name, object value)
            : base(name ?? throw new ArgumentNullException(nameof(name), "Must provide a name of the field to override"))
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
                case FieldInfo info:
                    return Equals(info);

                case FieldOverride field:
                    return (null == Target || field.Target == Target) &&
                           (null == Type   || field.Type == Type) &&
                           (null == Name   || field.Name == Name);
                default:
                    return base.Equals(other);
            }
        }

        public bool Equals(FieldInfo? other)
        {
            return null != other && 
                  (null == Target || other.DeclaringType == Target) &&
                  (null == Type   || other.FieldType == Type) &&
                  (null == Name   || other.Name == Name);
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
}
