using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Resolution;

namespace Unity.Injection
{
    /// <summary>
    /// This class stores information about which properties to inject,
    /// and will configure the container accordingly.
    /// </summary>
    public class InjectionProperty : MemberInfoBase<PropertyInfo>
    {
        #region Constructors

        /// <summary>
        /// Configure the container to inject the given property name,
        /// using the value supplied.
        /// </summary>
        /// <param name="name">Name of property to inject.</param>
        /// <param name="isOptional">Tells Unity if this field is optional.</param>
        public InjectionProperty(string name, bool isOptional = false)
            : base(name, isOptional ? OptionalDependencyAttribute.Instance
                                    : (object)DependencyAttribute.Instance)
        {
        }

        /// <summary>
        /// Configure the container to inject the given property name,
        /// using the value supplied.
        /// </summary>
        /// <param name="name">Name of property to inject.</param>
        /// <param name="value">InjectionParameterValue for property.</param>
        public InjectionProperty(string name, object value)
            : base(name, value)
        {
        }

        #endregion


        #region Overrides

        protected override IEnumerable<PropertyInfo> DeclaredMembers(Type type) => UnityDefaults.SupportedProperties(type);

        public override string ToString()
        {
            return Data is DependencyResolutionAttribute 
                ? $"Resolve.Property('{Name}')"
                : $"Inject.Property('{Name}', {Data})";
        }

        #endregion


        #region Selection

        protected override PropertyInfo SelectDiagnostic(Type type)
        {
            PropertyInfo? selection = null;

            if (IsInitialized) throw new InvalidOperationException("Sharing an InjectionProperty between registrations is not supported");

            // Select Property
            foreach (var info in DeclaredMembers(type))
            {
                if (info.Name != Name) continue;

                selection = info;
                break;
            }

            // Validate
            if (null == selection)
            {
                throw new ArgumentException(
                    $"Injected property '{Name}' could not be matched with any property on type '{type?.FullName}'.");
            }

            if (!selection.CanWrite)
                throw new InvalidOperationException(
                    $"Readonly property '{selection.Name}' on type '{type?.FullName}' cannot be injected");

            if (0 != selection.GetIndexParameters().Length)
                throw new InvalidOperationException(
                    $"Indexer '{selection.Name}' on type '{type?.FullName}' cannot be injected");

            var setter = selection.GetSetMethod(true);

            if (null == setter)
                throw new InvalidOperationException(
                    $"Readonly property '{selection.Name}' on type '{type?.FullName}' cannot be injected");

            if (setter.IsStatic)
                throw new InvalidOperationException(
                    $"Static property '{selection.Name}' on type '{type?.FullName}' cannot be injected");

            if (setter.IsPrivate)
                throw new InvalidOperationException(
                    $"Private property '{selection.Name}' on type '{type?.FullName}' cannot be injected");

            if (setter.IsFamily)
                throw new InvalidOperationException(
                    $"Protected property '{selection.Name}' on type '{type?.FullName}' cannot be injected");

            if (Data is IResolve || Data is IResolverFactory<FieldInfo> || Data is IResolverFactory<Type>)
                return selection;

            if (!Data.Matches(selection.PropertyType))
            {
                throw new ArgumentException(
                    $"Injected data '{Data}' could not be matched with type of property '{selection.PropertyType.FullName}'.");
            }

            return selection;
        }

        #endregion
    }
}
