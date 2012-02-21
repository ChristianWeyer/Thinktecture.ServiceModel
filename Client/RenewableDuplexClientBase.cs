/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.ServiceModel;

namespace Thinktecture.ServiceModel
{
    /// <summary>
    /// This class can be used as an alternative to DuplexClientBase class in .NET 3.0 when 
    /// channel recovery is desired.
    /// </summary>
    /// <typeparam name="TChannel">Type of the channel to be associated with this proxy.</typeparam>
    /// <remarks>This type is currently not in use.</remarks>
    public abstract class RenewableDuplexClientBase<TChannel>
        : RenewableClientBase<TChannel> where TChannel : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenewableDuplexClientBase&lt;TChannel&gt;"/> class.
        /// </summary>
        /// <param name="callbackInstance">The callback instance.</param>
        /// <param name="endpointConfiguration">The endpoint configuration.</param>
        public RenewableDuplexClientBase(
            object callbackInstance,
            string endpointConfiguration)
            : base(
                new InstanceContext(callbackInstance),
                endpointConfiguration)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenewableDuplexClientBase&lt;TChannel&gt;"/> class.
        /// </summary>
        /// <param name="callbackInstance">The callback instance.</param>
        /// <param name="endpointConfiguration">The endpoint configuration.</param>
        /// <param name="autoRecover">if set to <c>true</c> [auto recover].</param>
        public RenewableDuplexClientBase(
            object callbackInstance,
            string endpointConfiguration,
            bool autoRecover)
            : base(
                new InstanceContext(callbackInstance),
                endpointConfiguration,
                autoRecover)
        {
        }
    }
}