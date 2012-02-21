/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.ServiceModel.Channels;

namespace Thinktecture.ServiceModel
{
    /// <summary>
    /// This is a helper class to manage the app domain wide channel factory 
    /// instances.
    /// </summary>  
    public class ChannelFactoryManager<T> where T : class
    {
        private static readonly object padlock = new object();
        private static System.ServiceModel.ChannelFactory<T> factory;

        /// <summary>
        /// Creates a channel from the statically stored channel factory.
        /// </summary>
        /// <param name="endpointConfigurationName">Name of the corresponding endpoint configuration</param>
        public static T GetChannel(string endpointConfigurationName)
        {
            System.ServiceModel.ChannelFactory<T> f = GetFactory(endpointConfigurationName);
            T channel = f.CreateChannel();

            return channel;
        }

        /// <summary>
        /// Creates a channel from the statically stored channel factory.
        /// </summary>
        public static T GetChannel(Binding binding, string url)
        {
            System.ServiceModel.ChannelFactory<T> f = GetFactory(binding, url);
            T channel = f.CreateChannel();

            return channel;
        }

        /// <summary>
        /// Get cached Channel Factory instance. It opens automatically. 
        /// </summary>
        /// <param name="endpointConfigurationName">Name of the corresponding endpoint configuration</param>
        /// <returns>ChannelFactory</returns>
        public static System.ServiceModel.ChannelFactory<T> GetFactory(string endpointConfigurationName)
        {
            if (factory == null)
            {
                lock (padlock)
                {
                    if (factory == null)
                    {
                        factory = CreateFactoryInstance(endpointConfigurationName);
                    }
                }
            }

            return factory;
        }

        /// <summary>
        /// Get cached Channel Factory instance. It opens automatically. 
        /// </summary>
        /// <returns>ChannelFactory</returns>
        public static System.ServiceModel.ChannelFactory<T> GetFactory(Binding binding, string url)
        {
            if (factory == null)
            {
                lock (padlock)
                {
                    if (factory == null)
                    {
                        factory = CreateFactoryInstance(binding, url);
                    }
                }
            }

            return factory;
        }

        /// <summary>
        /// Get cached instance of the ChannelFactory (create if called for the first time)
        /// </summary>
        /// <param name="endpointConfigurationName">Name of the corresponding endpoint configuration</param>
        private static System.ServiceModel.ChannelFactory<T> CreateFactoryInstance(string endpointConfigurationName)
        {
            factory = new System.ServiceModel.ChannelFactory<T>(endpointConfigurationName);

            return factory;
        }

        /// <summary>
        /// Get cached instance of the ChannelFactory (create if called for the first time)
        /// </summary>
        private static System.ServiceModel.ChannelFactory<T> CreateFactoryInstance(Binding binding, string url)
        {
            factory = new System.ServiceModel.ChannelFactory<T>(binding, url);

            return factory;
        }
    }
}