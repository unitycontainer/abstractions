﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reflection;

namespace Unity.Builder.Operation
{
    /// <summary>
    /// A class that records that a constructor is about to be call, and is 
    /// responsible for generating the error string required when
    /// an error has occurred.
    /// </summary>
    public class InvokingMethodOperation : BuildOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvokingMethodOperation"/> class.
        /// </summary>
        public InvokingMethodOperation(Type typeBeingConstructed, string methodSignature)
            : base(typeBeingConstructed)
        {
            MethodSignature = methodSignature;
        }

        /// <summary>
        /// Method we're trying to call.
        /// </summary>
        public string MethodSignature { get; }

        /// <summary>
        /// Generate the description string.
        /// </summary>
        /// <returns>The string.</returns>
        public override string ToString()
        {
            return $"{Constants.InvokingMethodOperation}{TypeBeingConstructed.GetTypeInfo().Name}{MethodSignature}";
        }
    }
}
