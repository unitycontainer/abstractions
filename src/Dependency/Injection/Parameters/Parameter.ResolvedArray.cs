﻿using System;
using System.Diagnostics;

namespace Unity.Injection;



/// <summary>
/// A class that stores a type, and generates a 
/// resolver object that resolves all the named instances or the
/// type registered in a container.
/// </summary>
[DebuggerDisplay("ResolvedArrayParameter: Type={ParameterType.Name}")]
public class ResolvedArrayParameter : ParameterBase
{
    #region Fields

    private readonly object[] _elementValues;

    #endregion


    #region Constructors

    /// <summary>
    /// Construct a new <see cref="ResolvedArrayParameter"/> that
    /// resolves to the given element type and collection of element values.
    /// </summary>
    /// <param name="elementType">The type of elements to resolve.</param>
    /// <param name="elementValues">The values for the elements, that will
    /// be converted to <see cref="ParameterValue"/> objects.</param>
    public ResolvedArrayParameter(Type elementType, params object[] elementValues)
        : this(elementType.MakeArrayType(), elementType, elementValues)
    {
    }

    /// <summary>
    /// Construct a new <see cref="ResolvedArrayParameter"/> that
    /// resolves to the given array and element types and collection of element values.
    /// </summary>
    /// <param name="contractType">The contract type for the array</param>
    /// <param name="elementType">The type of elements to resolve</param>
    /// <param name="elementValues">The values for the elements, that will
    /// be converted to <see cref="ParameterValue"/> objects.</param>
    protected ResolvedArrayParameter(Type contractType, Type elementType, object[] elementValues)
        : base(contractType, false)
    {
        _elementValues = elementValues;
    }

    #endregion


    #region Reflection

    public override void ProvideInfo<TInjectionInfo>(ref TInjectionInfo info)
    {
        info.ContractType = ParameterType!;
        info.Arguments    = _elementValues;
    }

    #endregion


    #region Implementation

    public override string ToString() 
        => $"ResolvedArrayParameter: Type={ParameterType!.Name}";

    #endregion
}


#region Generic

/// <summary>
/// A generic version of <see cref="ResolvedArrayParameter"/> for convenience
/// when creating them by hand.
/// </summary>
/// <typeparam name="TElement">Type of the elements for the array of the parameter.</typeparam>
public class ResolvedArrayParameter<TElement> : ResolvedArrayParameter
{
    /// <summary>
    /// Construct a new <see cref="ResolvedArrayParameter{TElement}"/> that
    /// resolves to the given element generic type with the given element values.
    /// </summary>
    /// <param name="elementValues">The values for the elements, that will
    /// be converted to <see cref="ParameterValue"/> objects.</param>
    public ResolvedArrayParameter(params object[] elementValues)
        : base(typeof(TElement[]), typeof(TElement), elementValues)
    {
    }
}

#endregion
