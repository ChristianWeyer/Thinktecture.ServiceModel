/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Thinktecture.ServiceModel.Description
{
    /// <summary>
    /// 
    /// </summary>
    public class InheritableDataContractSerializer : XmlObjectSerializer
    {
        DataContractSerializer dcs;

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritableDataContractSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public InheritableDataContractSerializer(Type type)
        {
            dcs = new DataContractSerializer(type);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritableDataContractSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="knownTypes">The known types.</param>
        public InheritableDataContractSerializer(Type type, IEnumerable<Type> knownTypes)
        {
            dcs = new DataContractSerializer(type, knownTypes);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritableDataContractSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="rootName">Name of the root.</param>
        /// <param name="rootNamespace">The root namespace.</param>
        public InheritableDataContractSerializer(Type type, string rootName, string rootNamespace)
        {
            dcs = new DataContractSerializer(type, rootName, rootNamespace);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritableDataContractSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="rootName">Name of the root.</param>
        /// <param name="rootNamespace">The root namespace.</param>
        public InheritableDataContractSerializer(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace)
        {
            dcs = new DataContractSerializer(type, rootName, rootNamespace);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritableDataContractSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="rootName">Name of the root.</param>
        /// <param name="rootNamespace">The root namespace.</param>
        /// <param name="knownTypes">The known types.</param>
        public InheritableDataContractSerializer(Type type, string rootName, string rootNamespace, IEnumerable<Type> knownTypes)
        {
            dcs = new DataContractSerializer(type, rootName, rootNamespace, knownTypes);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritableDataContractSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="rootName">Name of the root.</param>
        /// <param name="rootNamespace">The root namespace.</param>
        /// <param name="knownTypes">The known types.</param>
        public InheritableDataContractSerializer(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace, IEnumerable<Type> knownTypes)
        {
            dcs = new DataContractSerializer(type, rootName, rootNamespace, knownTypes);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritableDataContractSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <param name="maxItemsInObjectGraph">The max items in object graph.</param>
        /// <param name="ignoreExtensionDataObject">if set to <c>true</c> [ignore extension data object].</param>
        /// <param name="preserveObjectReferences">if set to <c>true</c> [preserve object references].</param>
        /// <param name="dataContractSurrogate">The data contract surrogate.</param>
        public InheritableDataContractSerializer(Type type, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate)
        {
            dcs = new DataContractSerializer(type, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritableDataContractSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="rootName">Name of the root.</param>
        /// <param name="rootNamespace">The root namespace.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <param name="maxItemsInObjectGraph">The max items in object graph.</param>
        /// <param name="ignoreExtensionDataObject">if set to <c>true</c> [ignore extension data object].</param>
        /// <param name="preserveObjectReferences">if set to <c>true</c> [preserve object references].</param>
        /// <param name="dataContractSurrogate">The data contract surrogate.</param>
        public InheritableDataContractSerializer(Type type, string rootName, string rootNamespace, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate)
        {
            dcs = new DataContractSerializer(type, rootName, rootNamespace, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InheritableDataContractSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="rootName">Name of the root.</param>
        /// <param name="rootNamespace">The root namespace.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <param name="maxItemsInObjectGraph">The max items in object graph.</param>
        /// <param name="ignoreExtensionDataObject">if set to <c>true</c> [ignore extension data object].</param>
        /// <param name="preserveObjectReferences">if set to <c>true</c> [preserve object references].</param>
        /// <param name="dataContractSurrogate">The data contract surrogate.</param>
        public InheritableDataContractSerializer(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate)
        {
            dcs = new DataContractSerializer(type, rootName, rootNamespace, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate);
        }

        /// <summary>
        /// Gets a value that specifies whether the <see cref="T:System.Xml.XmlDictionaryReader"/> is positioned over an XML element that can be read.
        /// </summary>
        /// <param name="reader">An <see cref="T:System.Xml.XmlDictionaryReader"/> used to read the XML stream or document.</param>
        /// <returns>
        /// true if the reader can read the data; otherwise, false.
        /// </returns>
        public override bool IsStartObject(XmlDictionaryReader reader)
        {
            return dcs.IsStartObject(reader);
        }

        /// <summary>
        /// Reads the XML stream or document with an <see cref="T:System.Xml.XmlDictionaryReader"/> and returns the deserialized object; it also enables you to specify whether the serializer can read the data before attempting to read it.
        /// </summary>
        /// <param name="reader">An <see cref="T:System.Xml.XmlDictionaryReader"/> used to read the XML document.</param>
        /// <param name="verifyObjectName">true to check whether the enclosing XML element name and namespace correspond to the root name and root namespace; otherwise, false to skip the verification.</param>
        /// <returns>The deserialized object.</returns>
        public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
        {
            return dcs.ReadObject(reader, verifyObjectName);
        }

        /// <summary>
        /// Writes the end of the object data as a closing XML element to the XML document or stream with an <see cref="T:System.Xml.XmlDictionaryWriter"/>.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> used to write the XML document or stream.</param>
        /// <exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception>
        /// <exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception>
        public override void WriteEndObject(XmlDictionaryWriter writer)
        {
            dcs.WriteEndObject(writer);
        }

        /// <summary>
        /// Writes only the content of the object to the XML document or stream using the specified <see cref="T:System.Xml.XmlDictionaryWriter"/>.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> used to write the XML document or stream.</param>
        /// <param name="graph">The object that contains the content to write.</param>
        /// <exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception>
        /// <exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception>
        public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
        {
            dcs.WriteObjectContent(writer, graph);
        }

        /// <summary>
        /// Writes the start of the object's data as an opening XML element using the specified <see cref="T:System.Xml.XmlDictionaryWriter"/>.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> used to write the XML document.</param>
        /// <param name="graph">The object to serialize.</param>
        /// <exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception>
        /// <exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception>
        public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
        {
            dcs.WriteStartObject(writer, graph);
        }

        /// <summary>
        /// Gets a value that specifies whether the <see cref="T:System.Xml.XmlReader"/> is positioned over an XML element that can be read.
        /// </summary>
        /// <param name="reader">An <see cref="T:System.Xml.XmlReader"/> used to read the XML stream or document.</param>
        /// <returns>
        /// true if the reader is positioned over the starting element; otherwise, false.
        /// </returns>
        public override bool IsStartObject(XmlReader reader)
        {
            return dcs.IsStartObject(reader);
        }

        /// <summary>
        /// Reads the XML stream or document with a <see cref="T:System.IO.Stream"/> and returns the deserialized object.
        /// </summary>
        /// <param name="stream">A <see cref="T:System.IO.Stream"/> used to read the XML stream or document.</param>
        /// <returns>The deserialized object.</returns>
        public override object ReadObject(Stream stream)
        {
            return base.ReadObject(stream);
        }

        /// <summary>
        /// Reads the XML document or stream with an <see cref="T:System.Xml.XmlDictionaryReader"/> and returns the deserialized object.
        /// </summary>
        /// <param name="reader">An <see cref="T:System.Xml.XmlDictionaryReader"/> used to read the XML document.</param>
        /// <returns>The deserialized object.</returns>
        public override object ReadObject(XmlDictionaryReader reader)
        {
            return base.ReadObject(reader);
        }

        /// <summary>
        /// Reads the XML document or stream with an <see cref="T:System.Xml.XmlReader"/> and returns the deserialized object.
        /// </summary>
        /// <param name="reader">An <see cref="T:System.Xml.XmlReader"/> used to read the XML stream or document.</param>
        /// <returns>The deserialized object.</returns>
        public override object ReadObject(XmlReader reader)
        {
            return dcs.ReadObject(reader);
        }

        /// <summary>
        /// Reads the XML document or stream with an <see cref="T:System.Xml.XmlReader"/> and returns the deserialized object; it also enables you to specify whether the serializer can read the data before attempting to read it.
        /// </summary>
        /// <param name="reader">An <see cref="T:System.Xml.XmlReader"/> used to read the XML document or stream.</param>
        /// <param name="verifyObjectName">true to check whether the enclosing XML element name and namespace correspond to the root name and root namespace; false to skip the verification.</param>
        /// <returns>The deserialized object.</returns>
        public override object ReadObject(XmlReader reader, bool verifyObjectName)
        {
            return dcs.ReadObject(reader, verifyObjectName);
        }

        /// <summary>
        /// Writes the end of the object data as a closing XML element to the XML document or stream with an <see cref="T:System.Xml.XmlWriter"/>.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Xml.XmlWriter"/> used to write the XML document or stream.</param>
        /// <exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception>
        /// <exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception>
        public override void WriteEndObject(XmlWriter writer)
        {
            dcs.WriteEndObject(writer);
        }

        /// <summary>
        /// Writes the complete content (start, content, and end) of the object to the XML document or stream with the specified <see cref="T:System.IO.Stream"/>.
        /// </summary>
        /// <param name="stream">A <see cref="T:System.IO.Stream"/> used to write the XML document or stream.</param>
        /// <param name="graph">The object that contains the data to write to the stream.</param>
        /// <exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception>
        /// <exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception>
        public override void WriteObject(Stream stream, object graph)
        {
            base.WriteObject(stream, graph);
        }

        /// <summary>
        /// Writes the complete content (start, content, and end) of the object to the XML document or stream with the specified <see cref="T:System.Xml.XmlDictionaryWriter"/>.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter"/> used to write the content to the XML document or stream.</param>
        /// <param name="graph">The object that contains the content to write.</param>
        /// <exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception>
        /// <exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception>
        public override void WriteObject(XmlDictionaryWriter writer, object graph)
        {
            base.WriteObject(writer, graph);
        }

        /// <summary>
        /// Writes the complete content (start, content, and end) of the object to the XML document or stream with the specified <see cref="T:System.Xml.XmlWriter"/>.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Xml.XmlWriter"/> used to write the XML document or stream.</param>
        /// <param name="graph">The object that contains the content to write.</param>
        /// <exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception>
        /// <exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception>
        public override void WriteObject(XmlWriter writer, object graph)
        {
            base.WriteObject(writer, graph);
        }

        /// <summary>
        /// Writes only the content of the object to the XML document or stream with the specified <see cref="T:System.Xml.XmlWriter"/>.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Xml.XmlWriter"/> used to write the XML document or stream.</param>
        /// <param name="graph">The object that contains the content to write.</param>
        /// <exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception>
        /// <exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception>
        public override void WriteObjectContent(XmlWriter writer, object graph)
        {
            dcs.WriteObjectContent(writer, graph);
        }

        /// <summary>
        /// Writes the start of the object's data as an opening XML element using the specified <see cref="T:System.Xml.XmlWriter"/>.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Xml.XmlWriter"/> used to write the XML document.</param>
        /// <param name="graph">The object to serialize.</param>
        /// <exception cref="T:System.Runtime.Serialization.InvalidDataContractException">the type being serialized does not conform to data contract rules. For example, the <see cref="T:System.Runtime.Serialization.DataContractAttribute"/> attribute has not been applied to the type.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">there is a problem with the instance being serialized.</exception>
        /// <exception cref="T:System.ServiceModel.QuotaExceededException">the maximum number of objects to serialize has been exceeded. Check the <see cref="P:System.Runtime.Serialization.DataContractSerializer.MaxItemsInObjectGraph"/> property.</exception>
        public override void WriteStartObject(XmlWriter writer, object graph)
        {
            dcs.WriteStartObject(writer, graph);
        }
    }
}
