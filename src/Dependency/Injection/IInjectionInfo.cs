using System;
using System.Reflection;

namespace Unity.Injection;



public interface IInjectionInfo
{
    /// <summary>
    /// <see cref="Type"/> of imported member, set by <see cref="MemberInfo"/>
    /// </summary>
    Type MemberType { get; }


    /// <summary>
    /// Allows default value if value can not be resolved
    /// </summary>
    bool AllowDefault { get; set; }


    /// <summary>
    /// <see cref="Type"/> of the resolved contract
    /// </summary>
    Type ContractType { get; set; }


    /// <summary>
    /// Name of the resolved contract
    /// </summary>
    string? ContractName { get; set; }



    #region Data Setters

    /// <summary>
    /// Sets Data value for current import
    /// </summary>
    /// <remarks>
    /// This setter used when runtime value is not known at design time. The type of the value
    /// will be analyzed at runtime. The value could be a resolver, another <see cref="InjectionMember"/>, 
    /// and etc.
    /// </remarks>
    object? Data { set; }

    /// <summary>
    /// Array of arguments
    /// </summary>
    object?[] Arguments { set; }

    /// <summary>
    /// Sets default value and flips <see cref="AllowDefault"/> to <see langword=""="true"/>
    /// </summary>
    object? Default { set; }

    #endregion
}
