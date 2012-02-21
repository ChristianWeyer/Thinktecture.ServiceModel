// ----------------------------------------------------------------------------
// Copyright (C) 2003-2005 Microsoft Corporation, All rights reserved.
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;
using WsdlNS = System.Web.Services.Description;

namespace Microsoft.ServiceModel.Samples
{
    /// <summary>
    /// Udp Binding Element.  
    /// Used to configure and construct Udp ChannelFactories and ChannelListeners.
    /// </summary>
    public class UdpTransportBindingElement 
        : TransportBindingElement // to signal that we're a transport
        , IPolicyExportExtension // for policy export
        , IWsdlExportExtension
    {
        bool multicast;
        static XmlDocument xmlDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpTransportBindingElement"/> class.
        /// </summary>
        public UdpTransportBindingElement()
        {
            this.multicast = UdpDefaults.Multicast;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpTransportBindingElement"/> class.
        /// </summary>
        /// <param name="other">The other.</param>
        protected UdpTransportBindingElement(UdpTransportBindingElement other)
            : base(other)
        {
            this.multicast = other.multicast;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UdpTransportBindingElement"/> is multicast.
        /// </summary>
        /// <value><c>true</c> if multicast; otherwise, <c>false</c>.</value>
        public bool Multicast
        {
            get
            {
                return this.multicast;
            }

            set
            {
                this.multicast = value;
            }
        }

        /// <summary>
        /// Builds the channel factory.
        /// </summary>
        /// <typeparam name="TChannel">The type of the channel.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return (IChannelFactory<TChannel>)(object)new UdpChannelFactory(this, context);
        }

        /// <summary>
        /// Builds the channel listener.
        /// </summary>
        /// <typeparam name="TChannel">The type of the channel.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (!this.CanBuildChannelListener<TChannel>(context))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Unsupported channel type: {0}.", typeof(TChannel).Name));
            }

            return (IChannelListener<TChannel>)(object)new UdpChannelListener(this, context);
        }

