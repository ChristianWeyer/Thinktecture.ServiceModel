/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.ServiceModel;
using Thinktecture.ServiceModel.Description;

namespace Thinktecture.ServiceModel.Tests
{
    [ServiceBehavior(ConfigurationName="ApplySerializerToSingleMethod")]
    internal class ApplySerializerToSingleMethod : IApplySerializerToSingleMethod
    {
        public void DoSomething()
        {
        }

        [NetDataContractSerializerFormat]
        public void DoSomethingElse()
        {
        }
    }
}