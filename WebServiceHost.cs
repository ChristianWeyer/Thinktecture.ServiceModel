/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Collections.Generic;

namespace Thinktecture.ServiceModel
{
    /// <summary>
    /// Custom WebServiceHost with bootstrap task support.
    /// </summary>
    public class WebServiceHost : System.ServiceModel.Web.WebServiceHost
    {
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
        /// Initializes a new instance of the <see cref="WebServiceHost"/> class.
        /// </summary>
        protected WebServiceHost()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceHost"/> class.
        /// </summary>
        public WebServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceHost"/> class.
        /// </summary>
        public WebServiceHost(object singeltonInstance, params Uri[] baseAddresses)
            : base(singeltonInstance, baseAddresses)
        {
        }
                
        /// <summary>
        /// Initializes the runtime for the service host.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The description of the service hosted is null.</exception>
        protected override void InitializeRuntime()
        {            
            base.InitializeRuntime();

            ExecuteBootstrapTasks();
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
    }
}