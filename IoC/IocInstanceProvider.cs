/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.ServiceLocation;
using Thinktecture.ServiceModel.Utility;

namespace Thinktecture.ServiceModel.IoC
{
    /// <summary>
    /// NOTE: only works with non-singleton instances.
    /// </summary>
    public partial class IocInstanceProvider : IInstanceProvider
    {
        private readonly Type type;
        private IServiceLocator container;
        private static string source = "Thinktecture.ServiceModel";

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>The container.</value>
        public IServiceLocator Container
        {
            get { return container; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IocInstanceProvider"/> class.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public IocInstanceProvider(Type serviceType)
            : this(serviceType, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IocInstanceProvider"/> class.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="containerName">Name of the container.</param>
        public IocInstanceProvider(Type serviceType, string containerName)
        {
            type = serviceType;
            container = CreateUnityContainer(containerName);
        } 

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext"/> object.</param>
        /// <returns>A user-defined service object.</returns>
        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext"/> object.</param>
        /// <param name="message">The message that triggered the creation of a service object.</param>
        /// <returns>The service object.</returns>
        public object GetInstance(InstanceContext instanceContext, System.ServiceModel.Channels.Message message)
        {
            try
            {
                return container.GetInstance(type);
            }
            catch (ActivationException ex)
            {
                Exception realEx = ex.InnerException.InnerException.InnerException.InnerException.InnerException;
                Trace.LogException(source, realEx);

                throw realEx;
            }
        }

        /// <summary>
        /// Called when an <see cref="T:System.ServiceModel.InstanceContext"/> object recycles a service object.
        /// </summary>
        /// <param name="instanceContext">The service's instance context.</param>
        /// <param name="instance">The service object to be recycled.</param>
        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            var objectInstance = instance as IDisposable;

            if (objectInstance != null)
            {
                objectInstance.Dispose();
            }
        }
    }
}
