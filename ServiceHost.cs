/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Description;
using Thinktecture.ServiceModel.Description;
using Thinktecture.ServiceModel.Tracing;

namespace Thinktecture.ServiceModel
{
    /// <summary>
    /// Custom ServiceHost for attaching the extensions responsible for flattening the WSDL
    /// and applying the optimized intranet hosting settings.
    /// </summary>
    public class ServiceHost : System.ServiceModel.ServiceHost
    {
        private bool alreadyInitialized;
        private bool useFlatWsdl;
        private Profile usageProfile;
        private IList<IBootstrapTask> bootstrapTasks;

        public IList<IBootstrapTask> BootstrapTasks
        {
            get
            {
                if (bootstrapTasks == null)
                {
                    bootstrapTasks = new List<IBootstrapTask>();
                }

                return bootstrapTasks;
            }            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHost"/> class.
        /// </summary>
        protected ServiceHost()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHost"/> class.
        /// </summary>
        public ServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHost"/> class.
        /// </summary>
        public ServiceHost(object singeltonInstance, params Uri[] baseAddresses)
            : base(singeltonInstance, baseAddresses)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHost"/> class.
        /// </summary>
        public ServiceHost(Type serviceType, Profile profile, Flattening enableFlatWsdl, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            useFlatWsdl = enableFlatWsdl == Flattening.Enabled ? true : false;         
            usageProfile = profile;

            CheckConfig();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHost"/> class.
        /// </summary>
        public ServiceHost(object singeltonInstance, Profile profile, Flattening enableFlatWsdl, params Uri[] baseAddresses)
            : base(singeltonInstance, baseAddresses)
        {
            useFlatWsdl = enableFlatWsdl == Flattening.Enabled ? true : false; 
            usageProfile = profile;
        }

        /// <summary>
        /// Return the usage profile.
        /// </summary>       
        public Profile Profile
        {
            get { return usageProfile; }
        }

        /// <summary>
        /// Gets a value indicating whether the flattened WSDL is enabled.
        /// </summary>        
        public bool FlatWsdl
        {
            get { return useFlatWsdl; }
        }

        /// <summary>
        /// Loads the service description information from the configuration file and applies it to the runtime being constructed.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The description of the service hosted is null.</exception>
        protected override void ApplyConfiguration()
        {            
            CheckConfig();
        }

        /// <summary>
        /// Initializes the runtime for the service host.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The description of the service hosted is null.</exception>
        protected override void InitializeRuntime()
        {
            MessageSizeDetectionEncodingBindingElement.Inject(Description);
            ExecuteBootstrapTasks();
            base.InitializeRuntime();
            BindOperationsSecondRound();
        }

        private void ExecuteBootstrapTasks()
        {
            foreach (var task in BootstrapTasks)
            {
                if (task != null)
                {
                    task.Execute();
                }
            }
        }

        /// <summary>
        /// Configures the <see cref="ServiceHost"/> instance appropriately.
        /// </summary>
        private void CheckConfig()
        {
            if (!alreadyInitialized)
            {
                base.ApplyConfiguration();
                alreadyInitialized = true;
            }
            else
            {
                if (usageProfile == Profile.Intranet)
                {
                    ChangeThrottling();
                    ChangeSerializer();
                    ChangeBindings();
                }

                if (useFlatWsdl)
                {
                    AddFlatWsdl();
                }
            }
        }

        /// <summary>
        /// Changes the throttling behavior.
        /// </summary>
        private void ChangeThrottling()
        {
            ServiceThrottlingBehavior throttle = Description.Behaviors.Find<ServiceThrottlingBehavior>();

            if (throttle == null)
            {
                throttle = new ServiceThrottlingBehavior();
                Description.Behaviors.Add(throttle);
            }

            throttle.MaxConcurrentCalls = int.MaxValue;
            throttle.MaxConcurrentSessions = int.MaxValue;
        }

        /// <summary>
        /// Changes the MaxItemsInObjectGraph quota used by the serializer.
        /// </summary>
        private void ChangeSerializer()
        {
            ServiceBehaviorAttribute serviceBehavior = Description.Behaviors.Find<ServiceBehaviorAttribute>();
            Debug.Assert(serviceBehavior != null, "Could not find a reference to the ServiceBehaviorAttribute.");
            serviceBehavior.MaxItemsInObjectGraph = int.MaxValue;
        }

        /// <summary>
        /// Changes the bindings.
        /// </summary>
        private void ChangeBindings()
        {
            foreach (ServiceEndpoint endpoint in Description.Endpoints)
            {
                endpoint.Binding = BindingController.IncreaseBindingQuotas(endpoint.Binding);
            }
        }

        /// <summary>
        /// Attaches the flat WSDL extension.
        /// </summary>
        private void AddFlatWsdl()
        {
            foreach (ServiceEndpoint endpoint in Description.Endpoints)
            {
                endpoint.Behaviors.Add(new FlatWsdl());
            }
        }

        private void BindOperationsSecondRound()
        {
            List<ContractDescription> visited =
                new List<ContractDescription>(
                    Description.Endpoints.Count);

            foreach (var endpoint in Description.Endpoints)
            {
                if (!visited.Contains(endpoint.Contract))
                {
                    foreach (var operation in endpoint.Contract.Operations)
                    {
                        BindOperation(operation);
                    }
                    visited.Add(endpoint.Contract);
                }
            }
        }

        private void BindOperation(OperationDescription operation)
        {
            foreach (var behavior in operation.Behaviors)
            {
                if (behavior is TraceMessageSizeAttribute)
                {
                    if (operation.Messages[0].Direction ==
                        MessageDirection.Input)
                    {
                        behavior.ApplyDispatchBehavior(operation,
                            null);
                    }
                    else
                    {
                        behavior.ApplyClientBehavior(operation,
                            null);
                    }
                }
            }
        }
    }
}