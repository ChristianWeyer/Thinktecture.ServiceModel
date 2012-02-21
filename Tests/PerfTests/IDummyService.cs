/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.ServiceModel;

namespace Thinktecture.ServiceModel.Samples.PerfTests
{
    [ServiceContract(ConfigurationName="IDummyService")]
    internal interface IDummyService
    {
        [OperationContract]
        string DoSomething(string value);
    }
}