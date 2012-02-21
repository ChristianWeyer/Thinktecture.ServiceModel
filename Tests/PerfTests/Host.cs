/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Thinktecture.ServiceModel.Samples.PerfTests
{
    public class Host
    {
        private static ServiceHost sh = new ServiceHost(typeof (DummyService), 
            new Uri("http://localhost:7777/Services/"),
            new Uri("net.tcp://localhost:7778/Services/"));

        public static void Start()
        {
            sh.AddServiceEndpoint(typeof (IDummyService),
                                  new BasicHttpBinding(),
                                  "ds");
            sh.AddServiceEndpoint(typeof(IDummyService),
                                  new NetTcpBinding(),
                                  "ds");

            ServiceMetadataBehavior metadata = new ServiceMetadataBehavior();
            metadata.HttpGetEnabled = true;

            sh.Description.Behaviors.Add(metadata);
            sh.Open();
        }

        public static void Stop()
        {
            sh.Close();
        }
    }
}