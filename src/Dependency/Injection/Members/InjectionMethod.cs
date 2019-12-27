using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Unity.Injection
{
    /// <summary>
    /// An <see cref="InjectionMember"/> that configures the
    /// container to call a method as part of buildup.
    /// </summary>
    public class InjectionMethod : MethodBase<MethodInfo>
    {
        #region Constructors

        /// <summary>
        /// Create a new <see cref="InjectionMethod"/> instance which will configure
        /// the container to call the given methods with the given parameters.
        /// </summary>
        /// <param name="name">Name of the method to call.</param>
        /// <param name="arguments">Parameter values for the method.</param>
        public InjectionMethod(string name, params object[] arguments)
            : base(name, arguments)
        {
        }

        #endregion


        #region Overrides

        protected override MethodInfo FastSelectMember(Type type)
        {
            var noData = 0 == (Data?.Length ?? 0);

            foreach (var member in DeclaredMembers(type))
            {
                if (null != Name)
                {
                    if (Name != member.Name) continue;
                    if (noData) return member;
                }

                if (null != Data && !Data.MatchMemberInfo(member)) continue;

                return member;
            }

            throw new ArgumentException(NoMatchFound);
        }

        protected override MethodInfo ValidatingSelectMember(Type type)
        {
            MethodInfo? selection = null;

            if (IsInitialized) throw new InvalidOperationException("Sharing an InjectionMethod between registrations is not supported");

            // Select Method
            foreach (var info in UnityDefaults.SupportedMethods(type))
            {
                if (Name != info.Name || !Data.MatchMemberInfo(info)) continue;

                if (null != selection)
                {
                    throw new ArgumentException(
                        $"Method {Name}({Data.Signature()}) is ambiguous, it could be matched with more than one method on type {type?.Name}.");
                }

                selection = info;
            }

            // Validate
            if (null == selection)
            {
                throw new ArgumentException(
                    $"Injected method {Name}({Data.Signature()}) could not be matched with any public methods on type {type?.Name}.");
            }

            if (selection.IsStatic)
            {
                throw new ArgumentException(
                    $"Static method {Name} on type '{selection.DeclaringType?.Name}' cannot be injected");
            }

            if (selection.IsPrivate)
                throw new InvalidOperationException(
                    $"Private method '{Name}' on type '{selection.DeclaringType?.Name}' cannot be injected");

            if (selection.IsFamily)
                throw new InvalidOperationException(
                    $"Protected method '{Name}' on type '{selection.DeclaringType?.Name}' cannot be injected");

            if (selection.IsGenericMethodDefinition)
            {
                throw new ArgumentException(
                    $"Open generic method {Name} on type '{selection.DeclaringType?.Name}' cannot be injected");
            }

            var parameters = selection.GetParameters();
            if (parameters.Any(param => param.IsOut))
            {
                throw new ArgumentException(
                    $"Method {Name} on type '{selection.DeclaringType?.Name}' cannot be injected. Methods with 'out' parameters are not injectable.");
            }

            if (parameters.Any(param => param.ParameterType.IsByRef))
            {
                throw new ArgumentException(
                    $"Method {Name} on type '{selection.DeclaringType?.Name}' cannot be injected. Methods with 'ref' parameters are not injectable.");
            }

            return selection;
        }

        public override IEnumerable<MethodInfo> DeclaredMembers(Type type) => 
            UnityDefaults.SupportedMethods(type).Where(member => member.Name == Name);

        public override string ToString()
        {
            return $"Invoke.Method('{Name}', {Data.Signature()})";
        }

#if NETSTANDARD1_0

        public override bool Equals(MethodInfo other)
        {
            if (null == other || other.Name != Name) return false;

            var parameterTypes = other.GetParameters()
                                      .Select(p => p.ParameterType)
                                      .ToArray();
            
            if (null == Selection) return false;

            if (Selection.ContainsGenericParameters)
                return Data.Length == parameterTypes.Length;

            return Selection.GetParameters()
                             .Select(p => p.ParameterType)
                             .SequenceEqual(parameterTypes);
        }

#endif
        #endregion
    }
}
