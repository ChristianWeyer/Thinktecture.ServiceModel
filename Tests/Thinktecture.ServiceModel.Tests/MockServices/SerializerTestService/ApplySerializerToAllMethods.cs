/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.Collections.Generic;
using System.ServiceModel;

namespace Thinktecture.ServiceModel.Tests
{
    [ServiceBehavior(ConfigurationName="ApplySerializerToAllMethods")]
    internal class ApplySerializerToAllMethods : IApplySerializerToAllMethods
    {
        public void DoSomething()
        {
        }

        public void DoSomethingElse()
        {
        }

        public IList<string> GetNames()
        {
            List<string> names = new List<string>();
            names.Add("Christian Weyer");
            names.Add("Ingo Rammer");

            return names;
        }
    }
}