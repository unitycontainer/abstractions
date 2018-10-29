﻿// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reflection;

namespace Unity.Builder.Operation
{
    /// <summary>
    /// A base class that holds the information shared by all operations
    /// performed by the container while setting properties.
    /// </summary>
    public abstract class PropertyOperation : BuildOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        protected PropertyOperation(Type typeBeingConstructed, string propertyName)
            : base(typeBeingConstructed)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// The property value currently being resolved.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Generate the description of this operation.
        /// </summary>
        /// <returns>The string.</returns>
        public override string ToString()
        {
            return $"{GetDescriptionFormat()}{TypeBeingConstructed.GetTypeInfo().Name}{PropertyName}";
        }

        /// <summary>
        /// GetOrDefault a format string used to create the description. Called by
        /// the base <see cref='ToString'/> method.
        /// </summary>
        /// <returns>The format string.</returns>
        protected abstract string GetDescriptionFormat();
    }
}
