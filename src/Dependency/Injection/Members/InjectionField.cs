using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Resolution;

namespace Unity.Injection
{
    public class InjectionField : MemberInfoBase<FieldInfo>
    {
        #region Constructors

        /// <summary>
        /// Configure the container to inject the given field name.
        /// </summary>
        /// <param name="name">Name of property to inject.</param>
        /// <param name="isOptionanal">Tells Unity if this field is optional.</param>
        public InjectionField(string name, bool isOptionanal = false)
            : base(name, isOptionanal ? OptionalDependencyAttribute.Instance 
                                      : (object)DependencyAttribute.Instance)
        {
        }

        /// <summary>
        /// Configure the container to inject the given field name,
        /// using the value supplied.
        /// </summary>
        /// <param name="name">Name of property to inject.</param>
        /// <param name="value">InjectionParameterValue for property.</param>
        public InjectionField(string name, object value)
            : base(name, value)
        {
        }

        #endregion


        #region Overrides

        protected override IEnumerable<FieldInfo> DeclaredMembers(Type type) => type.SupportedFields();

        public override string ToString()
        {
            return Data is DependencyResolutionAttribute
                ? $"Resolve.Field('{Name}')"
                : $"Inject.Field('{Name}', {Data})";
        }

        #endregion


        #region Selection

        protected override FieldInfo SelectDiagnostic(Type type)
        {
            FieldInfo? selection = null;

            if (IsInitialized) throw new InvalidOperationException(
                "Sharing an InjectionField between registrations is not supported");

            // Select Field
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
                    $"Injected field '{Name}' could not be matched with any public field on type '{type?.FullName}'.");
            }

            if (selection.IsStatic)
                throw new InvalidOperationException(
                    $"Static field '{selection.Name}' on type '{type?.FullName}' cannot be injected");

            if (selection.IsInitOnly)
                throw new InvalidOperationException(
                    $"Readonly field '{selection.Name}' on type '{type?.FullName}' cannot be injected");

            if (selection.IsPrivate)
                throw new InvalidOperationException(
                    $"Private field '{selection.Name}' on type '{type?.FullName}' cannot be injected");

            if (selection.IsFamily)
                throw new InvalidOperationException(
                    $"Protected field '{selection.Name}' on type '{type?.FullName}' cannot be injected");

            if (Data is IResolve || Data is IResolverFactory<FieldInfo> || Data is IResolverFactory<Type>)
                return selection;

            if (!Data.Matches(selection.FieldType))
                throw new ArgumentException(
                    $"Injected data '{Data}' could not be matched with type of field '{selection.FieldType.FullName}'.");

            return selection;
        }

        #endregion
    }
}
