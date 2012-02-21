// ----------------------------------------------------------------------------
// Copyright (C) 2003-2005 Microsoft Corporation, All rights reserved.
// ----------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.ServiceModel.Samples
{
    /// <summary>
    /// IChannelFactory implementation for Udp.
    /// 
    /// Supports IOutputChannel only, as Udp is fundamentally
    /// a datagram protocol.
    /// </summary>
    class UdpChannelFactory : ChannelFactoryBase<IOutputChannel>
    {
        #region member_variables
        BufferManager bufferManager;
        MessageEncoderFactory messageEncoderFactory;
        bool multicast;
        #endregion

        internal UdpChannelFactory(UdpTransportBindingElement bindingElement, BindingContext context)
            : base(context.Binding)
        {
            this.multicast = bindingElement.Multicast;
            this.bufferManager = BufferManager.CreateBufferManager(bindingElement.MaxBufferPoolSize, int.MaxValue);

            Collection<MessageEncodingBindingElement> messageEncoderBindingElements
                = context.BindingParameters.FindAll<MessageEncodingBindingElement>();

            if(messageEncoderBindingElements.Count > 1)
            {
                throw new InvalidOperationException("More than one MessageEncodingBindingElement was found in the BindingParameters of the BindingContext");
            }
            else if (messageEncoderBindingElements.Count == 1)
            {
                this.messageEncoderFactory = messageEncoderBindingElements[0].CreateMessageEncoderFactory();
            }
            else
            {
                this.messageEncoderFactory = UdpConstants.DefaultMessageEncoderFactory;
            }
        }

        public BufferManager BufferManager
        {
            get
            {
                return this.bufferManager;
            }
        }

        public MessageEncoderFactory MessageEncoderFactory
        {
            get
            {
                return this.messageEncoderFactory;
            }
        }

        public bool Multicast
        {
            get
            {
                return this.multicast;
            }
        }

        public override T GetProperty<T>()
        {
            T messageEncoderProperty = this.MessageEncoderFactory.Encoder.GetProperty<T>();
            if (messageEncoderProperty != null)
            {
                return messageEncoderProperty;
            }

            if (typeof(T) == typeof(MessageVersion))
            {
                return (T)(object)this.MessageEncoderFactory.Encoder.MessageVersion;
            }

            return base.GetProperty<T>();
        }

        protected override void OnOpen(TimeSpan timeout)
        {
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return new CompletedAsyncResult(callback, state);
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
            CompletedAsyncResult.End(result);
        }

        /// <summary>
        /// Called when [create channel].
        /// </summary>
        /// <param name="remoteAddress">The remote address.</param>
        /// <param name="via">The via.</param>
        /// <returns></returns>
        protected override IOutputChannel OnCreateChannel(EndpointAddress remoteAddress, Uri via)
        {
            return new UdpOutputChannel(this, remoteAddress, via, MessageEncoderFactory.Encoder);
        }

        /// <summary>
        /// Invoked during the transition of a communication object into the closing state.
        /// </summary>
        protected override void OnClosed()
        {
            base.OnClosed();
            this.bufferManager.Clear();
        }
    }
}
