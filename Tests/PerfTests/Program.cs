/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;

namespace Thinktecture.ServiceModel.Samples.PerfTests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Host.Start();

            Console.WriteLine("Service is running...");
            Console.WriteLine();

            ChannelFactoryManagerPerfTest cfmTest = new ChannelFactoryManagerPerfTest();
            cfmTest.Run();
            Console.ReadKey();

            Host.Stop();
        }
    }
}