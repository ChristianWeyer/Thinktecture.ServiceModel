/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.ServiceLocatorAdapter;

namespace Thinktecture.ServiceModel.IoC
{
    /// <summary>
    /// NOTE: only works correctly with non-singleton instances.
    /// </summary>
    public partial class IocInstanceProvider
    {
        private static IServiceLocator CreateUnityContainer(string containerName)
        {
            IUnityContainer unity = new UnityContainer();
            var section = ConfigurationManager.GetSection("unity") as UnityConfigurationSection;

            if (section == null)
            {
                throw new InvalidOperationException("No Unity configuration section found.");
            }

            if (containerName == null)
            {
                section.Containers.Default.Configure(unity);
            }
            else
            {
                section.Containers[containerName].Configure(unity);
            }

            return new UnityServiceLocator(unity);
        }
    }
}

