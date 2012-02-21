/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Diagnostics;

namespace Thinktecture.ServiceModel.Samples.FlatWsdlSample
{
    internal class Host
    {
        private static void Main(string[] args)
        {
            // Use Thinktecture.ServiceModel.ServiceHost to host the ProductCatalog service.
            // IMPORTANT: In order to use Thinktecture.ServiceMode.ServiceHost you have to 
            // make sure that you specify the same XML namespace in the ServiceContractAttribute
            // attribute, ServiceBehaviorAttribute attribute and in the Namespace property
            // in the binding object.
            ServiceHost host = new ServiceHost(
                typeof (ProductCatalog), 
                Profile.Internet, 
                Flattening.Enabled);
            host.Open();

            Console.WriteLine("Service is running...");
            Process.Start("http://localhost:7777/Services?wsdl");
            Console.ReadKey();

            host.Close();
        }
    }
}