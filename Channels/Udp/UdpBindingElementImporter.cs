// ----------------------------------------------------------------------------
// Copyright (C) 2003-2005 Microsoft Corporation, All rights reserved.
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;
using WsdlNS = System.Web.Services.Description;

namespace Microsoft.ServiceModel.Samples
{
    /// <summary>
    /// Policy import/export for Udp
    /// </summary>
    public class UdpBindingElementImporter : IPolicyImportExtension, IWsdlImportExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UdpBindingElementImporter"/> class.
        /// </summary>
        public UdpBindingElementImporter()
        {
        }

        void IPolicyImportExtension.ImportPolicy(MetadataImporter importer, PolicyConversionContext context)
        {
            if (importer == null)
            {
                throw new ArgumentNullException("importer");
            }
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            UdpTransportBindingElement udpBindingElement = null;
            bool multicast = false;
            PolicyAssertionCollection policyAssertions = context.GetBindingAssertions();
            if (policyAssertions.Remove(UdpPolicyStrings.TransportAssertion, UdpPolicyStrings.UdpNamespace) != null)
            {
                udpBindingElement = new UdpTransportBindingElement();
            }
            if (policyAssertions.Remove(UdpPolicyStrings.MulticastAssertion, UdpPolicyStrings.UdpNamespace) != null)
            {
                multicast = true;
            }
            if (udpBindingElement != null)
            {
                udpBindingElement.Multicast = multicast;
                context.BindingElements.Add(udpBindingElement);
            }
        }

        /// <summary>
        /// Called prior to importing metadata documents.
        /// </summary>
        /// <param name="wsdlDocuments">The service descriptions to modify.</param>
        /// <param name="xmlSchemas">The schema collection to be used when importing.</param>
        /// <param name="policy">The policy assertions used when importing.</param>
        public void BeforeImport(WsdlNS.ServiceDescriptionCollection wsdlDocuments,
                                 System.Xml.Schema.XmlSchemaSet xmlSchemas, ICollection<XmlElement> policy)
        { }

        /// <summary>
        /// Called when importing a contract.
        /// </summary>
        /// <param name="importer">The importer.</param>
        /// <param name="context">The import context to be modified.</param>
        public void ImportContract(WsdlImporter importer, WsdlContractConversionContext context)
        { }

        /// <summary>
        /// Called when importing an endpoint.
        /// </summary>
        /// <param name="importer">The importer.</param>
        /// <param name="context">The import context to modify.</param>
        public void ImportEndpoint(WsdlImporter importer, WsdlEndpointConversionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (context.Endpoint.Binding == null)
            {
                throw new ArgumentNullException("context.Endpoint.Binding");
            }

            BindingElementCollection bindingElements = context.Endpoint.Binding.CreateBindingElements();
            TransportBindingElement transportBindingElement = bindingElements.Find<TransportBindingElement>();
            if (transportBindingElement is UdpTransportBindingElement)
            {
                ImportAddress(context);
            }            
        }

        //this imports the address of the endpoint.
        void ImportAddress(WsdlEndpointConversionContext context)
        {
            EndpointAddress address = null;

            if (context.WsdlPort != null)
            {
                XmlElement addressing10Element =
                    context.WsdlPort.Extensions.Find("EndpointReference", AddressingVersionConstants.WSAddressing10NameSpace);

                XmlElement addressing200408Element =
                    context.WsdlPort.Extensions.Find("EndpointReference", AddressingVersionConstants.WSAddressingAugust2004NameSpace);

                WsdlNS.SoapAddressBinding soapAddressBinding = 
                    (WsdlNS.SoapAddressBinding) context.WsdlPort.Extensions.Find(typeof(WsdlNS.SoapAddressBinding));

                if (addressing10Element != null)
                {
                    address = EndpointAddress.ReadFrom(AddressingVersion.WSAddressing10,
                                                       new XmlNodeReader(addressing10Element));
                }
                if (addressing200408Element != null)
                {
                    address = EndpointAddress.ReadFrom(AddressingVersion.WSAddressingAugust2004,
                                                       new XmlNodeReader(addressing200408Element));
                }
                else if (soapAddressBinding != null)
                {
                    // checking for soapAddressBinding checks for both Soap 1.1 and Soap 1.2
                    address = new EndpointAddress(soapAddressBinding.Location);
                }
            }
            
            if (address != null)
            {
                context.Endpoint.Address = address;
            }
        }
    }
}
