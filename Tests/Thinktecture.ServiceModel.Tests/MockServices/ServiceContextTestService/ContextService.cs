/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.ServiceModel;

namespace Thinktecture.ServiceModel.Tests
{
    [ServiceBehavior(
        InstanceContextMode=InstanceContextMode.Single,
        ConfigurationName="ContextService",
        IncludeExceptionDetailInFaults = true)]
    internal class ContextService : IContextService
    {
        public void DoSomething()
        {
            // Add some contextual information to the store.
            ServiceContextStore.Current.Add("MyContextualInformation", "value");
            // Now invoke the business facade which in turn consumes that information.
            IBusinessObject businessObject = new BusinessObject();
            businessObject.DoBusiness();
        }
    }
}