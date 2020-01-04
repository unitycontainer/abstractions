using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Unity
{
    internal static class Compatibility_NetStandard_1_0
    {
        public static IEnumerable<ConstructorInfo> GetConstructors(this Type type)
        {
            return type.GetTypeInfo()
                       .DeclaredConstructors
                       .Where(ctor => !ctor.IsFamily && !ctor.IsPrivate && !ctor.IsStatic);
        }

        public static FieldInfo? GetField(this Type type, string name)
        {
            return type.GetTypeInfo()
                       .DeclaredFields
                       .FirstOrDefault(p => p.Name == name);
        }


        public static PropertyInfo? GetProperty(this Type type, string name)
        {
            return type.GetTypeInfo()
                       .DeclaredProperties
                       .FirstOrDefault(p => p.Name == name);
        }
    }
}
