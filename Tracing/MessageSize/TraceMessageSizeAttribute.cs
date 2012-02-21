/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Thinktecture.ServiceModel.Tracing
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TraceMessageSizeAttribute : Attribute, IOperationBehavior
    {
        List<DispatchOperation> dispatchOperations = 
            new List<DispatchOperation>();
        List<ClientOperation> clientOperations =  
            new List<ClientOperation>();

        class TraceMessageSizeFormatter : IDispatchMessageFormatter, 
            IClientMessageFormatter
        {
            private IDispatchMessageFormatter innerFormatter;
            private IClientMessageFormatter clientFormatter;
            private DispatchOperation dispatchOperation;
            private ClientOperation clientOperation;

            public TraceMessageSizeFormatter(                
                DispatchOperation dispatchOperation)
            {
                this.dispatchOperation = dispatchOperation;
                this.innerFormatter = dispatchOperation.Formatter;
            }

            public TraceMessageSizeFormatter(
                ClientOperation clientOperation)
            {
                this.clientOperation = clientOperation;
                this.clientFormatter = clientOperation.Formatter;                
            }

            #region IDispatchMessageFormatter Members
            
            public void DeserializeRequest(Message message, 
                object[] parameters)
            {
                OnDeserialize(message, MessageDirection.Input);
                innerFormatter.DeserializeRequest(message, parameters);
            }

            public Message SerializeReply(MessageVersion messageVersion,
                object[] parameters, object result)
            {
                var reply = innerFormatter.SerializeReply(messageVersion,
                    parameters, result);
                OnSerialize(reply, MessageDirection.Output);
                return reply;
            }

            #endregion

            #region IClientMessageFormatter Members

            public object DeserializeReply(Message message, 
                object[] parameters)
            {
                OnDeserialize(message, MessageDirection.Output);
                return clientFormatter.DeserializeReply(message, parameters);
            }

            public Message SerializeRequest(MessageVersion messageVersion,
                object[] parameters)
            {
                var message = clientFormatter.SerializeRequest(messageVersion,
                    parameters);
                OnSerialize(message, MessageDirection.Input);
                return message;
            }

            #endregion

            private void OnSerialize(Message message, 
                MessageDirection direction)
            {
                var msWriter = MessageSizeWriter.Create(message);
                InitializeMessageWriter(message, msWriter, direction);
            }

            private void OnDeserialize(Message message, 
                MessageDirection direction)
            {
                var msWriter = MessageSizeWriter.Read(message);
                if (msWriter != null)
                {
                    InitializeMessageWriter(message, msWriter, direction);
                    if (!msWriter.IsBoundToLengthMeasurableStream)
                    {
                        msWriter.Write();
                    }
                }
                else
                {
                    if (message.Version != MessageVersion.None)
                    {
                        throw new InvalidOperationException("TraceMessageSizeAttribute cannot be used " + 
                            "without setting up MessageSizeDetectionEncoder.");
                    }
                }
            }

            private void InitializeMessageWriter(Message message,
                MessageSizeWriter msWriter, MessageDirection direction)
            {
                msWriter.Action = message.Headers.Action;
                msWriter.Direction = direction;
                if (dispatchOperation != null)
                {
                    var channelDispatcher = 
                        dispatchOperation.Parent.ChannelDispatcher;
                    msWriter.ListenerAddress = channelDispatcher.Listener.Uri;                        
                    msWriter.OperationName = dispatchOperation.Name;
                }
                else
                {
                    msWriter.OperationName = clientOperation.Name;
                }
            }
        }

        #region IOperationBehavior Members

        public void AddBindingParameters(
            OperationDescription operationDescription,
            BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(
            OperationDescription operationDescription,
            ClientOperation clientOperation)
        {                      
            if (clientOperation != null)
            {
                this.clientOperations.Add(clientOperation);
            }
            else
            {
                foreach (var op in clientOperations)
                {
                    op.Formatter = new TraceMessageSizeFormatter(op);
                }                
            }
        }

        public void ApplyDispatchBehavior(
            OperationDescription operationDescription,
            DispatchOperation dispatchOperation)
        {
            if (dispatchOperation != null)
            {
                this.dispatchOperations.Add(dispatchOperation);
            }
            else
            {
                foreach (var op in dispatchOperations)
                {
                    op.Formatter = new TraceMessageSizeFormatter(op);                    
                }
            }
        }

        public void Validate(OperationDescription operationDescription)
        {
        }

        #endregion       
    }
}
