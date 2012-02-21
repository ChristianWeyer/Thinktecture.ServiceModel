// ----------------------------------------------------------------------------
// Copyright (C) 2003-2005 Microsoft Corporation, All rights reserved.
// ----------------------------------------------------------------------------

using System;
using System.Configuration;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

namespace Microsoft.ServiceModel.Samples
{
    /// <summary>
    /// Configuration section for Udp. 
    /// </summary>
    public class UdpTransportElement : BindingElementExtensionElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UdpTransportElement"/> class.
        /// </summary>
        public UdpTransportElement()
        {
        }

        #region Configuration_Properties
        /// <summary>
        /// Gets or sets the size of the max buffer pool.
        /// </summary>
        /// <value>The size of the max buffer pool.</value>
        [ConfigurationProperty(UdpConfigurationStrings.MaxBufferPoolSize, DefaultValue = UdpDefaults.MaxBufferPoolSize)]
        [LongValidator(MinValue = 0)]
        public long MaxBufferPoolSize
        {
            get { return (long)base[UdpConfigurationStrings.MaxBufferPoolSize]; }
            set { base[UdpConfigurationStrings.MaxBufferPoolSize] = value; }
        }

        /// <summary>
        /// Gets or sets the size of the max received message.
        /// </summary>
        /// <value>The size of the max received message.</value>
        [ConfigurationProperty(UdpConfigurationStrings.MaxReceivedMessageSize, DefaultValue = UdpDefaults.MaxReceivedMessageSize)]
        [IntegerValidator(MinValue = 1)]
        public int MaxReceivedMessageSize
        {
            get { return (int)base[UdpConfigurationStrings.MaxReceivedMessageSize]; }
            set { base[UdpConfigurationStrings.MaxReceivedMessageSize] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UdpTransportElement"/> is multicast.
        /// </summary>
        /// <value><c>true</c> if multicast; otherwise, <c>false</c>.</value>
        [ConfigurationProperty(UdpConfigurationStrings.Multicast, DefaultValue = UdpDefaults.Multicast)]
        public bool Multicast
        {
            get { return (bool)base[UdpConfigurationStrings.Multicast]; }
            set { base[UdpConfigurationStrings.Multicast] = value; }
        }
        #endregion

        /// <summary>
        /// When overridden in a derived class, gets the <see cref="T:System.Type"/> object that represents the custom binding element.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"/> object that represents the custom binding type.</returns>
        public override Type BindingElementType
        {
            get { return typeof(UdpTransportBindingElement); }
        }

        /// <summary>
        /// When overridden in a derived class, returns a custom binding element object.
        /// </summary>
        /// <returns>
        /// A custom <see cref="T:System.ServiceModel.Channels.BindingElement"/> object.
        /// </returns>
        protected override BindingElement CreateBindingElement()
        {
            UdpTransportBindingElement bindingElement = new UdpTransportBindingElement();
            this.ApplyConfiguration(bindingElement);
            return bindingElement;
        }

        #region Configuration_Infrastructure_overrides
        /// <summary>
        /// Applies the content of a specified binding element to this binding configuration element.
        /// </summary>
        /// <param name="bindingElement">A binding element.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="bindingElement"/> is null.</exception>
        public override void ApplyConfiguration(BindingElement bindingElement)
        {
            base.ApplyConfiguration(bindingElement);

            UdpTransportBindingElement udpBindingElement = (UdpTransportBindingElement)bindingElement;
            udpBindingElement.MaxBufferPoolSize = this.MaxBufferPoolSize;
            udpBindingElement.MaxReceivedMessageSize = this.MaxReceivedMessageSize;
            udpBindingElement.Multicast = this.Multicast;
        }

        /// <summary>
        /// Copies the content of the specified configuration element to this configuration element.
        /// </summary>
        /// <param name="from">The configuration element to be copied.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="from"/> is null.</exception>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">The configuration file is read-only.</exception>
        public override void CopyFrom(ServiceModelExtensionElement from)
        {
            base.CopyFrom(from);

            UdpTransportElement source = (UdpTransportElement)from;
            this.MaxBufferPoolSize = source.MaxBufferPoolSize;
            this.MaxReceivedMessageSize = source.MaxReceivedMessageSize;
            this.Multicast = source.Multicast;
        }

        /// <summary>
        /// Initializes this binding configuration section with the content of the specified binding element.
        /// </summary>
        /// <param name="bindingElement">A binding element.</param>
        protected override void InitializeFrom(BindingElement bindingElement)
        {
            base.InitializeFrom(bindingElement);

            UdpTransportBindingElement udpBindingElement = (UdpTransportBindingElement)bindingElement;
            this.MaxBufferPoolSize = udpBindingElement.MaxBufferPoolSize;
            this.MaxReceivedMessageSize = (int)udpBindingElement.MaxReceivedMessageSize;
            this.Multicast = udpBindingElement.Multicast;
        }

        /// <summary>
        /// Gets the collection of properties.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The <see cref="T:System.Configuration.ConfigurationPropertyCollection"/> of properties for the element.
        /// </returns>
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                ConfigurationPropertyCollection properties = base.Properties;
                properties.Add(new ConfigurationProperty(UdpConfigurationStrings.MaxBufferPoolSize,
                    typeof(long), UdpDefaults.MaxBufferPoolSize, null, new LongValidator(0, Int64.MaxValue), ConfigurationPropertyOptions.None));
                properties.Add(new ConfigurationProperty(UdpConfigurationStrings.MaxReceivedMessageSize,
                    typeof(int), UdpDefaults.MaxReceivedMessageSize, null, new IntegerValidator(1, Int32.MaxValue), ConfigurationPropertyOptions.None));
                properties.Add(new ConfigurationProperty(UdpConfigurationStrings.Multicast,
                    typeof(Boolean), UdpDefaults.Multicast, null, null, ConfigurationPropertyOptions.None));
                return properties;
            }
        }
        #endregion
    }
}
