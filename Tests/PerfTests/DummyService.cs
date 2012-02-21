/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

namespace Thinktecture.ServiceModel.Samples.PerfTests
{
    internal class DummyService : IDummyService
    {
        public string DoSomething(string value)
        {
            return "Hello from dummy service.";
        }
    }
}