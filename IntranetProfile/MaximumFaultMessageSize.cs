/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Thinktecture.ServiceModel
{
    /// <summary>
    /// Custom EndpointBehavior for increasing the MaxFaultSize quota in the 
    /// ClientRuntime.
    /// </summary>
    internal class MaximumFaultMessageSize : IEndpointBehavior
    {
        private int messageSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaximumFaultMessageSize"/> class.
        /// </summary>
        public MaximumFaultMessageSize(int size)
        {
            messageSize = size;
        }

        /// <summary>
        /// Passes data at runtime to bindings to support custom behavior.
        /// </summary>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across an endpoint.
        /// </summary>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            if (clientRuntime != null)
                clientRuntime.MaxFaultSize = messageSize;
        }

        /// <summary>
        /// Implements a modification or extension of the service across an endpoint.
        /// </summary>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        /// <summary>
        /// Confirm that the endpoint meets some intended criteria.
        /// </summary>        
        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}