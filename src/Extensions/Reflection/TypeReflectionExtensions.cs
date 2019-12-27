using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Unity
{
    /// <summary>
    /// Provides extension methods to the <see cref="Type"/> class due to the introduction 
    /// of <see cref="TypeInfo"/> class.
    /// </summary>
    internal static class TypeReflectionExtensions
    {

        public static Type GetArrayParameterType(this Type typeToReflect, Type[] genericArguments)
        {
            var rank = typeToReflect.GetArrayRank();
            var element = typeToReflect.GetElementType();
            var type = element.IsArray ? element.GetArrayParameterType(genericArguments)
                                       : genericArguments[element.GenericParameterPosition];

            return 1 == rank ? type.MakeArrayType() : type.MakeArrayType(rank);
        }

#if NETSTANDARD1_0
        public static IEnumerable<FieldInfo> GetFields(this Type type)
        {
            TypeInfo? info = type.GetTypeInfo();
            while (null != info)
            {
                foreach (var member in info.DeclaredFields)
                    yield return member;

                info = info.BaseType?.GetTypeInfo();
            }
        }

        public static IEnumerable<PropertyInfo> GetProperties(this Type type)
        {
            TypeInfo? info = type.GetTypeInfo();
            while (null != info)
            {
                foreach (var member in info.DeclaredProperties)
                    yield return member;

                info = info.BaseType?.GetTypeInfo();
            }
        }

        public static IEnumerable<MethodInfo> GetMethods(this Type type)
        {
            TypeInfo? info = type.GetTypeInfo();
            while (null != info)
            {
                foreach (var member in info.DeclaredMethods)
                    yield return member;

                info = info.BaseType?.GetTypeInfo();
            }
        }
#endif

#if NET40

        public static Attribute GetCustomAttribute(this ParameterInfo info, Type type)
        {
            return info.GetCustomAttributes(type, true)
                       .Cast<Attribute>()
                       .FirstOrDefault();
        }

        public static TypeInfo GetTypeInfo(this Type type)
        {
            return new TypeInfo(type ?? throw new ArgumentNullException(nameof(type)));
        }

        public static Delegate CreateDelegate(this MethodInfo method, Type delegateType)
        {
            return Delegate.CreateDelegate(delegateType, method);
        }

        public static Delegate CreateDelegate(this MethodInfo method, Type delegateType, object target)
        {
            return Delegate.CreateDelegate(delegateType, target, method);
        }
        
        public static MethodInfo GetMethodInfo(this Delegate method)
        {
            return method.Method;
        }
#else
        public static MethodInfo GetGetMethod(this PropertyInfo info, bool _)
        {
            return info.GetMethod;
        }

        public static MethodInfo GetSetMethod(this PropertyInfo info, bool _)
        {
            return info.SetMethod;
        }
#endif
    }
}

