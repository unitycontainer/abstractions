using System;
using System.Reflection;

namespace Unity
{
    internal static class Compatibility_NetCoreApp_1_0
    {
        public static FieldInfo? GetField(this Type type, string name)
        { 
            return type.GetField(name);
        }
    }
}
