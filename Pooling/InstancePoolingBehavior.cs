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

namespace Thinktecture.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class InstancePoolingAttribute : Attribute, IServiceBehavior
    {
        private const int defaultMaxPoolSize = 32;
        private const int defaultMinPoolSize = 0;
        private int maxPoolSize = defaultMaxPoolSize;
        private int minPoolSize = defaultMinPoolSize;
        private ServiceThrottlingBehavior throttlingBehavior = null;

        /// <summary>
        /// Gets or sets the size of the min pool.
        /// </summary>
        /// <value>The size of the min pool.</value>
        public int MinPoolSize
        {
            get { return minPoolSize; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException(ResourceHelper.GetString("ExNegativePoolSize"));
                }

                minPoolSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the max pool.
        /// </summary>
        /// <value>The size of the max pool.</value>
        public int MaxPoolSize
        {
            get { return maxPoolSize; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException(ResourceHelper.GetString("ExNegativePoolSize"));
                }

                maxPoolSize = value;
            }
        }

        /// <summary>
        /// Adds the binding parameters.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="serviceHostBase">The service host base.</param>
        /// <param name="endpoints">The endpoints.</param>
        /// <param name="parameters">The parameters.</param>
        void IServiceBehavior.AddBindingParameters(ServiceDescription description, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection parameters)
        {
            if (throttlingBehavior != null)
            {
                ((IServiceBehavior)throttlingBehavior).AddBindingParameters(description, serviceHostBase, endpoints, parameters);
            }
        }

        /// <summary>
        /// Applies the dispatch behavior.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="serviceHostBase">The service host base.</param>
        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
            InstancePoolingInstanceProvider instanceProvider =
                new InstancePoolingInstanceProvider(description.ServiceType, minPoolSize);

            if (throttlingBehavior != null)
            {
                ((IServiceBehavior)throttlingBehavior).ApplyDispatchBehavior(description, serviceHostBase);
            }

            ServiceThrottle throttle = null;

            foreach (ChannelDispatcherBase cdb in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher cd = cdb as ChannelDispatcher;

                if (cd != null)
                {
                    if ((throttlingBehavior == null) && (maxPoolSize != Int32.MaxValue))
                    {
                        if (throttle == null)
                        {
                            throttle = cd.ServiceThrottle;
                        }

                        if (cd.ServiceThrottle == null)
                        {
                            throw new InvalidOperationException(ResourceHelper.GetString("ExNullThrottle"));
                        }

                        if (throttle != cd.ServiceThrottle)
                        {
                            throw new InvalidOperationException(ResourceHelper.GetString("ExDifferentThrottle"));
                        }
                    }

                    foreach (EndpointDispatcher ed in cd.Endpoints)
                    {
                        ed.DispatchRuntime.InstanceProvider = instanceProvider;
                    }
                }
            }

            if ((throttle != null) && (throttle.MaxConcurrentInstances > maxPoolSize))
            {
                throttle.MaxConcurrentInstances = maxPoolSize;
            }
        }

        /// <summary>
        /// Validates the specified description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="serviceHostBase">The service host base.</param>
        void IServiceBehavior.Validate(ServiceDescription description, ServiceHostBase serviceHostBase)
        {
            if (maxPoolSize < minPoolSize)
            {
                throw new InvalidOperationException(ResourceHelper.GetString("ExMinLargerThanMax"));
            }

            ServiceBehaviorAttribute serviceBehavior = description.Behaviors.Find<ServiceBehaviorAttribute>();

            if (serviceBehavior != null &&
                serviceBehavior.InstanceContextMode == InstanceContextMode.Single)
            {
                throw new InvalidOperationException(ResourceHelper.GetString("ExInvalidContext"));
            }

            int throttlingIndex = GetBehaviorIndex(description, typeof(ServiceThrottlingBehavior));

            if (throttlingIndex == -1)
            {
                throttlingBehavior = new ServiceThrottlingBehavior();
                throttlingBehavior.MaxConcurrentInstances = MaxPoolSize;

                ((IServiceBehavior)throttlingBehavior).Validate(description, serviceHostBase);
            }
            else
            {
                int poolingIndex = GetBehaviorIndex(description, typeof(InstancePoolingAttribute));

                if (poolingIndex < throttlingIndex)
                {
                    throw new InvalidOperationException(ResourceHelper.GetString("ExThrottleBeforePool"));
                }
            }
        }

        /// <summary>
        /// Gets the index of the behavior.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="behaviorType">Type of the behavior.</param>
        /// <returns></returns>
        int GetBehaviorIndex(ServiceDescription description, Type behaviorType)
        {
            for (int i = 0; i < description.Behaviors.Count; i++)
            {
                if (behaviorType.IsInstanceOfType(description.Behaviors[i]))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
