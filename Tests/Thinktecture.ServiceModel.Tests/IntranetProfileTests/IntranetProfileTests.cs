/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Thinktecture.ServiceModel.Tests
{
    /// <summary>
    /// Contains the unit tests for API enabling the Intranet Profile. 
    /// </summary>
    [TestClass]
    public class IntranetProfileTests
    {
        public IntranetProfileTests()
        {
        }

        /// <summary>
        /// Tests if the intranet profile is properly applied on the service side.
        /// </summary>
        [TestMethod]
        public void TestIntranetProfileOnTheServiceSide()
        {
            // Create an instance of the custom service host that uses the intranet profile.
            ServiceHost host = new ServiceHost(typeof (GeneralTestService), Profile.Intranet, Flattening.Disabled);

            // Alert if the intranet profile is not applied in the ServiceHost instance.
            Assert.IsTrue(host.Profile == Profile.Intranet, "Intranet profile is not applied.");

            // Start the service.
            host.Open();

            // Find the ServiceBehaviorAttribute.
            ServiceBehaviorAttribute serviceBehavior = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            // Verify the service behavior quotas.
            VerifyServiceBehaviorAttributeSettings(serviceBehavior);

            // Make sure the intranet quotas are applied in all endpoints in the service.
            foreach (ServiceEndpoint ep in host.Description.Endpoints)
            {
                VerifyBindingSettings(ep.Binding);
            }

            // Finally shutdown the service.
            host.Close();
        }

        /// <summary>
        /// Tests if the intranet profile is properly applied on the client side.
        /// </summary>
        [TestMethod]
        public void TestIntranetProfileOnTheClientSide()
        {
            ChannelFactory<IGlobalTestService> cf =
                new ChannelFactory<IGlobalTestService>(
                    "IGlobalTestService_BasicHttpBinding", Profile.Intranet);

            // Check if the intranet profile is not applied in the channel factory instance.
            Assert.IsTrue(cf.Profile == Profile.Intranet, "Intranet profile is not applied.");

            // Make sure the intranet quotas are applied to the ChannelFactory endpoint.
            VerifyBindingSettings(cf.Endpoint.Binding);

            // Finally close the ChannelFactory.
            cf.Close();
        }

        /// <summary>
        /// Verifies that the throttling settings in ServiceBehaviorAttribute are maxed out.
        /// </summary>        
        private void VerifyServiceBehaviorAttributeSettings(ServiceBehaviorAttribute serviceBehavior)
        {
            Assert.IsTrue(serviceBehavior.MaxItemsInObjectGraph == int.MaxValue,
                          "Max items in object graph quota is not maxed out.");
        }

        /// <summary>
        /// Verifies that the throttling settings in a given binding are maxed out.
        /// </summary>
        private void VerifyBindingSettings(Binding binding)
        {
            CustomBinding customBinding = new CustomBinding(binding);
            BindingContext bctx = new BindingContext(customBinding, new BindingParameterCollection());

            // Verify binding timeouts.
            VerifyBindingTimeouts(binding);

            // Verify the XmlDictionaryReaderQuotas.
            XmlDictionaryReaderQuotas dictQuotas = bctx.GetInnerProperty<XmlDictionaryReaderQuotas>();
            VerifyXmlDictionaryReaderQuotas(dictQuotas, binding.Name);

            // Verify the transport.
            TransportBindingElement transport = bctx.RemainingBindingElements.Find<TransportBindingElement>();
            VerifyTransportQuotas(transport, binding.Name);
        }

        /// <summary>
        /// Verifies that the defaulte timeouts in a given binding are maxed out.
        /// </summary>        
        private void VerifyBindingTimeouts(Binding binding)
        {
            Assert.IsTrue(binding.ReceiveTimeout == TimeSpan.MaxValue,
                          "Receive timeout is not maxed out in binding: {0}", binding.Name);
            Assert.IsTrue(binding.SendTimeout == TimeSpan.MaxValue, "Send timeout is not maxed out in binding: {0}",
                          binding.Name);
        }

        /// <summary>
        /// Verifies that the quota values in a given XmlDictionaryReaderQuotas instance are maxed out.
        /// </summary>        
        private void VerifyXmlDictionaryReaderQuotas(XmlDictionaryReaderQuotas dictQuotas, string bindingName)
        {
            if (dictQuotas != null)
            {
                Assert.IsTrue(dictQuotas.MaxArrayLength == int.MaxValue,
                              "Max array length is not maxed out in binding: {0}.", bindingName);
                Assert.IsTrue(dictQuotas.MaxBytesPerRead == int.MaxValue,
                              "Max bytes per read is not maxed out in binding: {0}.", bindingName);
                Assert.IsTrue(dictQuotas.MaxDepth == int.MaxValue, "Max depth is not maxed out in binding: {0}.",
                              bindingName);
                Assert.IsTrue(dictQuotas.MaxNameTableCharCount == int.MaxValue,
                              "Max name table char count is not maxed out in binding: {0}.", bindingName);
                Assert.IsTrue(dictQuotas.MaxStringContentLength == int.MaxValue,
                              "Max string content length is not maxed out in binding: {0}.", bindingName);
            }
        }

        /// <summary>
        /// Verifies that the quota values in a given transport binding element are maxed out.
        /// </summary>        
        private void VerifyTransportQuotas(TransportBindingElement transport, string bindingName)
        {
            if (transport != null)
            {
                if (typeof (HttpTransportBindingElement) == transport.GetType()) // http
                {
                    HttpTransportBindingElement httpTransport = transport as HttpTransportBindingElement;

                    if (httpTransport.TransferMode == TransferMode.Streamed ||
                        httpTransport.TransferMode == TransferMode.StreamedRequest)
                    {
                        Assert.IsTrue(httpTransport.MaxReceivedMessageSize == long.MaxValue,
                                      "Max receive message size is not maxed out in binding: {0}", bindingName);
                    }
                    else
                    {
                        Assert.IsTrue(httpTransport.MaxReceivedMessageSize == int.MaxValue,
                                      "Max receive message size is not maxed out in binding: {0}", bindingName);
                    }
                    Assert.IsTrue(httpTransport.MaxBufferSize == int.MaxValue,
                                  "Max buffer size is not maxed out in binding: {0}", bindingName);
                }
                else if (typeof (TcpTransportBindingElement) == transport.GetType()) // tcp                            
                {
                    TcpTransportBindingElement tcpTransport = transport as TcpTransportBindingElement;

                    if (tcpTransport.TransferMode == TransferMode.Streamed ||
                        tcpTransport.TransferMode == TransferMode.StreamedRequest)
                    {
                        Assert.IsTrue(tcpTransport.MaxReceivedMessageSize == long.MaxValue,
                                      "Max receive message size is not maxed out in binding: {0}", bindingName);
                    }
                    else
                    {
                        Assert.IsTrue(tcpTransport.MaxReceivedMessageSize == int.MaxValue,
                                      "Max receive message size is not maxed out in binding: {0}", bindingName);
                    }
                    Assert.IsTrue(tcpTransport.MaxBufferSize == int.MaxValue,
                                  "Max buffer size is not maxed out in binding: {0}", bindingName);
                }
                else if (typeof (NamedPipeTransportBindingElement) == transport.GetType()) // pipe
                {
                    NamedPipeTransportBindingElement pipeTransport = transport as NamedPipeTransportBindingElement;

                    if (pipeTransport.TransferMode == TransferMode.Streamed ||
                        pipeTransport.TransferMode == TransferMode.StreamedRequest)
                    {
                        Assert.IsTrue(pipeTransport.MaxReceivedMessageSize == long.MaxValue,
                                      "Max receive message size is not maxed out in binding: {0}", bindingName);
                    }
                    else
                    {
                        Assert.IsTrue(pipeTransport.MaxReceivedMessageSize == int.MaxValue,
                                      "Max receive message size is not maxed out in binding: {0}", bindingName);
                    }
                    Assert.IsTrue(pipeTransport.MaxBufferSize == int.MaxValue,
                                  "Max buffer size is not maxed out in binding: {0}", bindingName);
                }
                else if (typeof (MsmqTransportBindingElement) == transport.GetType()) // msmq
                {
                    MsmqTransportBindingElement msmqTransport = transport as MsmqTransportBindingElement;
                    Assert.IsTrue(msmqTransport.MaxReceivedMessageSize == long.MaxValue,
                                  "Max receive message size is not maxed out in binding: {0}", bindingName);
                }
                else
                {
                    // We don't know the transport type (probably a custom transport). So let's max it out to the int.MaxValue.
                    Assert.IsTrue(transport.MaxReceivedMessageSize == int.MaxValue,
                                  "Max receive message size is not maxed out in binding: {0}", bindingName);
                }
            }
        }
    }
}