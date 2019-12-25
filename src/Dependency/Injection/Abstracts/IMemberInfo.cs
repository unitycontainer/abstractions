using System;
using System.Reflection;

namespace Unity.Injection
{
    public interface IMemberInfo<TMemberInfo>
        where TMemberInfo : MemberInfo
    {
        // TODO: optimize to take array of members
        TMemberInfo? MemberInfo(Type type);
    }
}
