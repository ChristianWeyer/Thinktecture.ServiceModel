/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;

namespace Thinktecture.ServiceModel.Description
{
    /// <summary>
    /// 
    /// </summary>
    public class PrefixDataContractSerializer : InheritableDataContractSerializer
    {
        Dictionary<string, string> registeredNamespaces = null;
        bool namespacesRegistered = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixDataContractSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public PrefixDataContractSerializer(Type type) : base(type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixDataContractSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="knownTypes">The known types.</param>
        public PrefixDataContractSerializer(Type type, IEnumerable<Type> knownTypes) :
            base(type, knownTypes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixDataContractSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="rootName">Name of the root.</param>
        /// <param name="rootNamespace">The root namespace.</param>
        public PrefixDataContractSerializer(Type type, string rootName, string rootNamespace) :
            base(type, rootName, rootNamespace)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixDataContractSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="rootName">Name of the root.</param>
        /// <param name="rootNamespace">The root namespace.</param>
        public PrefixDataContractSerializer(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace) :
            base(type, rootName, rootNamespace)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixDataContractSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="rootName">Name of the root.</param>
        /// <param name="rootNamespace">The root namespace.</param>
        /// <param name="knownTypes">The known types.</param>
        public PrefixDataContractSerializer(Type type, string rootName, string rootNamespace, IEnumerable<Type> knownTypes) :
            base(type, rootName, rootNamespace, knownTypes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixDataContractSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="rootName">Name of the root.</param>
        /// <param name="rootNamespace">The root namespace.</param>
        /// <param name="knownTypes">The known types.</param>
        public PrefixDataContractSerializer(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace, IEnumerable<Type> knownTypes) :
            base(type, rootName, rootNamespace, knownTypes)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixDataContractSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <param name="maxItemsInObjectGraph">The max items in object graph.</param>
        /// <param name="ignoreExtensionDataObject">if set to <c>true</c> [ignore extension data object].</param>
        /// <param name="preserveObjectReferences">if set to <c>true</c> [preserve object references].</param>
        /// <param name="dataContractSurrogate">The data contract surrogate.</param>
        public PrefixDataContractSerializer(Type type, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate) :
            base(type, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixDataContractSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="rootName">Name of the root.</param>
        /// <param name="rootNamespace">The root namespace.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <param name="maxItemsInObjectGraph">The max items in object graph.</param>
        /// <param name="ignoreExtensionDataObject">if set to <c>true</c> [ignore extension data object].</param>
        /// <param name="preserveObjectReferences">if set to <c>true</c> [preserve object references].</param>
        /// <param name="dataContractSurrogate">The data contract surrogate.</param>
        public PrefixDataContractSerializer(Type type, string rootName, string rootNamespace, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate) :
            base(type, rootName, rootNamespace, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixDataContractSerializer"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="rootName">Name of the root.</param>
        /// <param name="rootNamespace">The root namespace.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <param name="maxItemsInObjectGraph">The max items in object graph.</param>
        /// <param name="ignoreExtensionDataObject">if set to <c>true</c> [ignore extension data object].</param>
        /// <param name="preserveObjectReferences">if set to <c>true</c> [preserve object references].</param>
        /// <param name="dataContractSurrogate">The data contract surrogate.</param>
        public PrefixDataContractSerializer(Type type, XmlDictionaryString rootName, XmlDictionaryString rootNamespace, IEnumerable<Type> knownTypes, int maxItemsInObjectGraph, bool ignoreExtensionDataObject, bool preserveObjectReferences, IDataContractSurrogate dataContractSurrogate) :
            base(type, rootName, rootNamespace, knownTypes, maxItemsInObjectGraph, ignoreExtensionDataObject, preserveObjectReferences, dataContractSurrogate)
        {
        }

        /// <summary>
        /// Registers the namespace.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="xmlNamespace">The XML namespace.</param>
        public void RegisterNamespace(string prefix, string xmlNamespace)
        {
            if (registeredNamespaces == null)
            {
                registeredNamespaces = new Dictionary<string, string>();
            }

            registeredNamespaces.Add(prefix, xmlNamespace);
        }

        /// <summary>
        /// Writes the object.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="graph">The graph.</param>
        public override void WriteObject(XmlWriter writer, object graph)
        {
            base.WriteObject(writer, graph);
        }

        /// <summary>
        /// Writes the object.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="graph">The graph.</param>
        public override void WriteObject(System.IO.Stream stream, object graph)
        {
            base.WriteObject(stream, graph);
        }

        /// <summary>
        /// Writes the object.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="graph">The graph.</param>
        public override void WriteObject(XmlDictionaryWriter writer, object graph)
        {
            namespacesRegistered = registeredNamespaces == null;
            
            base.WriteObject(writer, graph);
        }

        private void EmitNamespaces(XmlWriter writer)
        {
            foreach (string key in registeredNamespaces.Keys)
            {
                writer.WriteAttributeString("xmlns", key, null, registeredNamespaces[key]);
            }

            namespacesRegistered = true;
        }

        private void EmitNamespaces(XmlReader reader)
        {
            XmlNamespaceManager mgr = new XmlNamespaceManager(reader.NameTable);

            foreach (string key in registeredNamespaces.Keys)
            {
                string ns = registeredNamespaces[key];

                if (string.IsNullOrEmpty(mgr.LookupNamespace(ns)))
                {
                    mgr.AddNamespace(key, ns);
                }
            }

            namespacesRegistered = true;
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
            
            if (!namespacesRegistered)
            {
                EmitNamespaces(writer);
            }

            base.WriteStartObject(writer, graph);

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

            if (!namespacesRegistered)
            {
                EmitNamespaces(writer);
            }

            base.WriteStartObject(writer, graph);

        }

        /// <summary>
        /// Reads the XML document or stream with an <see cref="T:System.Xml.XmlDictionaryReader"/> and returns the deserialized object.
        /// </summary>
        /// <param name="reader">An <see cref="T:System.Xml.XmlDictionaryReader"/> used to read the XML document.</param>
        /// <returns>The deserialized object.</returns>
        public override object ReadObject(XmlDictionaryReader reader)
        {
            if (registeredNamespaces != null)
            {
                EmitNamespaces(reader);
            }

            return base.ReadObject(reader);
        }

        /// <summary>
        /// Reads the XML stream or document with a <see cref="T:System.IO.Stream"/> and returns the deserialized object.
        /// </summary>
        /// <param name="stream">A <see cref="T:System.IO.Stream"/> used to read the XML stream or document.</param>
        /// <returns>The deserialized object.</returns>
        public override object ReadObject(System.IO.Stream stream)
        {
            return this.ReadObject(new XmlTextReader(stream));
        }

        /// <summary>
        /// Reads the XML document or stream with an <see cref="T:System.Xml.XmlReader"/> and returns the deserialized object.
        /// </summary>
        /// <param name="reader">An <see cref="T:System.Xml.XmlReader"/> used to read the XML stream or document.</param>
        /// <returns>The deserialized object.</returns>
        public override object ReadObject(XmlReader reader)
        {
            if (registeredNamespaces != null)
            {
                EmitNamespaces(reader);
            }

            return base.ReadObject(reader);
        }

        /// <summary>
        /// Reads the XML stream or document with an <see cref="T:System.Xml.XmlDictionaryReader"/> and returns the deserialized object; it also enables you to specify whether the serializer can read the data before attempting to read it.
        /// </summary>
        /// <param name="reader">An <see cref="T:System.Xml.XmlDictionaryReader"/> used to read the XML document.</param>
        /// <param name="verifyObjectName">true to check whether the enclosing XML element name and namespace correspond to the root name and root namespace; otherwise, false to skip the verification.</param>
        /// <returns>The deserialized object.</returns>
        public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
        {
            if (registeredNamespaces != null)
            {
                EmitNamespaces(reader);
            }

            return base.ReadObject(reader, verifyObjectName);
        }

        /// <summary>
        /// Reads the XML document or stream with an <see cref="T:System.Xml.XmlReader"/> and returns the deserialized object; it also enables you to specify whether the serializer can read the data before attempting to read it.
        /// </summary>
        /// <param name="reader">An <see cref="T:System.Xml.XmlReader"/> used to read the XML document or stream.</param>
        /// <param name="verifyObjectName">true to check whether the enclosing XML element name and namespace correspond to the root name and root namespace; false to skip the verification.</param>
        /// <returns>The deserialized object.</returns>
        public override object ReadObject(XmlReader reader, bool verifyObjectName)
        {
            if (registeredNamespaces != null)
            {
                EmitNamespaces(reader);
            }

            return base.ReadObject(reader, verifyObjectName);
        }
    }
}
