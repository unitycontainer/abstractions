using System.Collections.Generic;
using System.Linq;
using System.Threading;


#nullable disable warnings

namespace System.Reflection
{
    internal static class IntrospectionExtensions
    {
        public static MethodInfo GetGetMethod(this PropertyInfo info, bool _)
        {
            return info.GetMethod;
        }

        public static MethodInfo GetSetMethod(this PropertyInfo info, bool _)
        {
            return info.SetMethod;
        }
    }
}

#nullable restore warnings
