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

namespace Thinktecture.ServiceModel
{
    /// <summary>
    /// Custom channel factory that can be used to apply the optimizations for intranet 
    /// scenarios.
    /// </summary>
    public sealed class ChannelFactory<TChannel> : System.ServiceModel.ChannelFactory<TChannel>
    {
        private bool alreadyInitialized;
        private Profile usageProfile = Profile.Internet;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelFactory&lt;TChannel&gt;"/> class.
        /// </summary>
        public ChannelFactory()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelFactory&lt;TChannel&gt;"/> class.
        /// </summary>        
        public ChannelFactory(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelFactory&lt;TChannel&gt;"/> class.
        /// </summary>        
        public ChannelFactory(Binding binding)
            : base(binding)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelFactory&lt;TChannel&gt;"/> class.
        /// </summary>
        public ChannelFactory(ServiceEndpoint endpoint)
            : base(endpoint)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelFactory&lt;TChannel&gt;"/> class.
        /// </summary>        
        public ChannelFactory(Type channelType)
            : base(channelType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelFactory&lt;TChannel&gt;"/> class.
        /// </summary>
        public ChannelFactory(Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelFactory&lt;TChannel&gt;"/> class.
        /// </summary>
        public ChannelFactory(Binding binding, string remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelFactory&lt;TChannel&gt;"/> class.
        /// </summary>
        public ChannelFactory(string endpointConfigurationName, EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelFactory&lt;TChannel&gt;"/> class.
        /// </summary>        
        public ChannelFactory(Profile profile)
        {
            usageProfile = profile;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelFactory&lt;TChannel&gt;"/> class.
        /// </summary>
        public ChannelFactory(string endpointConfigurationName, Profile profile)
            : base(endpointConfigurationName)
        {
            usageProfile = profile;
            ApplyConfiguration(endpointConfigurationName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelFactory&lt;TChannel&gt;"/> class.
        /// </summary>
        public ChannelFactory(Binding binding, Profile profile)
            : base(binding)
        {
            usageProfile = profile;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelFactory&lt;TChannel&gt;"/> class.
        /// </summary>
        public ChannelFactory(ServiceEndpoint endpoint, Profile profile)
            : base(endpoint)
        {
            usageProfile = profile;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelFactory&lt;TChannel&gt;"/> class.
        /// </summary>
        public ChannelFactory(Type channelType, Profile profile)
            : base(channelType)
        {
            usageProfile = profile;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelFactory&lt;TChannel&gt;"/> class.
        /// </summary>
        public ChannelFactory(Binding binding, EndpointAddress remoteAddress, Profile profile)
            : base(binding, remoteAddress)
        {
            usageProfile = profile;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelFactory&lt;TChannel&gt;"/> class.
        /// </summary>
        public ChannelFactory(Binding binding, string remoteAddress, Profile profile)
            : base(binding, remoteAddress)
        {
            usageProfile = profile;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelFactory&lt;TChannel&gt;"/> class.
        /// </summary>
        public ChannelFactory(string endpointConfigurationName, EndpointAddress remoteAddress, Profile profile)
            : base(endpointConfigurationName, remoteAddress)
        {
            usageProfile =profile;
            ApplyConfiguration(endpointConfigurationName);
        }

        /// <summary>
        /// Returns the used usage profile.
        /// </summary>        
        public Profile Profile
        {
            get { return usageProfile; }
        }

        /// <summary>
        /// Initializes the channel factory with the behaviors provided by a specified configuration file and with those in the service endpoint of the channel factory.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The service endpoint of the channel factory is null.</exception>
        protected override void ApplyConfiguration(string configurationName)
        {
            if (!alreadyInitialized)
            {
                base.ApplyConfiguration(configurationName);
                alreadyInitialized = true;
            }
            else
            {
                if (usageProfile == Profile.Intranet)
                {
                    Endpoint.Binding = BindingController.IncreaseBindingQuotas(Endpoint.Binding);
                    Endpoint.Behaviors.Add(new MaximumFaultMessageSize(int.MaxValue));

                    foreach (OperationDescription opDesc in Endpoint.Contract.Operations)
                    {
                        DataContractSerializerOperationBehavior dcs =
                            opDesc.Behaviors.Find<DataContractSerializerOperationBehavior>();

                        if (dcs != null)
                            dcs.MaxItemsInObjectGraph = int.MaxValue;
                    }
                }
            }
        }
    }
}