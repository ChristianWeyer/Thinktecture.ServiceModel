/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Thinktecture.ServiceModel.Tests
{
    /// <summary>
    /// Contains the unit test for <see cref="ServiceContextAttribute"/> attribute
    /// and <see cref="ServiceContextStore"/>.
    /// </summary>
    [TestClass]
    public class ServiceContextTests
    {
        public ServiceContextTests()
        {
        }

        /// <summary>
        /// Tests if the contextual information can be transferred and manipulated via
        /// <see cref="ServiceContextStore"/>.
        /// In this test we simulate a WCF service which uses <see cref="ServiceContextStore"/>
        /// to pass contextual information from the service facade to the business facade.
        /// </summary>
        [TestMethod]
        public void TestServiceContextStoreDataFlow()
        {
            // First start the service.
            ServiceHost host = new ServiceHost(typeof (ContextService));
            host.Open();

            // Now simulate a client call to the service.
            System.ServiceModel.ChannelFactory<IContextService> cf =
                new System.ServiceModel.ChannelFactory<IContextService>("IContextService_BasicHttpBinding");
            IContextService channel = cf.CreateChannel();
            channel.DoSomething();
            ((IClientChannel) channel).Close();

            // Finally shutdown the service.
            host.Close();
        }
    }

    public interface IBusinessObject
    {
        void DoBusiness();
    }

    /// <summary>
    /// This is the business object that uses the contextual information added by the service facade.
    /// </summary>
    public class BusinessObject : IBusinessObject
    {
        public BusinessObject()
        {
        }

        public void DoBusiness()
        {
            // First try to see if the count property is producing the right values.
            Assert.IsTrue(ServiceContextStore.Current.Count == 1,
                          "ServiceContextStore.Count is not producing the actual count.");

            // Try to access the contextual information that is expected to be added by the service facede.
            string ci = ServiceContextStore.Current.Get<string>("MyContextualInformation");

            if (ci != null)
            {
                // Now try to update the contextual data using the indexer.
                ServiceContextStore.Current.Set<string>("MyContextualInformation","updated");
                // Verify the updates.
                Assert.AreNotEqual(ci, ServiceContextStore.Current.Get<string>("MyContextualInformation"),
                                   "Updates are not reflected in the service context store.");
            }
            else
            {
                // Fail the test if the desired information is not available.
                Assert.Fail("Contextual information is not accessible");
            }

            // Now try to remove something and see if the Remove facility is functioning properly.
            ServiceContextStore.Current.Remove("MyContextualInformation");
            ci = ServiceContextStore.Current.Get<string>("MyContextualInformation");

            if (ci != null)
            {
                // Fail the test if the desired information is not available.
                Assert.Fail("Contextual information is not properly deleted.");
            }
        }
    }
}