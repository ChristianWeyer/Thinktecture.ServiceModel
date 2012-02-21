/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;

namespace Thinktecture.ServiceModel.Description
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = true)]
    public sealed class RegisterNamespacePrefixAttribute : Attribute
    {
        private string nsPrefix;
        private string @namespace;

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        /// <value>The prefix.</value>
        public string Prefix
        {
            get { return nsPrefix; }
        }

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        public string XmlNamespace
        {
            get { return @namespace; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterNamespacePrefixAttribute"/> class.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="xmlNamespace">The XML namespace.</param>
        public RegisterNamespacePrefixAttribute(string prefix, string xmlNamespace)
        {
            nsPrefix = prefix;
            @namespace = xmlNamespace;
        }        
    }
}