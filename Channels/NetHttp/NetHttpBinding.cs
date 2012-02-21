using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

namespace Thinktecture.ServiceModel.Channels
{
    /// <summary>
    /// 
    /// </summary>
public class NetHttpBinding : BasicHttpBinding
    {
        public NetHttpBinding()
            : this(BasicHttpSecurityMode.Transport)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetHttpBinding"/> class.
        /// </summary>
        /// <param name="configurationName">Name of the configuration.</param>
        public NetHttpBinding(string configurationName)
            : this()
        {
            ApplyConfiguration(configurationName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetHttpBinding"/> class.
        /// </summary>
        /// <param name="securityMode">The security mode.</param>
        public NetHttpBinding(BasicHttpSecurityMode securityMode)
            : base(securityMode)
        {
        }

        /// <summary>
        /// Creates a collection that contains the binding elements that are part of the current binding.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.ICollection`1"/> object of type 
        /// <see cref="T:System.ServiceModel.Channels.BindingElement"/> that contains the binding elements 
        /// from the current binding object in the correct order.
        /// </returns>
        public override BindingElementCollection CreateBindingElements()
        {
            try
            {
                var basicHttpElements = base.CreateBindingElements();

                return new BindingElementCollection(basicHttpElements.Select(e => Transform(e)));
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(Translate(ex.Message));
            }
        }

        private string Translate(string basicHttpMessage)
        {
            basicHttpMessage = basicHttpMessage.Replace("BasicHttp binding", "NetHttp binding");
            basicHttpMessage = basicHttpMessage.Replace("BasicHttpBinding", "NetHttpBinding");

            return basicHttpMessage;
        }

        private BindingElement Transform(BindingElement original)
        {
            if (original is MessageEncodingBindingElement)
                return new BinaryMessageEncodingBindingElement();

            return original;
        }

        private void ApplyConfiguration(string configurationName)
        {
            BindingsSection bindings = ((BindingsSection)(ConfigurationManager.GetSection("system.serviceModel/bindings")));
            NetHttpBindingCollectionElement section = (NetHttpBindingCollectionElement)bindings["netHttpBinding"];
            NetHttpBindingElement element = section.Bindings[configurationName];

            if ((element == null))
            {
                throw new System.Configuration.ConfigurationErrorsException(string.Format(CultureInfo.CurrentCulture, "There is no binding named {0} at {1}.", configurationName, section.BindingName));
            }
            else
            {
                element.ApplyConfiguration(this);
            }
        }
    }
}
