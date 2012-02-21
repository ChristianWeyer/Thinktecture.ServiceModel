/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using Thinktecture.ServiceModel.Properties;
using Thinktecture.ServiceModel.Utility;

namespace Thinktecture.ServiceModel.Tracing
{
    /// <summary>
    /// This custom parameter inspector is for logging operation invocations
    /// to the console.
    /// </summary>
    public class CallTraceParameterInspector : IParameterInspector
    {
        private const string source = "Thinktecture.ServiceCall";

        /// <summary>
        /// Initializes a new instance of the <see cref="CallLogParameterInspector"/> class.
        /// </summary>        
        public CallTraceParameterInspector()
        {            
        }

        /// <summary>
        /// Gets called after the call.
        /// </summary>
        /// <remarks>This method has zero instructions.</remarks>
        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
        }

        /// <summary>
        /// Logs the calls to the console before invoking the service method.
        /// </summary>        
        public object BeforeCall(string operationName, object[] inputs)
        {
            Trace.Information(source, String.Format(CultureInfo.InvariantCulture,
                                                    Resources.ServiceAndOperationCalled,
                                                    OperationContext.Current.Host.Description.Name,
                                                    operationName));
                        
            return null;
        }
    }
}