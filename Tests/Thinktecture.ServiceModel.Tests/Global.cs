/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Thinktecture.ServiceModel.Tests
{
    [TestClass]
    public class Global
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            GlobalTestServiceHost.EnsureStarted();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            GlobalTestServiceHost.Shutdown();
        }
    }
}