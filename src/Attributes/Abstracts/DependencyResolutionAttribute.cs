using System;
using System.Reflection;
using Unity.Exceptions;
using Unity.Resolution;

namespace Unity
{
    /// <summary>
    /// Base class for attributes that can be placed on parameters
    /// or properties to specify how to resolve the value for
    /// that parameter or property.
    /// </summary>
    public abstract class DependencyResolutionAttribute : Attribute,
                                                          IResolverFactory<Type>,
                                                          IResolverFactory<ParameterInfo>,
                                                          IResolverFactory<PropertyInfo>,
                                                          IResolverFactory<FieldInfo>
    {
        #region Constructors

        protected DependencyResolutionAttribute(string? name)
        {
            Name = name;
        }

        #endregion


        #region Public Members

        /// <summary>
        /// The name specified in the constructor.
        /// </summary>
        public string? Name { get; }

        #endregion


        #region IResolverFactory

        public virtual ResolveDelegate<TContext> GetResolver<TContext>(ParameterInfo info) 
            where TContext : IResolveContext
        {
#if NET40
            if (!(info.DefaultValue is DBNull))
#else
            if (info.HasDefaultValue)
#endif
            {
                return (ref TContext context) =>
                {
                    try
                    {
                        return context.Resolve(info.ParameterType, Name);
                    }
                    catch (Exception ex) when (!(ex.InnerException is CircularDependencyException))
                    {
                        return info.DefaultValue;
                    }
                };
            }
            else
                return GetResolver<TContext>(info.ParameterType);
        }

        public ResolveDelegate<TContext> GetResolver<TContext>(PropertyInfo info)
            where TContext : IResolveContext => GetResolver<TContext>(info.PropertyType);

        public ResolveDelegate<TContext> GetResolver<TContext>(FieldInfo info)
            where TContext : IResolveContext => GetResolver<TContext>(info.FieldType);

        public abstract ResolveDelegate<TContext> GetResolver<TContext>(Type type) where TContext : IResolveContext;

        #endregion
    }
}
