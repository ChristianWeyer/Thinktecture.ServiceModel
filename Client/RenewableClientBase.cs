/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.ServiceModel;

namespace Thinktecture.ServiceModel
{
    /// <summary>
    /// This class can be used as an alternative to ClientBase class in .NET 3.0 when 
    /// channel recovery is desired.
    /// </summary>
    /// <typeparam name="TChannel">Type of the channel to be associated with this proxy.</typeparam>
    /// <remarks>This type is currently not in use.</remarks>
    public abstract class RenewableClientBase<TChannel> : ICommunicationObject where TChannel : class
    {
        private bool performAutoRecover;
        private TChannel channel;
        private System.ServiceModel.ChannelFactory<TChannel> channelFactory;
        private IClientChannel clientChannel; // Let's keep a casted reference to avoid multiple casting.

        /// <summary>
        /// Initializes a new instance of the <see cref="RenewableClientBase{TChannel}"/> class.
        /// </summary>
        /// <param name="endpointConfiguration">The endpoint configuration name.</param>
        /// <remarks>Currently we support only the configuration file based initialization</remarks>
        public RenewableClientBase(string endpointConfiguration)
        {
            clientChannel = default(IClientChannel);
            channel = default(TChannel);
            channelFactory = new ChannelFactory<TChannel>(endpointConfiguration);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenewableClientBase{TChannel}"/> class.
        /// </summary>
        /// <param name="endpointConfiguration">The endpoint configuration name.</param>
        /// <param name="autoRecover">
        /// if set to <c>true</c> the proxy will automatically recover the faulting channels.
        /// </param>
        /// /// <remarks>Currently we support only the configuration file based initialization.</remarks>
        public RenewableClientBase(string endpointConfiguration, bool autoRecover)
            : this(endpointConfiguration)
        {
            performAutoRecover = autoRecover;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenewableClientBase&lt;TChannel&gt;"/> class.
        /// </summary>
        /// <param name="instanceContext">The instance context for servicing the callback methods.</param>
        /// <param name="endpointConfiguration">The endpoint configuration name.</param>
        /// <remarks>Currently we support only the configuration file based initialization.</remarks>
        internal RenewableClientBase(
            InstanceContext instanceContext,
            string endpointConfiguration)
        {
            clientChannel = default(IClientChannel);
            channel = default(TChannel);
            channelFactory = new DuplexChannelFactory<TChannel>(
                instanceContext,
                endpointConfiguration);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenewableClientBase&lt;TChannel&gt;"/> class.
        /// </summary>
        /// <param name="callbackInstance">The instance context for servicing the callback methods.</param>
        /// <param name="endpointConfiguration">The endpoint configuration name.</param>
        /// <param name="autoRecover">
        /// if set to <c>true</c> the proxy will automatically recover the faulting channels.
        /// </param>
        /// <remarks>Currently we support only the configuration file based initialization.</remarks>
        internal RenewableClientBase(
            InstanceContext callbackInstance,
            string endpointConfiguration,
            bool autoRecover)
            :
                this(callbackInstance, endpointConfiguration)
        {
            performAutoRecover = autoRecover;
        }

        /// <summary>
        /// Gets the TChannel.
        /// </summary>
        protected TChannel Channel
        {
            get
            {
                InitializeChannel();
                return channel;
            }
        }
        
        /// <summary>
        /// Event on closed.
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// Event on closing.
        /// </summary>
        public event EventHandler Closing;

        /// <summary>
        /// Event on faulted.
        /// </summary>
        public event EventHandler Faulted;

        /// <summary>
        /// Event on opened.
        /// </summary>
        public event EventHandler Opened;

        /// <summary>
        /// Event on opening.
        /// </summary>
        public event EventHandler Opening;

        /// <summary>
        /// Aborts the channel factory and the channel if available.
        /// </summary>
        public void Abort()
        {
            if (clientChannel != null)
                clientChannel.Abort();
            channelFactory.Abort();
        }

        /// <summary>
        /// Closes the channel factory and the channel if available.
        /// </summary>        
        void ICommunicationObject.Close(TimeSpan timeout)
        {
            if (clientChannel != null)
                clientChannel.Close(timeout);
            channelFactory.Close(timeout);
        }

        /// <summary>
        /// Closes the channel factory and the channel if available.
        /// </summary>
        public void Close()
        {
            ((ICommunicationObject) this).Close(TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// Opens the current channel.
        /// </summary>
        void ICommunicationObject.Open(TimeSpan timeout)
        {
            InitializeChannel();
            clientChannel.Open(timeout);
        }

        /// <summary>
        /// Opens the current channel.
        /// </summary>
        public void Open()
        {
            ((ICommunicationObject) this).Open(TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// Gets the state of current channel. If the channel is 
        /// not created CommunicationState.Created is returned.
        /// </summary>
        public CommunicationState State
        {
            get
            {
                if (clientChannel != null)
                {
                    return clientChannel.State;
                }
                return CommunicationState.Created;
            }
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public IAsyncResult BeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public IAsyncResult BeginClose(AsyncCallback callback, object state)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public IAsyncResult BeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public IAsyncResult BeginOpen(AsyncCallback callback, object state)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public void EndClose(IAsyncResult result)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public void EndOpen(IAsyncResult result)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Contains the logic for re-initializing the channel
        /// using the same channel factory.
        /// </summary>
        public void RenewIfFaulted()
        {
            if (clientChannel == null)
                throw new InvalidOperationException("Channel is not initialized!");

            if (clientChannel.State == CommunicationState.Faulted)
            {
                lock (channelFactory)
                {
                    if (clientChannel.State == CommunicationState.Faulted)
                    {
                        clientChannel.Abort(); // Abort the faulted channel and let it go.
                        channel = channelFactory.CreateChannel();
                        clientChannel = (IClientChannel) channel;
                        SubscribeForChannelEvents(clientChannel);
                    }
                }
            }
        }

        /// <summary>
        /// Contains the logic for initializing the channel for the first time.
        /// </summary>
        private void InitializeChannel()
        {
            if (channel == null)
            {
                lock (channelFactory)
                {
                    if (channel == null)
                    {
                        channel = channelFactory.CreateChannel();
                        clientChannel = (IClientChannel) channel;
                        SubscribeForChannelEvents(clientChannel);
                    }
                }
            }
        }

        /// <summary>
        /// Attaches the event handlers to a given channel.
        /// </summary>        
        private void SubscribeForChannelEvents(IClientChannel channel)
        {
            channel.Opening += new EventHandler(channel_Opening);
            channel.Opened += new EventHandler(channel_Opened);
            channel.Closing += new EventHandler(channel_Closing);
            channel.Closed += new EventHandler(channel_Closed);
            channel.Faulted += new EventHandler(channel_Faulted);
        }

        /// <summary>
        /// Handles the Faulted event of the channel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void channel_Faulted(object sender, EventArgs e)
        {
            if (Faulted != null)
                Faulted(this, e);
            if (performAutoRecover)
                RenewIfFaulted();
        }

        /// <summary>
        /// Handles the Closed event of the channel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void channel_Closed(object sender, EventArgs e)
        {
            if (Closed != null)
                Closed(this, e);
        }

        /// <summary>
        /// Handles the Closing event of the channel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void channel_Closing(object sender, EventArgs e)
        {
            if (Closing != null)
                Closing(this, e);
        }

        /// <summary>
        /// Handles the Opened event of the channel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void channel_Opened(object sender, EventArgs e)
        {
            if (Opened != null)
                Opened(this, e);
        }

        /// <summary>
        /// Handles the Opening event of the channel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void channel_Opening(object sender, EventArgs e)
        {
            if (Opening != null)
                Opening(this, e);
        }
    }
}