/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Configuration;
using System.ServiceModel.Configuration;

namespace Thinktecture.ServiceModel.IoC
{
    /// <summary>
    /// 
    /// </summary>
    public class IocBehaviorExtensionElement : BehaviorExtensionElement
    {
        private const string ContainerConfigurationPropertyName = "containerName";

        /// <summary>
        /// Gets or sets the name of the container.
        /// </summary>
        /// <value>The name of the container.</value>
        [ConfigurationProperty(ContainerConfigurationPropertyName, IsRequired = true)]
        public string ContainerName
        {
            get { return (string)base[ContainerConfigurationPropertyName]; }
            set { base[ContainerConfigurationPropertyName] = value; }
        }

        /// <summary>
        /// Gets the type of behavior.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"/>.</returns>
        public override Type BehaviorType
        {
            get { return typeof(IocServiceBehaviorAttribute); }
        }

        /// <summary>
        /// Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>The behavior extension.</returns>
        protected override object CreateBehavior()
        {
            return new IocServiceBehaviorAttribute(ContainerName);
        }
    }
}