        /// <summary>
        /// Used by higher layers to determine what types of channel factories this
        /// binding element supports. Which in this case is just IOutputChannel.
        /// </summary>
        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            return (typeof(TChannel) == typeof(IOutputChannel));
        }

        /// <summary>
        /// Used by higher layers to determine what types of channel listeners this
        /// binding element supports. Which in this case is just IInputChannel.
        /// </summary>
        public override bool CanBuildChannelListener<TChannel>(BindingContext context)
        {
            return (typeof(TChannel) == typeof(IInputChannel));
        }

        /// <summary>
        /// Gets the URI scheme for the transport.
        /// </summary>
        /// <value></value>
        /// <returns>Returns the URI scheme for the transport, which varies depending on what derived class implements this method.</returns>
        public override string Scheme
        {
            get
            {
                return UdpConstants.Scheme;
            }
        }

        // We expose in policy the fact that we're UDP, and whether we're multicast or not.
        // Import is done through UdpBindingElementImporter.
        void IPolicyExportExtension.ExportPolicy(MetadataExporter exporter, PolicyConversionContext context)
        {
            if (exporter == null)
            {
                throw new ArgumentNullException("exporter");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            ICollection<XmlElement> bindingAssertions = context.GetBindingAssertions();
            XmlDocument xmlDocument = new XmlDocument();
            bindingAssertions.Add(xmlDocument.CreateElement(
                UdpPolicyStrings.Prefix, UdpPolicyStrings.TransportAssertion, UdpPolicyStrings.UdpNamespace));

            if (Multicast)
            {
                bindingAssertions.Add(xmlDocument.CreateElement(
                    UdpPolicyStrings.Prefix, UdpPolicyStrings.MulticastAssertion, UdpPolicyStrings.UdpNamespace));
            }

            bool createdNew = false;
            MessageEncodingBindingElement encodingBindingElement = context.BindingElements.Find<MessageEncodingBindingElement>();
            
            if (encodingBindingElement == null)
            {
                createdNew = true;
                encodingBindingElement = new TextMessageEncodingBindingElement();
            }

            if (createdNew && encodingBindingElement is IPolicyExportExtension)
            {
                ((IPolicyExportExtension)encodingBindingElement).ExportPolicy(exporter, context);
            }

            AddWSAddressingAssertion(context, encodingBindingElement.MessageVersion.Addressing);
        }

        /// <summary>
        /// When overridden in a derived class, returns a copy of the binding element object.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.Channels.BindingElement"/> object that is a deep clone of the original.
        /// </returns>
        public override BindingElement Clone()
        {
            return new UdpTransportBindingElement(this);
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override T GetProperty<T>(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return context.GetInnerProperty<T>();
        }

        /// <summary>
        /// Writes custom Web Services Description Language (WSDL) elements into the generated WSDL for a contract.
        /// </summary>
        /// <param name="exporter">The <see cref="T:System.ServiceModel.Description.WsdlExporter"/> that exports the contract information.</param>
        /// <param name="context">Provides mappings from exported WSDL elements to the contract description.</param>
        public void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
        {
        }

        /// <summary>
        /// Writes custom Web Services Description Language (WSDL) elements into the generated WSDL for an endpoint.
        /// </summary>
        /// <param name="exporter">The <see cref="T:System.ServiceModel.Description.WsdlExporter"/> that exports the endpoint information.</param>
        /// <param name="context">Provides mappings from exported WSDL elements to the endpoint description.</param>
        public void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {
            BindingElementCollection bindingElements = context.Endpoint.Binding.CreateBindingElements();
            MessageEncodingBindingElement encodingBindingElement = bindingElements.Find<MessageEncodingBindingElement>();
            
            if (encodingBindingElement == null)
            {
                encodingBindingElement = new TextMessageEncodingBindingElement();
            }

            // Set SoapBinding Transport URI
            if (UdpPolicyStrings.UdpNamespace != null)
            {
                WsdlNS.SoapBinding soapBinding = GetSoapBinding(context, exporter);

                if (soapBinding != null)
                {
                    soapBinding.Transport = UdpPolicyStrings.UdpNamespace;
                }
            }

            if (context.WsdlPort != null)
            {
                AddAddressToWsdlPort(context.WsdlPort, context.Endpoint.Address, encodingBindingElement.MessageVersion.Addressing);
            }
        }

        private static void AddAddressToWsdlPort(WsdlNS.Port wsdlPort, EndpointAddress endpointAddress, AddressingVersion addressing)
        {
            if (addressing == AddressingVersion.None)
            {
                return;
            }

            MemoryStream memoryStream = new MemoryStream();
            XmlWriter xmlWriter = XmlWriter.Create(memoryStream);
            xmlWriter.WriteStartElement("temp");

            if (addressing == AddressingVersion.WSAddressing10)
            {
                xmlWriter.WriteAttributeString("xmlns", "wsa10", null, AddressingVersionConstants.WSAddressing10NameSpace);
            }
            else if (addressing == AddressingVersion.WSAddressingAugust2004)
            {
                xmlWriter.WriteAttributeString("xmlns", "wsa", null, AddressingVersionConstants.WSAddressingAugust2004NameSpace);
            }
            else
            {
                throw new InvalidOperationException("This addressing version is not supported:\n" + addressing.ToString());
            }

            endpointAddress.WriteTo(addressing, xmlWriter);
            xmlWriter.WriteEndElement();

            xmlWriter.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);

            XmlReader xmlReader = XmlReader.Create(memoryStream);
            xmlReader.MoveToContent();

            XmlElement endpointReference = (XmlElement)XmlDoc.ReadNode(xmlReader).ChildNodes[0];

            wsdlPort.Extensions.Add(endpointReference);
        }

        static void AddWSAddressingAssertion(PolicyConversionContext context, AddressingVersion addressing)
        {
            XmlElement addressingAssertion = null;

            if (addressing == AddressingVersion.WSAddressing10)
            {
                addressingAssertion = XmlDoc.CreateElement("wsaw", "UsingAddressing", "http://www.w3.org/2006/05/addressing/wsdl");
            }
            else if (addressing == AddressingVersion.WSAddressingAugust2004)
            {
                addressingAssertion = XmlDoc.CreateElement("wsap", "UsingAddressing", AddressingVersionConstants.WSAddressingAugust2004NameSpace + "/policy");
            }
            else if (addressing == AddressingVersion.None)
            {
                // do nothing
                addressingAssertion = null;
            }
            else
            {
                throw new InvalidOperationException("This addressing version is not supported:\n" + addressing.ToString());
            }

            if (addressingAssertion != null)
            {
                context.GetBindingAssertions().Add(addressingAssertion);
            }
        }

        private static WsdlNS.SoapBinding GetSoapBinding(WsdlEndpointConversionContext endpointContext, WsdlExporter exporter)
        {
            EnvelopeVersion envelopeVersion = null;
            WsdlNS.SoapBinding existingSoapBinding = null;
            object versions = null;
            object SoapVersionStateKey = new object();

            //get the soap version state
            if (exporter.State.TryGetValue(SoapVersionStateKey, out versions))
            {
                if (versions != null && ((Dictionary<WsdlNS.Binding, EnvelopeVersion>)versions).ContainsKey(endpointContext.WsdlBinding))
                {
                    envelopeVersion = ((Dictionary<WsdlNS.Binding, EnvelopeVersion>)versions)[endpointContext.WsdlBinding];
                }
            }

            if (envelopeVersion == EnvelopeVersion.None)
            {
                return null;
            }

            //get existing soap binding
            foreach (object o in endpointContext.WsdlBinding.Extensions)
            {
                if (o is WsdlNS.SoapBinding)
                {
                    existingSoapBinding = (WsdlNS.SoapBinding)o;
                }
            }
                
            return existingSoapBinding;
        }

        //reflects the structure of the wsdl
        static XmlDocument XmlDoc
        {
            get
            {
                if (xmlDocument == null)
                {
                    NameTable nameTable = new NameTable();
                    nameTable.Add("Policy");
                    nameTable.Add("All");
                    nameTable.Add("ExactlyOne");
                    nameTable.Add("PolicyURIs");
                    nameTable.Add("Id");
                    nameTable.Add("UsingAddressing");
                    nameTable.Add("UsingAddressing");
                    xmlDocument = new XmlDocument(nameTable);
                }
                return xmlDocument;
            }
        }
    }
}
