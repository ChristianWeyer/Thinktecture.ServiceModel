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
    /// Contains the unit tests for <see cref="RenewableClientBase"/>.
    /// </summary>
    [TestClass]
    public class RenewableClientBaseTests
    {
        public RenewableClientBaseTests()
        {
        }

        /// <summary>
        /// Tests if the underlying channel is automatically recovered when required.
        /// </summary>
        [TestMethod]
        public void TestChannelAutoRecover()
        {
            // Create an instance of proxy class that derives from RenewableClientBase class.
            GlobalTestServiceClient client = new GlobalTestServiceClient("IGlobalTestService_BasicHttpBinding", true);

            try
            {
                // Try to invoke an erroneous opertaion.
                client.EchoThrowAnException();
            }
            catch
            {
                // Even if an exception occurs the proxy should recover the underlying channel and its 
                // state should be opened.
                Assert.AreEqual<CommunicationState>(CommunicationState.Opened, client.State,
                                                    "Proxy instance is not recovered properly.");
            }
        }
    }
}