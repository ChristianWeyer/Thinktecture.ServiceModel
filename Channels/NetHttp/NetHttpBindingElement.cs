using System;
using System.ServiceModel.Configuration;

namespace Thinktecture.ServiceModel.Channels
{
    /// <summary>
    /// 
    /// </summary>
    public class NetHttpBindingElement : BasicHttpBindingElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetHttpBindingElement"/> class.
        /// </summary>
        /// <param name="configurationName">Name of the configuration.</param>
        public NetHttpBindingElement(string configurationName) :
            base(configurationName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetHttpBindingElement"/> class.
        /// </summary>
        public NetHttpBindingElement()
            : this(null)
        {
        }

        protected override Type BindingElementType
        {
            get
            {
                return typeof(NetHttpBinding);
            }
        }
    }
}
