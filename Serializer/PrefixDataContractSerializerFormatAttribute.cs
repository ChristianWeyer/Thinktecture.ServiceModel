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

namespace Thinktecture.ServiceModel.Description
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class PrefixDataContractSerializerFormatAttribute :
        Attribute, IContractBehavior
    {
        /// <summary>
        /// Configures any binding elements to support the contract behavior.
        /// </summary>
        /// <param name="contractDescription">The contract description to modify.</param>
        /// <param name="endpoint">The endpoint to modify.</param>
        /// <param name="bindingParameters">The objects that binding elements require to support the behavior.</param>
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across a contract.
        /// </summary>
        /// <param name="contractDescription">The contract description for which the extension is intended.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="clientRuntime">The client runtime.</param>
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            InjectSerializerWithPrefixes(contractDescription);
        }

        /// <summary>
        /// Implements a modification or extension of the client across a contract.
        /// </summary>
        /// <param name="contractDescription">The contract description to be modified.</param>
        /// <param name="endpoint">The endpoint that exposes the contract.</param>
        /// <param name="dispatchRuntime">The dispatch runtime that controls service execution.</param>
        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            InjectSerializerWithPrefixes(contractDescription);
        }

        private void InjectSerializerWithPrefixes(ContractDescription contractDescription)
        {
            RegisterNamespacePrefixAttribute[] attributes =
                (RegisterNamespacePrefixAttribute[])Attribute.GetCustomAttributes(contractDescription.ContractType, typeof(RegisterNamespacePrefixAttribute));

            foreach (OperationDescription od in contractDescription.Operations)
            {
                InjectSerializer(od, attributes);
            }
        }

        /// <summary>
        /// Implement to confirm that the contract and endpoint can support the contract behavior.
        /// </summary>
        /// <param name="contractDescription">The contract to validate.</param>
        /// <param name="endpoint">The endpoint to validate.</param>
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }

        private static void InjectSerializer(OperationDescription description, RegisterNamespacePrefixAttribute[] attributes)
        {
            PrefixContractSerializerOperationBehavior padcsOperationBehavior =
                description.Behaviors.Find<PrefixContractSerializerOperationBehavior>();

            if (padcsOperationBehavior == null)
            {
                DataContractSerializerOperationBehavior dcsOperationBehavior =
                    description.Behaviors.Find<DataContractSerializerOperationBehavior>();

                if (dcsOperationBehavior != null)
                {
                    description.Behaviors.Remove(dcsOperationBehavior);

                    padcsOperationBehavior = new PrefixContractSerializerOperationBehavior(description, attributes);
                    padcsOperationBehavior.MaxItemsInObjectGraph = int.MaxValue;
                    
                    description.Behaviors.Add(padcsOperationBehavior);
                }
            }            
        }
    }
}
