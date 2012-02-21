/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Diagnostics;
using System.Reflection;
using System.ServiceModel;

namespace Thinktecture.ServiceModel.Samples.PerfTests
{
    /// <summary>
    /// This class contains the perf test to to prove that using Thinktecture.ServiceModel.ChannelFactoryManager
    /// instead of svcutil generated proxy can improve the performance.    
    /// </summary>
    internal class ChannelFactoryManagerPerfTest
    {
        string endpointName = "BasicHttpBinding_IDummyService"; 
        //string endpointName = "NetTcpBinding_IDummyService";

        public void Run()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Test results.");
            Console.ResetColor();

            Console.WriteLine("First round:"); 
            
            MeasureTimingToAccessServiceUsingSvcUtilGeneratedClient();
            MeasureTimingToAccessServiceUsingChannelFactoryManager();

            Console.WriteLine("Second round:"); 

            MeasureTimingToAccessServiceUsingSvcUtilGeneratedClient();
            MeasureTimingToAccessServiceUsingChannelFactoryManager();
        }

        private void MeasureTimingToAccessServiceUsingSvcUtilGeneratedClient()
        {
            Console.WriteLine("Measuring time to access the service using svcutil generated client.");
            // Perform the task for 10 times and calculate the time average time.
            Stopwatch sw = new Stopwatch();
            long totalTicks = 0;

            for (int i = 1; i <= 10; i++)
            {
                sw.Start();
                AccessServiceUsingSvcUtilGeneratedClient();
                sw.Stop();
                totalTicks += sw.ElapsedTicks;
                Console.WriteLine("Round:{0} - {1} ticks.", i, sw.ElapsedTicks);
                sw.Reset();
            }

            Console.WriteLine("Avg: {0}", totalTicks/10);
            Console.WriteLine();
        }

        private void MeasureTimingToAccessServiceUsingChannelFactoryManager()
        {
            Console.WriteLine("Measuring time to access the service using channel factory manager.");
            // Perform the task for 10 times and calculate the time average time.
            Stopwatch sw = new Stopwatch();
            long totalTicks = 0;

            for (int i = 1; i <= 10; i++)
            {
                sw.Start();
                AccessServiceUsingChannalFactoryManager();
                sw.Stop();
                totalTicks += sw.ElapsedTicks;
                Console.WriteLine("Round:{0} - {1} ticks.", i, sw.ElapsedTicks);
                sw.Reset();
            }

            Console.WriteLine("Avg: {0}", totalTicks/10);
            Console.WriteLine();
        }

        private void AccessServiceUsingSvcUtilGeneratedClient()
        {
            SvcUtilGeneratedClient client = new SvcUtilGeneratedClient(endpointName);
            string r = client.DoSomething("");
            client.Close();
        }

        private void AccessServiceUsingChannalFactoryManager()
        {
            IDummyService channel = ChannelFactoryManager<IDummyService>.GetChannel(endpointName);
            string r = channel.DoSomething("");
            ((IClientChannel) channel).Close();
        }
    }
}