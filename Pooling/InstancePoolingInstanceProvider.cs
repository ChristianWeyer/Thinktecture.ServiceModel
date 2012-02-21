/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Thinktecture.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    internal class InstancePoolingInstanceProvider : IInstanceProvider
    {
        private const int idleTimeout = 5 * 60 * 1000;
        private int minimumPoolSize;
        private Type serviceInstanceType;
        private Stack<object> pool;
        private object poolLock = new object();
        private int activeObjectsCount;
        private Timer idleTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstancePoolingInstanceProvider"/> class.
        /// </summary>
        /// <param name="instanceType">Type of the instance.</param>
        /// <param name="minPoolSize">Size of the min pool.</param>
        public InstancePoolingInstanceProvider(Type instanceType, int minPoolSize)
        {
            minimumPoolSize = minPoolSize;
            serviceInstanceType = instanceType;

            pool = new Stack<object>();
            activeObjectsCount = 0;

            idleTimer = new Timer(idleTimeout);
            idleTimer.Elapsed += new System.Timers.ElapsedEventHandler(idleTimer_Elapsed);

            Initialize();
        }

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext"/> object.</param>
        /// <returns>A user-defined service object.</returns>
        object IInstanceProvider.GetInstance(InstanceContext instanceContext)
        {
            return ((IInstanceProvider)this).GetInstance(instanceContext, null);
        }

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext"/> object.</param>
        /// <param name="message">The message that triggered the creation of a service object.</param>
        /// <returns>The service object.</returns>
        object IInstanceProvider.GetInstance(InstanceContext instanceContext, Message message)
        {
            object obj = null;

            lock (poolLock)
            {
                if (pool.Count > 0)
                {
                    obj = pool.Pop();
                }
                else
                {
                    obj = CreateNewPoolObject();
                }
                activeObjectsCount++;
            }

            idleTimer.Stop();

            return obj;
        }

        /// <summary>
        /// Called when an <see cref="T:System.ServiceModel.InstanceContext"/> object recycles a service object.
        /// </summary>
        /// <param name="instanceContext">The service's instance context.</param>
        /// <param name="instance">The service object to be recycled.</param>
        void IInstanceProvider.ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            lock (poolLock)
            {
                pool.Push(instance);
                activeObjectsCount--;

                if (activeObjectsCount == 0)
                    idleTimer.Start();
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        void Initialize()
        {
            for (int i = 0; i < minimumPoolSize; i++)
            {
                pool.Push(CreateNewPoolObject());
            }
        }

        /// <summary>
        /// Creates the new pool object.
        /// </summary>
        /// <returns></returns>
        private object CreateNewPoolObject()
        {
            return Activator.CreateInstance(serviceInstanceType);
        }

        /// <summary>
        /// Handles the Elapsed event of the idleTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        void idleTimer_Elapsed(object sender, ElapsedEventArgs args)
        {
            idleTimer.Stop();

            lock (poolLock)
            {
                if (activeObjectsCount == 0)
                {
                    while (pool.Count > minimumPoolSize)
                    {                      
                        object removedItem = pool.Pop();

                        if (removedItem is IDisposable)
                        {
                            ((IDisposable)removedItem).Dispose();
                        }
                    }
                }
            }
        }        
    }
}
