/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Thinktecture.ServiceModel.Tests
{
    /// <summary>
    /// Contains the unit tests for <see cref="Thinktecture.ServiceModel.CommunicationObjectHelper"/>.
    /// </summary>
    [TestClass]
    public class CommunicationObjectStateTests
    {
        public CommunicationObjectStateTests()
        {
        }

        /// <summary>
        /// Tests if a <see cref="CommunicationObject"/> is properly closed
        /// during a normal usage scenario (No exceptions are occured).
        /// </summary>
        [TestMethod]
        public void TestEnsureCleanupUnderNormalCircumstances()
        {
            // Create a CommunicationObject.
            TestCommunicationObject commObj = new TestCommunicationObject();

            // Open and use it.
            commObj.Open();
            CommunicationObjectHelper.EnsureCleanup(commObj,
                delegate { commObj.DoSomeWork(); }
                );

            Assert.AreEqual<CommunicationState>(CommunicationState.Closed, commObj.State,
                                                "Communication object is in an inconsistant state.");
        }

        /// <summary>
        /// Tests if a <see cref="CommunicationObject"/> is properly closed
        /// if it becomes Faulted due to an exception occurs while performing the delegated action.
        /// </summary>
        [TestMethod]
        public void TestEnsureCleanupWhenCommunicationObjectBecomesFaultedDueToAction()
        {
            // Create a CommunicationObject.
            TestCommunicationObject commObj = new TestCommunicationObject();

            // Open and use it.
            commObj.Open();
            CommunicationObjectHelper.EnsureCleanup(commObj,
                delegate
                    {
                        // Do some erroneous work that will make the CommunicationObject
                        // Faulted.
                        commObj.DoSomeErroneousWork();
                    }
                );

            Assert.AreEqual<CommunicationState>(CommunicationState.Closed, commObj.State,
                                                "Communication object is in an inconsistant state.");
        }

        /// <summary>
        /// Tests if a <see cref="CommunicationObject"/> is properly closed
        /// if a CommunicationException is thrown during a call to Close method.
        /// </summary>
        [TestMethod]
        public void TestEnsureCleanupWhenCommunicationObjectThrowsCommunicationExceptionOnClose()
        {
            // Create a CommunicationObject that will throw a CommunicationException while being closed.
            ErrorOnCloseCommunicationObject commObj = new ErrorOnCloseCommunicationObject();

            // Open and use it.
            commObj.Open();
            try
            {
                CommunicationObjectHelper.EnsureCleanup(commObj,
                    delegate { commObj.DoSomeWork(); }
                    );
            }
            catch (Exception)
            {
                // Something happend. May be we should log it here. 
                // However, it's not necessary to take the 
                // appropriate actions to close the communication object.
            }

            Assert.AreEqual<CommunicationState>(CommunicationState.Closed, commObj.State,
                                                "Communication object is in an inconsistant state.");
        }

        /// <summary>
        /// Tests <see cref="CommunicationObjectHelper.EnsureCleanup"/> for null input handling.
        /// </summary>
        [TestMethod]
        public void TestEnsureCleanupForNullInputHandling()
        {
            // Create a CommunicationObject.
            TestCommunicationObject commObj = new TestCommunicationObject();

            // First try to pass null to action delegate and see 
            // if it results in an ArgumentException.
            try
            {
                CommunicationObjectHelper.EnsureCleanup(commObj, null);
                Assert.Fail("EnsureCleanup was called with null parameters.");
            }
            catch (ArgumentException)
            {
            }

            // Then try to pass null to both parameters and see 
            // if it results in an ArgumentException.
            try
            {
                CommunicationObjectHelper.EnsureCleanup(null, null);
            }
            catch (ArgumentException)
            {
            }
        }
    }

    /// <summary>
    /// This is a mock communication object used for testing.
    /// </summary>
    public class TestCommunicationObject : CommunicationObject
    {
        protected override TimeSpan DefaultCloseTimeout
        {
            get { return TimeSpan.FromMinutes(1); }
        }

        protected override TimeSpan DefaultOpenTimeout
        {
            get { return TimeSpan.FromMinutes(1); }
        }

        protected override void OnAbort()
        {
        }

        protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return null;
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return null;
        }

        protected override void OnClose(TimeSpan timeout)
        {
        }

        protected override void OnEndClose(IAsyncResult result)
        {
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
        }

        protected override void OnOpen(TimeSpan timeout)
        {
        }

        internal void DoSomeWork()
        {
        }

        internal void DoSomeErroneousWork()
        {
            // Bring the state to Faulted.
            base.Fault();
        }
    }

    /// <summary>
    /// This is a mock communication object used for testing.
    /// </summary>
    public class ErrorOnCloseCommunicationObject : CommunicationObject
    {
        protected override TimeSpan DefaultCloseTimeout
        {
            get { return TimeSpan.FromMinutes(1); }
        }

        protected override TimeSpan DefaultOpenTimeout
        {
            get { return TimeSpan.FromMinutes(1); }
        }

        protected override void OnAbort()
        {
        }

        protected override IAsyncResult OnBeginClose(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return null;
        }

        protected override IAsyncResult OnBeginOpen(TimeSpan timeout, AsyncCallback callback, object state)
        {
            return null;
        }

        protected override void OnClose(TimeSpan timeout)
        {
            throw new CommunicationException("This is an automatically produced communication exception.");
        }

        protected override void OnEndClose(IAsyncResult result)
        {
        }

        protected override void OnEndOpen(IAsyncResult result)
        {
        }

        protected override void OnOpen(TimeSpan timeout)
        {
        }

        internal void DoSomeWork()
        {
        }
    }
}