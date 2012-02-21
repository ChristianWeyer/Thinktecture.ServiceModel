/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thinktecture.ServiceModel.Description;

namespace Thinktecture.ServiceModel.Tests
{
    /// <summary>
    /// Contains the unit tests for NetDataContractSerializerOperationBehavior.
    /// </summary>
    [TestClass]
    public class NetDataContractSerializerTest
    {
        public NetDataContractSerializerTest()
        {
        }

        /// <summary>
        /// Tests if the NetDataContractSerializer is used for every operation in a contract
        /// when the CLR interface defining the contract is marked with <see cref="NetDataContractSerializerFormatAttribute"/>
        /// attribute.
        /// </summary>
        [TestMethod]
        public void TestIfTheNetDataContractSerializerIsAppliedToAllMethods()
        {
            // Create and open the service host.
            ServiceHost host = new ServiceHost(typeof (ApplySerializerToAllMethods));
            host.Open();

            foreach (ServiceEndpoint ep in host.Description.Endpoints)
            {
                foreach (OperationDescription op in ep.Contract.Operations)
                {
                    if (op.Behaviors.Find<NetDataContractSerializerOperationBehavior>() == null)
                    {
                        Assert.Fail("This service has an operation which does not use the NetDataContractSerializer.");
                    }
                }
            }

            host.Close();
        }

        /// <summary>
        /// Tests if the NetDataContractSerializer is used for individual operations in a contract
        /// when only desired operations are marked with <see cref="NetDataContractSerializerFormatAttribute"/>
        /// attribute.
        /// </summary>
        [TestMethod]
        public void TestIfTheNetDataContractSerializerIsAppliedToSingleMethod()
        {
            // Create and open the service host.
            ServiceHost host = new ServiceHost(typeof (ApplySerializerToSingleMethod));
            host.Open();

            foreach (ServiceEndpoint ep in host.Description.Endpoints)
            {
                // First check whether the NDCS is not available in the first operation.
                OperationDescription op = ep.Contract.Operations[0];

                if (op.Behaviors.Find<NetDataContractSerializerOperationBehavior>() != null)
                {
                    Assert.Fail("NetDataContractSerializer is added to an un-intended operation: {0}.", op.Name);
                }

                // Then check whether the NDCS is available in the second operation.
                op = ep.Contract.Operations[1];

                if (op.Behaviors.Find<NetDataContractSerializerOperationBehavior>() == null)
                {
                    Assert.Fail("NetDataContractSerializer is not added to an intended operation: {0}.", op.Name);
                }
            }
            host.Close();
        }

        /// <summary>
        /// Tests if the type information is properly added to the serialized data.
        /// </summary>
        [TestMethod]
        public void TestIfTheTypeInfoIsProperlySerialized()
        {
            // Create and open the service host.
            ServiceHost host = new ServiceHost(typeof (ApplySerializerToAllMethods));
            host.Open();

            // Create the client side channel and invoke the service method.
            ChannelFactory<IApplySerializerToAllMethods> cf =
                new ChannelFactory<IApplySerializerToAllMethods>("IApplySerializerToAllMethods_BasicHttpBinding");
            IApplySerializerToAllMethods client = cf.CreateChannel();
            IList<string> names = client.GetNames();
            Assert.IsTrue(names.GetType() == typeof (List<string>),
                          "Type information was properly rendered to the client.");
            ((IClientChannel) client).Close();
            host.Close();
        }
    }
}