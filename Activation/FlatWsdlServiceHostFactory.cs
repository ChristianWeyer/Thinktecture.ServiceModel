/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.ServiceModel;

namespace Thinktecture.ServiceModel.Activation
{
    /// <summary>
    /// Custom ServiceHostFactory for creating ServiceHost instances that support
    /// flattened WSDL emmision.
    /// </summary>
    public sealed class FlatWsdlServiceHostFactory : System.ServiceModel.Activation.ServiceHostFactory
    {
        /// <summary>
        /// Creates a <see cref="T:System.ServiceModel.ServiceHost"></see> with specific base addresses and initializes it with specified data.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.ServiceHost"></see> with specific base addresses.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">There is no hosting context provided or constructorString is null or empty.</exception>
        /// <exception cref="T:System.ArgumentNullException">baseAddress is null.</exception>
        public override ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
        {
            return base.CreateServiceHost(constructorString, baseAddresses);
        }

        /// <summary>
        /// Creates a <see cref="T:System.ServiceModel.ServiceHost"></see> for a specified type of service with a specific base address.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.ServiceHost"></see> for the type of service specified with a specific base address.
        /// </returns>
        protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return new ServiceHost(serviceType, Profile.Internet, Flattening.Enabled, baseAddresses);
        }
    }
}