using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using Thinktecture.ServiceModel;
using Thinktecture.ServiceModel.Tests;
using System.ServiceModel.Description;

namespace Thinktecture.ServiceModelTests
{
    /// <summary>
    /// Summary description for ExtensionsTest
    /// </summary>
    [TestClass]
    public class ExtensionsTest
    {
        public ExtensionsTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestAddOrReplaceServiceBehavior()
        {
            var host = new System.ServiceModel.ServiceHost(typeof(GeneralTestService));
            host.AddServiceEndpoint(typeof(IGeneralTestService), new BasicHttpBinding(), new Uri("http://localhost:7777/services/extensions"));

            var mdb = new ServiceMetadataBehavior();
            mdb.HttpGetEnabled = true;
            host.AddOrReplaceBehavior<ServiceMetadataBehavior>(mdb);
        }
    }
}
