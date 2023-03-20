﻿using System;
using System.Reflection;

namespace Unity.Dependency
{
    public interface IInjectionInfo
    {
        #region Member Type

        /// <summary>
        /// <see cref="Type"/> of imported member, set by <see cref="MemberInfo"/>
        /// </summary>
        Type MemberType { get; }

        #endregion


        #region Contract

        Type ContractType { get; set; }

        string? ContractName { get; set; }

        #endregion


        #region Metadata

        /// <summary>
        /// True if annotated with <see cref="Unity.DependencyResolutionAttribute"/>
        /// </summary>
        bool IsImport { get; set; }


        /// <summary>
        /// Allows default value if can not be resolved
        /// </summary>
        bool AllowDefault { get; set; }


        /// <summary>
        /// Sets default value and flips <see cref="AllowDefault"/> to <see langword=""="true"/>
        /// </summary>
        object? Default { set; }


        /// <summary>
        /// Array of arguments
        /// </summary>
        object?[] Arguments { set; }


        /// <summary>
        /// Sets Data value for current import
        /// </summary>
        /// <remarks>
        /// This setter used when runtime value is not known at design time. The type of the value
        /// will be analyzed at runtime. The value could be a resolver, another <see cref="InjectionMember"/>, 
        /// and etc.
        /// </remarks>
        object? Data { set; }

        #endregion
    }

    public interface IInjectionInfo<TMemberInfo> : IInjectionInfo
    {
        /// <summary>
        /// One of <see cref="ParameterInfo"/>, <see cref="FieldInfo"/>, or
        /// <see cref=" PropertyInfo"/>
        /// </summary>
        TMemberInfo MemberInfo { get; }
    }
}
