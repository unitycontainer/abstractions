using System;
using System.Linq;
using System.Reflection;

namespace Unity.Injection
{
    public abstract class MemberInfoBase<TMemberInfo> : InjectionMember<TMemberInfo, object>
                                    where TMemberInfo : MemberInfo
    {
        #region Constructors

        protected MemberInfoBase(string name, object data) 
            : base(name, data)
        {
        }


        #endregion


        #region Overrides

        public override TMemberInfo MemberInfo(Type type)
        {
            if (null == Selection) throw new InvalidOperationException($"Injection Member '{this}' is not initialized");

#if NETSTANDARD1_0 || NETCOREAPP1_0 
            var declaringType = Selection.DeclaringType.GetTypeInfo();
            if (null != declaringType && !declaringType.IsGenericType && !declaringType.ContainsGenericParameters)
                return Selection;
#else
            if (Selection.DeclaringType != null &&
               !Selection.DeclaringType.IsGenericType &&
               !Selection.DeclaringType.ContainsGenericParameters)
                return Selection;
#endif

            return DeclaredMembers(type).First(p => p.Name == Selection.Name);
        }

#if NETSTANDARD1_0
        public override bool Equals(TMemberInfo? other)
        {
            return null != other && other.Name == Name;
        }
#endif
        #endregion


        #region Selection

        protected override TMemberInfo Select(Type type)
        {
            foreach (var member in DeclaredMembers(type))
            {
                if (member.Name != Name) continue;

                return member;
            }

            throw new ArgumentException(NoMatchFound);
        }

        #endregion
    }
}
