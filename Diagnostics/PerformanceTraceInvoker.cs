/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;

namespace Thinktecture.ServiceModel.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    internal class PerformanceTraceInvoker : IOperationInvoker
    {
        private IOperationInvoker innerOperationInvoker;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceTraceInvoker"/> class.
        /// </summary>
        /// <param name="invoker">The invoker.</param>
        public PerformanceTraceInvoker(IOperationInvoker invoker)
        {
            innerOperationInvoker = invoker;
        }

        /// <summary>
        /// Returns an <see cref="T:System.Array"/> of parameter objects.
        /// </summary>
        /// <returns>
        /// The parameters that are to be used as arguments to the operation.
        /// </returns>
        object[] IOperationInvoker.AllocateInputs()
        {
            return innerOperationInvoker.AllocateInputs();
        }

        /// <summary>
        /// Returns an object and a set of output objects from an instance and set of input objects.
        /// </summary>
        /// <param name="instance">The object to be invoked.</param>
        /// <param name="inputs">The inputs to the method.</param>
        /// <param name="outputs">The outputs from the method.</param>
        /// <returns>The return value.</returns>
        object IOperationInvoker.Invoke(object instance, object[] inputs, out object[] outputs)
        {
            OperationContext.Current.Extensions.Add(new PerformanceTraceExtension());

            try
            {
                return innerOperationInvoker.Invoke(instance, inputs, out outputs);
            }
            finally
            {
                OperationContext.Current.Extensions.Remove(
                    OperationContext.Current.Extensions.Find<PerformanceTraceExtension>());
            }
        }

        /// <summary>
        /// An asynchronous implementation of the <see cref="M:System.ServiceModel.Dispatcher.IOperationInvoker.Invoke(System.Object,System.Object[],System.Object[]@)"/> method.
        /// </summary>
        /// <param name="instance">The object to be invoked.</param>
        /// <param name="inputs">The inputs to the method.</param>
        /// <param name="callback">The asynchronous callback object.</param>
        /// <param name="state">Associated state data.</param>
        /// <returns>
        /// A <see cref="T:System.IAsyncResult"/> used to complete the asynchronous call.
        /// </returns>
        IAsyncResult IOperationInvoker.InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            OperationContext.Current.Extensions.Add(new PerformanceTraceExtension());

            return innerOperationInvoker.InvokeBegin(instance, inputs, callback, state);
        }

        /// <summary>
        /// The asynchronous end method.
        /// </summary>
        /// <param name="instance">The object invoked.</param>
        /// <param name="outputs">The outputs from the method.</param>
        /// <param name="result">The <see cref="T:System.IAsyncResult"/> object.</param>
        /// <returns>The return value.</returns>
        object IOperationInvoker.InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            OperationContext.Current.Extensions.Remove(
                OperationContext.Current.Extensions.Find<PerformanceTraceExtension>());

            return innerOperationInvoker.InvokeEnd(instance, out outputs, result);
        }

        /// <summary>
        /// Gets a value that specifies whether the <see cref="M:System.ServiceModel.Dispatcher.IOperationInvoker.Invoke(System.Object,System.Object[],System.Object[]@)"/> or <see cref="M:System.ServiceModel.Dispatcher.IOperationInvoker.InvokeBegin(System.Object,System.Object[],System.AsyncCallback,System.Object)"/> method is called by the dispatcher.
        /// </summary>
        /// <value></value>
        /// <returns>true if the dispatcher invokes the synchronous operation; otherwise, false.</returns>
        bool IOperationInvoker.IsSynchronous
        {
            get { return innerOperationInvoker.IsSynchronous; }
        }
    }
}
