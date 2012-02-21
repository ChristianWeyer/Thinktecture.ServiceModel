/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Thinktecture.ServiceModel.IoC
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class IocServiceBehaviorAttribute : Attribute, IServiceBehavior
    {
        private readonly string containerName;

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        public string Container
        {
            get
            {
                return containerName;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IocServiceBehaviorAttribute"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public IocServiceBehaviorAttribute(string container)
        {
            containerName = container;
        }

        /// <summary>
        /// Provides the ability to change run-time property values or insert custom extension objects such as error handlers, message or parameter interceptors, security extensions, and other custom extension objects.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The host that is currently being built.</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription,
                                          ServiceHostBase serviceHostBase)
        {
            foreach (var channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                var channelDispatcher = channelDispatcherBase as ChannelDispatcher;

                if (channelDispatcher == null)
                    continue;

                foreach (var endpointDispatcher in channelDispatcher.Endpoints)
                {
                    endpointDispatcher.DispatchRuntime.InstanceProvider =
                        new IocInstanceProvider(serviceDescription.ServiceType, containerName);
                }
            }
        }

        /// <summary>
        /// Provides the ability to pass custom data to binding elements to support the contract implementation.
        /// </summary>
        /// <param name="serviceDescription">The service description of the service.</param>
        /// <param name="serviceHostBase">The host of the service.</param>
        /// <param name="endpoints">The service endpoints.</param>
        /// <param name="bindingParameters">Custom objects to which binding elements have access.</param>
        public void AddBindingParameters(ServiceDescription serviceDescription,
                                         ServiceHostBase serviceHostBase,
                                         Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Provides the ability to inspect the service host and the service description to confirm that the service can run successfully.
        /// </summary>
        /// <param name="serviceDescription">The service description.</param>
        /// <param name="serviceHostBase">The service host that is currently being constructed.</param>
        public void Validate(ServiceDescription serviceDescription,
                             ServiceHostBase serviceHostBase)
        {
        }
    }
}
