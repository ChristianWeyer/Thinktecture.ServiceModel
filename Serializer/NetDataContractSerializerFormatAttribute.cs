/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Thinktecture.ServiceModel.Description
{
    /// <summary>
    /// Use this attribute to replace the default <see cref="DataContractSerializer"/> by
    /// <see cref="NetDataContractSerializer"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface)]
    public sealed class NetDataContractSerializerFormatAttribute :
        Attribute, IOperationBehavior, IContractBehavior
    {
        /// <summary>
        /// Configures any binding elements to support the contract behavior.
        /// </summary>
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint,
                                         BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across a contract.
        /// </summary>
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint,
                                        ClientRuntime clientRuntime)
        {
            foreach (OperationDescription od in contractDescription.Operations)
            {
                InjectNetDataContractSerializer(od);
            }
        }

        /// <summary>
        /// Implements a modification or extension of the client across a contract.
        /// </summary>
        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint,
                                          DispatchRuntime dispatchRuntime)
        {
            foreach (OperationDescription od in contractDescription.Operations)
            {
                InjectNetDataContractSerializer(od);
            }
        }

        /// <summary>
        /// Confirms that the contract and endpoint can support the contract behavior.
        /// </summary>
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }

        /// <summary>
        /// Passes data at runtime to bindings to support custom behavior.
        /// </summary>
        public void AddBindingParameters(OperationDescription operationDescription,
                                         BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across an operation.
        /// </summary>
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            InjectNetDataContractSerializer(operationDescription);
        }


        /// <summary>
        /// Implements a modification or extension of the service across an operation.
        /// </summary>
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            InjectNetDataContractSerializer(operationDescription);
        }

        /// <summary>
        /// Confirms that the operation meets some intended criteria.
        /// </summary>
        public void Validate(OperationDescription operationDescription)
        {
        }

        /// <summary>
        /// Injects the net data contract serializer to the runtime.
        /// </summary>        
        private static void InjectNetDataContractSerializer(
            OperationDescription description)
        {
            DataContractSerializerOperationBehavior dcsOperationBehavior =
                description.Behaviors.Find<DataContractSerializerOperationBehavior>();

            if (dcsOperationBehavior != null)
            {
                description.Behaviors.Remove(dcsOperationBehavior);
                NetDataContractSerializerOperationBehavior ndcs =
                    new NetDataContractSerializerOperationBehavior(description);
                ndcs.MaxItemsInObjectGraph = int.MaxValue;
                description.Behaviors.Add(ndcs);
            }
        }
    }
}