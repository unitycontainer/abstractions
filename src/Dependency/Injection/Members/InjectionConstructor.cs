using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Unity.Injection
{
    /// <summary>
    /// A class that holds the collection of information
    /// for a constructor, so that the container can
    /// be configured to call this constructor.
    /// </summary>
    public class InjectionConstructor : MethodBase<ConstructorInfo>
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="InjectionConstructor"/> that looks
        /// for a constructor with the given set of parameters.
        /// </summary>
        /// <param name="arguments">The values for the constructor's parameters, that will
        /// be used to create objects.</param>
        public InjectionConstructor(params object[] arguments)
            : base(arguments)
        {
        }

        public InjectionConstructor(ConstructorInfo info, params object[] arguments)
            : base(arguments)
        {
            Selection = info;
        }

        #endregion


        #region Overrides

        public override ConstructorInfo MemberInfo(Type type)
        {
            if (null == Selection) throw new InvalidOperationException(ErrorInvalidSelection);

            if (type == Selection.DeclaringType) return Selection;

#if NETSTANDARD1_0 || NETCOREAPP1_0
            if (Selection.GetParameters().Select(p => p.ParameterType.GetTypeInfo())
                         .Any(i => i.IsGenericType && i.ContainsGenericParameters)) 
#else
            if (Selection.DeclaringType?.IsGenericTypeDefinition ?? false) 
#endif
            {
                return base.MemberInfo(type);
            }

            var original = Selection.GetParameters();
            foreach (var member in DeclaredMembers(type))
            {
                var paremeters = member.GetParameters();
                
                if (original.Length != paremeters.Length) continue;
                for (var i = 0; i < original.Length; i++)
                {
                    var left = original[i];
                    var right = paremeters[i];

                    if (left.ParameterType != right.ParameterType || left.Name != right.Name)
                        continue;
                }

                return member;
            }

            throw new InvalidOperationException($"Unable to select compatible construcotr on type {type}");
        }

        protected override ConstructorInfo FastSelectMember(Type type)
        {
            foreach (var member in DeclaredMembers(type))
            {
                if (!Data.MatchMemberInfo(member)) continue;

                return member;
            }

            throw new ArgumentException(NoMatchFound);
        }

        protected override ConstructorInfo ValidatingSelectMember(Type type)
        {
            ConstructorInfo? selection = null;

            if (IsInitialized) throw new InvalidOperationException("Sharing an InjectionConstructor between registrations is not supported");

            // Select Constructor
            foreach (var info in DeclaredMembers(type))
            {
                if (!Data.MatchMemberInfo(info)) continue;

                if (null != selection)
                {
                    throw new ArgumentException(
                        $"Constructor .ctor({Data.Signature()}) is ambiguous, it could be matched with more than one constructor on type {type?.Name}.");
                }

                selection = info;
            }

            // Validate
            if (null != selection) return selection;

            throw new ArgumentException(
                $"Injected constructor .ctor({Data.Signature()}) could not be matched with any public constructors on type {type?.Name}.");
        }

        public override IEnumerable<ConstructorInfo> DeclaredMembers(Type type) => UnityDefaults.SupportedConstructors(type);

        public override string ToString()
        {
            return $"Invoke.Constructor({Data.Signature()})";
        }

        #endregion
    }
}
