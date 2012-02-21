/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace Thinktecture.ServiceModel
{
    /// <summary>
    /// Contains the helper methods to tweak the bindings for intranet hosting scenarios.
    /// </summary>
    internal static class BindingController
    {
        /// <summary>
        /// Increases the quotas in the specified binding.
        /// </summary>        
        public static Binding IncreaseBindingQuotas(Binding binding)
        {
            CustomBinding newBinding = new CustomBinding(binding);
            newBinding.ReceiveTimeout = TimeSpan.MaxValue;
            newBinding.SendTimeout = TimeSpan.MaxValue;

            BindingContext bcontext = new BindingContext(newBinding, new BindingParameterCollection());
            XmlDictionaryReaderQuotas xmlDictReaderQuotas = bcontext.GetInnerProperty<XmlDictionaryReaderQuotas>();

            if (xmlDictReaderQuotas != null)
            {
                xmlDictReaderQuotas.MaxArrayLength = int.MaxValue;
                xmlDictReaderQuotas.MaxBytesPerRead = int.MaxValue;
                xmlDictReaderQuotas.MaxDepth = int.MaxValue;
                xmlDictReaderQuotas.MaxNameTableCharCount = int.MaxValue;
                xmlDictReaderQuotas.MaxStringContentLength = int.MaxValue;
            }

            TransportBindingElement transport = bcontext.RemainingBindingElements.Find<TransportBindingElement>();

            if (transport != null)
            {
                if (typeof (HttpTransportBindingElement) == transport.GetType()) // http
                {
                    HttpTransportBindingElement httpTransport = transport as HttpTransportBindingElement;

                    // Are we on a streaming transport? Then we can make MaxReceivedMessageSize to 
                    // long.MaxValue. Otherwise we have to set it to int.MaxValue as that's the max buffer size.
                    if (httpTransport.TransferMode == TransferMode.Streamed ||
                        httpTransport.TransferMode == TransferMode.StreamedRequest)
                    {
                        httpTransport.MaxReceivedMessageSize = long.MaxValue;
                    }
                    else
                    {
                        httpTransport.MaxReceivedMessageSize = int.MaxValue;
                    }
                    httpTransport.MaxBufferSize = int.MaxValue;
                }
                else if (typeof (TcpTransportBindingElement) == transport.GetType()) // tcp                            
                {
                    TcpTransportBindingElement tcpTransport = transport as TcpTransportBindingElement;

                    if (tcpTransport.TransferMode == TransferMode.Streamed ||
                        tcpTransport.TransferMode == TransferMode.StreamedRequest)
                    {
                        tcpTransport.MaxReceivedMessageSize = long.MaxValue;
                    }
                    else
                    {
                        tcpTransport.MaxReceivedMessageSize = int.MaxValue;
                    }
                    tcpTransport.MaxBufferSize = int.MaxValue;
                }
                else if (typeof (NamedPipeTransportBindingElement) == transport.GetType()) // pipe
                {
                    NamedPipeTransportBindingElement pipeTransport = transport as NamedPipeTransportBindingElement;

                    if (pipeTransport.TransferMode == TransferMode.Streamed ||
                        pipeTransport.TransferMode == TransferMode.StreamedRequest)
                    {
                        pipeTransport.MaxReceivedMessageSize = long.MaxValue;
                    }
                    else
                    {
                        pipeTransport.MaxReceivedMessageSize = int.MaxValue;
                    }
                    pipeTransport.MaxBufferSize = int.MaxValue;
                }
                else if (typeof (MsmqTransportBindingElement) == transport.GetType()) // msmq
                {
                    MsmqTransportBindingElement msmqTrasport = transport as MsmqTransportBindingElement;
                    msmqTrasport.MaxReceivedMessageSize = long.MaxValue;
                }
                else
                {
                    // We don't know the transport type (probably a custom transport). So let's max it out to the int.MaxValue.
                    transport.MaxReceivedMessageSize = int.MaxValue;
                }
            }

            return newBinding;
        }
    }
}