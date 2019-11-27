using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.Reflection
{
#if NET40

    internal class TypeInfo 
    {
        private const BindingFlags DeclaredOnlyLookup = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
        private Type _type;


        internal TypeInfo(Type type)
        {
            _type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public bool IsGenericTypeDefinition => _type.IsGenericTypeDefinition;

        public Type[] GenericTypeArguments => _type.GetGenericArguments();

        public Type[] GenericTypeParameters => _type.IsGenericTypeDefinition ? _type.GetGenericArguments()
                                                                             : Type.EmptyTypes;
        public string Name => _type.Name;

        public Type BaseType => _type.BaseType;

        public bool IsGenericType => _type.IsGenericType;

        public Type AsType() => _type;

        public bool IsAssignableFrom(Type type) => _type.IsAssignableFrom(type);

        public bool IsAssignableFrom(TypeInfo typeInfo) => _type.IsAssignableFrom(typeInfo.AsType());

        public bool IsGenericParameter => _type.IsGenericParameter;

        public bool ContainsGenericParameters => _type.ContainsGenericParameters;

        public Type GetGenericTypeDefinition() => _type.GetGenericTypeDefinition();

    #region moved over from Type

        public virtual MethodInfo GetDeclaredMethod(String name)
        {
            return _type.GetMethod(name, DeclaredOnlyLookup);
        }

        public virtual IEnumerable<MethodInfo> GetDeclaredMethods(String name)
        {
            foreach (MethodInfo method in _type.GetMethods(DeclaredOnlyLookup))
            {
                if (method.Name == name)
                    yield return method;
            }
        }

        public virtual PropertyInfo GetDeclaredProperty(String name)
        {
            return _type.GetProperty(name, DeclaredOnlyLookup);
        }


        //// Properties

        public virtual IEnumerable<ConstructorInfo> DeclaredConstructors
        {
            get
            {
                return _type.GetConstructors(DeclaredOnlyLookup);
            }
        }

        public virtual IEnumerable<FieldInfo> DeclaredFields
        {
            get
            {
                return _type.GetFields(DeclaredOnlyLookup);
            }
        }

        public virtual IEnumerable<MethodInfo> DeclaredMethods
        {
            get
            {
                return _type.GetMethods(DeclaredOnlyLookup);
            }
        }

        public virtual IEnumerable<PropertyInfo> DeclaredProperties
        {
            get
            {
                return _type.GetProperties(DeclaredOnlyLookup);
            }
        }

    #endregion

        public override int GetHashCode()
        {
            return _type.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return _type.Equals(obj);
        }

        public static bool operator ==(TypeInfo left, TypeInfo right)
        {
            return left?.GetHashCode() == right?.GetHashCode();
        }

        public static bool operator !=(TypeInfo left, TypeInfo right)
        {
            return left?.GetHashCode() != right?.GetHashCode();
        }

    }
#endif


    internal static class IntrospectionExtensions
    {
#if NET40

        public static Attribute GetCustomAttribute(this ParameterInfo info, Type type)
        {
            return info.GetCustomAttributes(type, true)
                       .Cast<Attribute>()
                       .FirstOrDefault();
        }

        public static TypeInfo GetTypeInfo(this Type type)
        {
            return new TypeInfo(type);
        }

        public static Delegate CreateDelegate(this MethodInfo method, Type delegateType)
        {
            return Delegate.CreateDelegate(delegateType, method);
        }
#else
        public static MethodInfo? GetGetMethod(this PropertyInfo info, bool _)
        {
            return info.GetMethod;
        }

        public static MethodInfo? GetSetMethod(this PropertyInfo info, bool _)
        {
            return info.SetMethod;
        }
#endif
    }
}

