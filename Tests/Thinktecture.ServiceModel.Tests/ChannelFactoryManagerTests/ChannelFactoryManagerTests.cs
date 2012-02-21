/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.Reflection;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Thinktecture.ServiceModel.Tests
{
    /// <summary>
    /// Contains the unit tests for <see cref="ChannelFactoryManager<T>"/>.
    /// </summary>
    [TestClass]
    public class ChannelFactoryManagerTests
    {
        public ChannelFactoryManagerTests()
        {
        }

        /// <summary>
        /// Tests if a single instance of <see cref="ChannelFactory"/> is reused by 
        /// <see cref="ChannelFactoryManager<T>" /> for creating multiple channels to the
        /// same endpoint.
        /// </summary>
        [TestMethod]
        public void TestIfTheChanelFactoryIsReusedInChannelFactoryManager()
        {
            // First create and close a channel.
            IGlobalTestService channel =
                ChannelFactoryManager<IGlobalTestService>.GetChannel("IGlobalTestService_BasicHttpBinding");
            ((IClientChannel) channel).Close();

            // Take a reference to the underlying ChannelFactory instance through reflection.
            FieldInfo fieldInfo =
                typeof (ChannelFactoryManager<IGlobalTestService>).GetField("factory",
                                                                            BindingFlags.Static | BindingFlags.NonPublic);
            System.ServiceModel.ChannelFactory<IGlobalTestService> underlyingChannelFactory =
                fieldInfo.GetValue(null) as System.ServiceModel.ChannelFactory<IGlobalTestService>;

            // Create and close another channel.
            channel = ChannelFactoryManager<IGlobalTestService>.GetChannel("IGlobalTestService_BasicHttpBinding");
            ((IClientChannel) channel).Close();

            // Now make sure both underlying ChannelFactory instances pointed by the fieldInfo and underlyingChannelFactory variables 
            // are same.
            Assert.AreSame(fieldInfo.GetValue(null), underlyingChannelFactory,
                           "Channel factory is not reused by ChannelFactoryManager");
        }
    }
}