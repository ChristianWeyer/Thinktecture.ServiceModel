/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.ServiceModel;

namespace Thinktecture.ServiceModel.Tests
{
    internal class GlobalTestServiceHost
    {
        private static ServiceHost host;

        public static void EnsureStarted()
        {
            if (host != null)
            {
                return;
            }

            try
            {
                host = new ServiceHost(typeof (GlobalTestService));
                host.Open();
            }
            catch (CommunicationException)
            {
                host.Abort();
            }
            catch
            {
                TryCloseHost();
                throw;
            }
        }

        public static void Shutdown()
        {
            if (host != null)
            {
                try
                {
                    host.Close();
                }
                catch (CommunicationException)
                {
                    host.Abort();
                }
                catch
                {
                    TryCloseHost();
                    throw;
                }
            }
        }

        private static void TryCloseHost()
        {
            if (host != null)
            {
                try
                {
                    host.Close();
                }
                catch
                {
                    host.Abort();
                }
            }
        }
    }
}