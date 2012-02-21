/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.ServiceModel.Description;

namespace Thinktecture.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceHostExtensions
    {
        /// <summary>
        /// Adds or replaces a service behavior.
        /// </summary>
        /// <typeparam name="TBehavior">The type of the behavior.</typeparam>
        /// <param name="serviceHost">The service host.</param>
        /// <param name="behavior">The new behavior.</param>
        public static void AddOrReplaceBehavior<TBehavior>(this System.ServiceModel.ServiceHost serviceHost, TBehavior behavior) where TBehavior : IServiceBehavior
        {
            if (serviceHost.Description.Behaviors.Find<TBehavior>() != null)
                serviceHost.Description.Behaviors.Remove<TBehavior>();               

            serviceHost.Description.Behaviors.Add(behavior);
        }
    }
}
