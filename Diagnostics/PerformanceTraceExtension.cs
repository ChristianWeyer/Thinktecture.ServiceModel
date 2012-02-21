/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Diagnostics;
using System.Globalization;
using System.ServiceModel;

namespace Thinktecture.ServiceModel.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    internal class PerformanceTraceExtension : IExtension<OperationContext>
    {
        private long ticks;

        /// <summary>
        /// Enables an extension object to find out when it has been aggregated. Called when the extension is added to the <see cref="P:System.ServiceModel.IExtensibleObject`1.Extensions"/> property.
        /// </summary>
        /// <param name="owner">The extensible object that aggregates this extension.</param>
        public void Attach(OperationContext owner)
        {
            ticks = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Enables an object to find out when it is no longer aggregated. Called when an extension is removed from the <see cref="P:System.ServiceModel.IExtensibleObject`1.Extensions"/> property.
        /// </summary>
        /// <param name="owner">The extensible object that aggregates this extension.</param>
        public void Detach(OperationContext owner)
        {
            ticks = DateTime.Now.Ticks - ticks;

            Debug.WriteLine(string.Format(CultureInfo.InvariantCulture,
                "WCF - Operation : {0} took {1} ticks",
                owner.IncomingMessageHeaders.Action, ticks));
        }
    }
}
