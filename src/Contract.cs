﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Unity;




/// <summary>
/// Structure holding contract information
/// </summary>
[DebuggerDisplay("Contract: Type = { Type?.Name }, Name = { Name }")]
[StructLayout(LayoutKind.Sequential)]
public readonly struct Contract
{
    #region Constants

    /// <summary>
    /// Marker constant for Catch All name
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public const string AnyContractName = "Any Contract Name";

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public static int AnyNameHash = AnyContractName.GetHashCode();

    #endregion


    #region Public Members

    // Do not change sequence

    /// <summary>
    /// Hash code of the contract
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
    public readonly int HashCode;

    /// <summary>
    /// <see cref="Type"/> of the contract
    /// </summary>
    public readonly Type Type;

    /// <summary>
    /// Name of the contract
    /// </summary>
    public readonly string? Name;

    #endregion


    #region Constructors

    internal Contract(int hash, Type type, string? name = null)
    {
        Type = type;
        Name = name;
        HashCode = hash;
    }

    public Contract(Type type, string? name = null)
    {
        Type = type;
        Name = name;
        HashCode = type.GetHashCode() ^ (name?.GetHashCode() ?? 0);
    }

    #endregion


    #region Public Members

    public Contract GetGenericTypeDefinition() 
        => new Contract(Type.GetGenericTypeDefinition(), Name);

    public Contract With(Type type) => new Contract(type, Name);

    public Contract With(string? name) => new Contract(Type, name);

    #endregion


    #region Implementation

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => HashCode;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode(int first, int second) => first ^ second;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode(Type type, string? name) => type.GetHashCode() ^ (name?.GetHashCode() ?? 0);

    public override bool Equals(object? obj)
    {
        if (obj is Contract other && ReferenceEquals(Type, other.Type) && Name == other.Name)
            return true;

        return false;
    }

    public static bool operator ==(Contract obj1, Contract obj2)
    {
        return ReferenceEquals(obj1.Type, obj2.Type) && obj1.Name == obj2.Name;
    }

    public static bool operator !=(Contract obj1, Contract obj2)
    {
        return !ReferenceEquals(obj1.Type, obj2.Type) || obj1.Name != obj2.Name;
    }

    public override string ToString()
    {
        return $"Contract: Type = {Type?.Name ?? "null"},  Name = {Name ?? "null"}";
    }

    #endregion
}
