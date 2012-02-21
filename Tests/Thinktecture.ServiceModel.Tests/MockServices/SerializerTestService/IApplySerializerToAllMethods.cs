/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.Collections.Generic;
using System.ServiceModel;
using Thinktecture.ServiceModel.Description;

namespace Thinktecture.ServiceModel.Tests
{
    [ServiceContract(ConfigurationName="IApplySerializerToAllMethods")]
    [NetDataContractSerializerFormat]
    internal interface IApplySerializerToAllMethods
    {
        [OperationContract]
        void DoSomething();

        [OperationContract]
        void DoSomethingElse();

        [OperationContract]
        IList<string> GetNames();
    }
}