/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel.Description;
using System.Xml;

namespace Thinktecture.ServiceModel.Description
{
    /// <summary>
    /// 
    /// </summary>
    public class PrefixContractSerializerOperationBehavior :
        DataContractSerializerOperationBehavior
    {
        private RegisterNamespacePrefixAttribute[] prefixAttributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixContractSerializerOperationBehavior"/> class.
        /// </summary>
        /// <param name="operationDescription">The operation description.</param>
        /// <param name="attributes">The attributes.</param>
        public PrefixContractSerializerOperationBehavior(OperationDescription operationDescription, RegisterNamespacePrefixAttribute[] attributes)
            : base(operationDescription)
        {
            prefixAttributes = attributes;            
        }

        /// <summary>
        /// Creates an instance of a class that inherits from <see cref="T:System.Runtime.Serialization.XmlObjectSerializer"/> for serialization and deserialization operations.
        /// </summary>
        /// <param name="type">The <see cref="T:System.Type"/> to create the serializer for.</param>
        /// <param name="name">The name of the generated type.</param>
        /// <param name="ns">The namespace of the generated type.</param>
        /// <param name="knownTypes">An <see cref="T:System.Collections.Generic.IList`1"/> of <see cref="T:System.Type"/> that contains known types.</param>
        /// <returns>
        /// An instance of a class that inherits from the <see cref="T:System.Runtime.Serialization.XmlObjectSerializer"/> class.
        /// </returns>
        public override XmlObjectSerializer CreateSerializer(
            Type type, string name, string ns, IList<Type> knownTypes)
        {
            var serializer = new PrefixDataContractSerializer(
                type,
                name,
                ns,
                knownTypes,                 
                MaxItemsInObjectGraph, 
                IgnoreExtensionDataObject, 
                false,
                DataContractSurrogate);

            AddPrefixes(serializer);

            return serializer;
        }

        /// <summary>
        /// Creates an instance of a class that inherits from <see cref="T:System.Runtime.Serialization.XmlObjectSerializer"/> for serialization and deserialization operations with an <see cref="T:System.Xml.XmlDictionaryString"/> that contains the namespace.
        /// </summary>
        /// <param name="type">The type to serialize or deserialize.</param>
        /// <param name="name">The name of the serialized type.</param>
        /// <param name="ns">An <see cref="T:System.Xml.XmlDictionaryString"/> that contains the namespace of the serialized type.</param>
        /// <param name="knownTypes">An <see cref="T:System.Collections.Generic.IList`1"/> of <see cref="T:System.Type"/> that contains known types.</param>
        /// <returns>
        /// An instance of a class that inherits from the <see cref="T:System.Runtime.Serialization.XmlObjectSerializer"/> class.
        /// </returns>
        public override XmlObjectSerializer CreateSerializer(
            Type type, XmlDictionaryString name, XmlDictionaryString ns, IList<Type> knownTypes)
        {
            var serializer = new PrefixDataContractSerializer(
                type,
                name,
                ns,
                knownTypes,
                MaxItemsInObjectGraph,
                IgnoreExtensionDataObject,
                false,
                DataContractSurrogate);

            AddPrefixes(serializer);

            return serializer;
        }

        private void AddPrefixes(PrefixDataContractSerializer serializer)
        {
            foreach (RegisterNamespacePrefixAttribute attr in prefixAttributes)
            {
                serializer.RegisterNamespace(attr.Prefix, attr.XmlNamespace);
            }
        }
    }
}