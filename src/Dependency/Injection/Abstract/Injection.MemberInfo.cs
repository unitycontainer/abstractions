using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.Dependency;

namespace Unity.Injection;



public abstract class InjectionMemberInfo<TMemberInfo> : InjectionMember<TMemberInfo, object>
                                     where TMemberInfo : MemberInfo
{
    #region Singleton
    
    protected static readonly object NoValue = new InvalidValue();
    
    #endregion


    #region Fields

    private readonly Type?   _contractType;
    private readonly string? _contractName;
    private readonly bool _optional;

    #endregion


    #region Constructors

    protected InjectionMemberInfo(string member, object data, bool optional)
        : base(member, data)
    {
        _contractName = Contract.AnyContractName;
        _optional = optional;
    }

    protected InjectionMemberInfo(string member, bool optional)
        : base(member, NoValue)
    {
        _optional = optional;
    }

    protected InjectionMemberInfo(string member, Type contractType, string? contractName, bool optional)
        : base(member, NoValue)
    {
        _contractType = contractType;
        _contractName = contractName;
        _optional = optional;
    }

    #endregion


    #region Implementation

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override MatchRank RankMatch(TMemberInfo other)
        => other.Name != Name
            ? MatchRank.NoMatch
            : ReferenceEquals(base.Data, NoValue)
                ? MatchRank.ExactMatch
                : MatchRank.Compatible;


    /// <inheritdoc/>
    public override void ProvideInfo<TInjectionInfo>(ref TInjectionInfo info)
    {
        if (Data is IInjectionInfoProvider provider)
        { 
            provider.ProvideInfo(ref info);
            return;
        }

        // Optional
        info.AllowDefault = _optional;

        // Type
        if (Data is Type target && typeof(Type) != info.MemberType)
        {
            info.ContractType = target;
            info.ContractName = null;
            return;
        }

        if (_contractType is not null && !ReferenceEquals(info.ContractType, _contractType))
                info.ContractType = _contractType!;
            
        if (!ReferenceEquals(_contractName, Contract.AnyContractName))
                info.ContractName = _contractName;

        // Data
        if (!ReferenceEquals(NoValue, Data)) info.Data = Data;
    }

    #endregion


    #region Implementation

    protected class InvalidValue
    {
        internal InvalidValue() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object? obj) => ReferenceEquals(this, obj);

        public override int GetHashCode() => 0x55555555;

        public override string ToString() => "Invalid object singleton";
    }

    #endregion
}
