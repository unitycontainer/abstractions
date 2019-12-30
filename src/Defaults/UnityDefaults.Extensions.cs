using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Unity
{
    internal static class UnityDefaultsExtensions
    {
        #region Supported declared members

        /// <summary>
        /// Method to query <see cref="Type"/> for supported constructors
        /// </summary>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static IEnumerable<ConstructorInfo> SupportedConstructors(this Type type) =>
             UnityDefaults.SupportedConstructors(type);


        /// <summary>
        /// Method to query <see cref="Type"/> for supported fields
        /// </summary>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static IEnumerable<FieldInfo> SupportedFields(this Type type) =>
             UnityDefaults.SupportedFields(type);


        /// <summary>
        /// Method to query <see cref="Type"/> for supported properties
        /// </summary>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static IEnumerable<PropertyInfo> SupportedProperties(this Type type) =>
             UnityDefaults.SupportedProperties(type);


        /// <summary>
        /// Method to query <see cref="Type"/> for supported methods
        /// </summary>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static IEnumerable<MethodInfo> SupportedMethods(this Type type) =>
             UnityDefaults.SupportedMethods(type);

        #endregion
    }
}
