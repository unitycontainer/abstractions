﻿using System;
using System.Reflection;

namespace Unity.Injection;



public sealed class InjectionField : InjectionMemberInfo<FieldInfo>
{
    #region Constructors

    /// <summary>
    /// Configures the container to inject specified field with a value imported
    /// based on field type
    /// </summary>
    /// <remarks>
    /// The field is injected with value imported from the container. The contract
    /// of the import is determined by <see cref="Type"/> of the field and no name.
    /// If the field is annotated with <see cref="DependencyResolutionAttribute"/>, 
    /// the attribute is ignored.
    /// </remarks>
    /// <param name="fieldName">Name of the field to inject.</param>
    public InjectionField(string fieldName)
        : base(fieldName, false)
    {
    }

    /// <summary>
    /// Configures the container to inject specified field with specific <see cref="Contract"/>
    /// </summary>
    /// <remarks>
    /// The field is injected with value imported from the container. The contract
    /// of the import is determined by provided <see cref="Type"/> and name.
    /// If the field is annotated with <see cref="DependencyResolutionAttribute"/>, 
    /// the attribute is ignored.
    /// </remarks>
    /// <param name="fieldName">Name of the field to inject.</param>
    /// <param name="contractType"><see cref="Type"/> of imported <see cref="Contract"/></param>
    /// <param name="contractName">Name of imported <see cref="Contract"/></param>
    public InjectionField(string fieldName, Type contractType, string? contractName)
        : base(fieldName, contractType, contractName, false)
    {
    }

    /// <summary>
    /// Configures the container to inject the given field with provided value.
    /// </summary>
    /// <remarks>
    /// If the field is annotated with <see cref="DependencyResolutionAttribute"/>, 
    /// the attribute is ignored.
    /// </remarks>
    /// <param name="fieldName">Name of the field to inject.</param>
    /// <param name="value">Value to be injected into the field</param>
    public InjectionField(string fieldName, object value)
        : base(fieldName, value, false)
    {
    }

    #endregion
}
