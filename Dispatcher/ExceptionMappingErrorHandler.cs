/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Thinktecture.ServiceModel.Dispatcher
{
    /// <summary>
    /// 
    /// </summary>
    sealed class ExceptionMappingErrorHandler : IErrorHandler
    {
        private ExceptionHandlingAttribute handlingAttribute;
        private IExceptionMapper exceptionMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMappingErrorHandler"/> class.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        public ExceptionMappingErrorHandler(ExceptionHandlingAttribute attribute)
        {
            handlingAttribute = attribute;

            if (handlingAttribute.ExceptionMapper != null)
                exceptionMapper = (IExceptionMapper)Activator.CreateInstance(handlingAttribute.ExceptionMapper);
        }

        /// <summary>
        /// Enables error-related processing and returns a value that indicates whether subsequent HandleError implementations are called.
        /// </summary>
        /// <param name="error">The exception thrown during processing.</param>
        /// <returns>
        /// true if subsequent <see cref="T:System.ServiceModel.Dispatcher.IErrorHandler"/> implementations must not be called; otherwise, false. The default is false.
        /// </returns>
        public bool HandleError(Exception error)
        {
            return true;
        }

        /// <summary>
        /// Provides the fault.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="version">The version.</param>
        /// <param name="fault">The fault.</param>
        public void ProvideFault(Exception exception, MessageVersion version, ref Message fault)
        {
            if (exception is FaultException)
                return;

            ServiceEndpoint endpoint =
                OperationContext.Current.Host.Description.Endpoints.Find(
                    OperationContext.Current.EndpointDispatcher.EndpointAddress.Uri);
            DispatchOperation dispatchOperation =
                OperationContext.Current.EndpointDispatcher.DispatchRuntime.Operations.Where(
                    op => op.Action == OperationContext.Current.IncomingMessageHeaders.Action).First();
            OperationDescription operationDesc =
                endpoint.Contract.Operations.Find(dispatchOperation.Name);

            object faultDetail = GetFaultDetail(operationDesc.SyncMethod, operationDesc.Faults, exception);

            if (faultDetail != null)
            {
                Type faultExceptionType =
                    typeof(FaultException<>).MakeGenericType(faultDetail.GetType());
                FaultException faultException =
                    (FaultException)Activator.CreateInstance(faultExceptionType, faultDetail, exception.Message);
                MessageFault faultMessage = faultException.CreateMessageFault();
                fault = Message.CreateMessage(version, faultMessage, faultException.Action);
            }
        }

        /// <summary>
        /// Gets the fault detail.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="faults">The faults.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        private object GetFaultDetail(MethodInfo method, FaultDescriptionCollection faults,
            Exception error)
        {
            object faultDetail = null;

            if (exceptionMapper != null)
            {
                faultDetail = exceptionMapper.MapToFault(error);
            }

            if (faultDetail == null && method != null)
            {
                ExceptionMappingAttribute[] mappers = (ExceptionMappingAttribute[])
                    method.GetCustomAttributes(typeof(ExceptionMappingAttribute), true);
            
                foreach (ExceptionMappingAttribute mapAttribute in mappers)
                {
                    if (mapAttribute.ExceptionType == error.GetType())
                    {
                        faultDetail = mapAttribute.GetFaultDetailForException(error);
                
                        if (faultDetail != null)
                        {
                            break;
                        }
                    }
                }
            }

            if (faultDetail != null && handlingAttribute.EnforceFaultContract &&
                !faults.Any(f => f.DetailType == faultDetail.GetType()))
            {
                faultDetail = null;
            }

            if (faultDetail == null)
            {
                foreach (FaultDescription faultDesc in faults)
                {
                    if (faultDesc.DetailType == error.GetType())
                    {
                        faultDetail = error;
                        break;
                    }
                }
            }

            return faultDetail;
        }
    }
}
