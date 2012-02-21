/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.ServiceModel;

namespace Thinktecture.ServiceModel.Tests
{
    [ServiceBehavior(ConfigurationName="GeneralTestService")]
    internal class GeneralTestService : IGeneralTestService
    {
        public void DoSomething()
        {
        }
    }
}