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

namespace Thinktecture.ServiceModel.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PerformanceTraceAttribute : 
        Attribute, IOperationBehavior
    {
        /// <summary>
        /// Implement to pass data at runtime to bindings to support custom behavior.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined.</param>
        /// <param name="bindingParameters">The collection of objects that binding elements require to support the behavior.</param>
        void IOperationBehavior.AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across an operation.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined.</param>
        /// <param name="clientOperation">The run-time object that exposes customization properties for the operation described by <paramref name="operationDescription"/>.</param>
        void IOperationBehavior.ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the service across an operation.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined.</param>
        /// <param name="dispatchOperation">The run-time object that exposes customization properties for the operation described by <paramref name="operationDescription"/>.</param>
        void IOperationBehavior.ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.Invoker = new PerformanceTraceInvoker(dispatchOperation.Invoker);
        }

        /// <summary>
        /// Implement to confirm that the operation meets some intended criteria.
        /// </summary>
        /// <param name="operationDescription">The operation being examined. Use for examination only. If the operation description is modified, the results are undefined.</param>
        void IOperationBehavior.Validate(OperationDescription operationDescription)
        {
        }      
    } 
}
