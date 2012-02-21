/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.ServiceModel;

namespace Thinktecture.ServiceModel.Tests
{
    [ServiceBehavior(ConfigurationName="GlobalTestService")]
    internal class GlobalTestService : IGlobalTestService
    {
        public string Echo(string input)
        {
            return input;
        }

        public void EchoThrowAnException()
        {
            throw new FaultException("This exception is intentionally thrown for testing purposes.");
        }
    }
}