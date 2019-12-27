using System;
using System.Collections.Generic;
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

        public static IEnumerable<FieldInfo> GetDeclaredFields(this Type type)
        {
            TypeInfo? info = type.GetTypeInfo();
            while (null != info)
            {
                foreach (var member in info.DeclaredFields)
                    yield return member;

                info = info.BaseType?.GetTypeInfo();
            }
        }

        public static IEnumerable<PropertyInfo> GetDeclaredProperties(this Type type)
        {
            TypeInfo? info = type.GetTypeInfo();
            while (null != info)
            {
                foreach (var member in info.DeclaredProperties)
                    yield return member;

                info = info.BaseType?.GetTypeInfo();
            }
        }

        public static IEnumerable<MethodInfo> GetDeclaredMethods(this Type type)
        {
            TypeInfo? info = type.GetTypeInfo();
            while (null != info)
            {
                foreach (var member in info.DeclaredMethods)
                    yield return member;

                info = info.BaseType?.GetTypeInfo();
            }
        }
    }
}

