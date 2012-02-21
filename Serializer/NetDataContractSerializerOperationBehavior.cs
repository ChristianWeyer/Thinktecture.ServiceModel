/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.ServiceModel.Description;
using System.Xml;

namespace Thinktecture.ServiceModel.Description
{
    /// <summary>
    /// Represents the runtime behavior of the <see cref="NetDataContractSerializer"/>.
    /// </summary>
    public class NetDataContractSerializerOperationBehavior :
        DataContractSerializerOperationBehavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetDataContractSerializerOperationBehavior"/> class.
        /// </summary>        
        public NetDataContractSerializerOperationBehavior(
            OperationDescription operationDescription)
            : base(operationDescription)
        {
        }

        /// <summary>
        /// Creates an instance of a class that inherits from <see cref="T:System.Runtime.Serialization.XmlObjectSerializer"></see> for serialization and deserialization operations.
        /// </summary>
        /// <returns>
        /// An instance of a class that inherits from the <see cref="T:System.Runtime.Serialization.XmlObjectSerializer"></see> class.
        /// </returns>
        public override XmlObjectSerializer CreateSerializer(
            Type type, string name, string ns, IList<Type> knownTypes)
        {
            return new NetDataContractSerializer(
                name, ns, new StreamingContext(),
                MaxItemsInObjectGraph, IgnoreExtensionDataObject,
                FormatterAssemblyStyle.Full, null);
        }

        /// <summary>
        /// Creates an instance of a class that inherits from <see cref="T:System.Runtime.Serialization.XmlObjectSerializer"></see> for serialization and deserialization operations with an <see cref="T:System.Xml.XmlDictionaryString"></see> that contains the namespace.
        /// </summary>
        /// <returns>
        /// An instance of a class that inherits from the <see cref="T:System.Runtime.Serialization.XmlObjectSerializer"></see> class.
        /// </returns>
        public override XmlObjectSerializer CreateSerializer(Type type, XmlDictionaryString name, XmlDictionaryString ns,
                                                             IList<Type> knownTypes)
        {
            return new NetDataContractSerializer(
                name, ns, new StreamingContext(),
                MaxItemsInObjectGraph, IgnoreExtensionDataObject,
                FormatterAssemblyStyle.Full, null);
        }
    }
}