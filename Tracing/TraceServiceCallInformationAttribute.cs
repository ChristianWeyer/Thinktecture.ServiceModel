/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Thinktecture.ServiceModel.Tracing
{
    /// <summary>
    /// Operation behavior to hook up the custom parameter inspector <see href="CallLogParameterInspector"/> for
    /// logging service operation calls to the console.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TraceServiceCallInformationAttribute : Attribute, IOperationBehavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogCallAttribute"/> class.
        /// </summary>
        public TraceServiceCallInformationAttribute()
        {
        }

        /// <summary>
        /// Adds the binding parameters.
        /// </summary>
        /// <remarks>This method has zero instructions.</remarks>
        public void AddBindingParameters(OperationDescription operationDescription,
                                         BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Applies the client behavior.
        /// </summary>
        /// <remarks>This method has zero instructions.</remarks>
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        /// <summary>
        /// Applies the dispatch behavior by adding an instance of <see cref="CallLogParameterInspector"/> to the 
        /// parameter inspectors collection of the dispatch operation.
        /// </summary>        
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.ParameterInspectors.Add(
                new CallTraceParameterInspector());
        }

        /// <summary>
        /// Validates the specified operation description.
        /// </summary> 
        /// <remarks>This method has zero instructions.</remarks>
        public void Validate(OperationDescription operationDescription)
        {
        }
    }
}