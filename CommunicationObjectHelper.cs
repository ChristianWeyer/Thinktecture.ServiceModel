/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using Thinktecture.ServiceModel.Properties;

namespace Thinktecture.ServiceModel
{
    /// <summary>
    /// Contains the helper methods for managing the <see cref="CommunicationObject"/>'s life time.
    /// </summary>
    public static class CommunicationObjectHelper
    {        
        /// <summary>
        /// Invokes the specified delegate and cleans up the specified channel.
        /// </summary>
        public static void EnsureCleanup(object channel, Action action)
        {
            var commObj = channel as ICommunicationObject;

            if (commObj == null)
            {
                throw new ArgumentException(Resources.NoCommunicationObject);
            }

            if (action == null)
            {
                throw new ArgumentException(Resources.NoValidDelegate);
            }

            try
            {
                action();
            }
            finally
            {
                TryCloseCommunicationObject(commObj);
            }
        }

        /// <summary>
        /// Invokes the specified delegate and retries the service operation call.
        /// </summary>
        /// <param name="action">The action.</param>
        public static void RetryCall(Action action)
        {
            if (ConfigurationManager.AppSettings["retries"] == null)
                throw new ConfigurationErrorsException(Resources.RetriesNotFound);
            if (ConfigurationManager.AppSettings["timeToWait"] == null)
                throw new ConfigurationErrorsException(Resources.TimeToWaitNotFound);

            var retries = Convert.ToInt32(ConfigurationManager.AppSettings["retries"]);
            var timeToWait = Convert.ToInt32(ConfigurationManager.AppSettings["timeToWait"]);

            RetryCall(retries, timeToWait, action);
        }

        /// <summary>
        /// Invokes the specified delegate and retries the service operation call.
        /// </summary>
        /// <param name="retries">The retries.</param>
        /// <param name="action">The action.</param>
        public static void RetryCall(int retries, Action action)
        {
            RetryCall(retries, 0, action);
        }

        /// <summary>
        /// Invokes the specified delegate and retries the service operation call.
        /// </summary>
        /// <param name="retries">The retries.</param>
        /// <param name="timeToWait">The time to wait.</param>
        /// <param name="action">The action.</param>
        public static void RetryCall(int retries, int timeToWait, Action action)
        {
            var currentRetry = 0;
            var success = false;
            var firstCall = true;
            Exception exception = null;

            while (!success && !(currentRetry == retries))
            {
                try
                {
                    if (timeToWait > 0)
                    {
                        if (firstCall == false)
                            Thread.Sleep(timeToWait);
                    }

                    firstCall = false;

                    action();

                    success = true;
                }
                catch (TimeoutException tex)
                {
                    currentRetry++;
                    exception = tex;
                }
                catch (CommunicationException cex)
                {
                    currentRetry++;
                    exception = cex;
                }
            }

            if (currentRetry == retries)
            {
                if (exception != null)
                    throw exception;
            }
        }

        /// <summary>
        /// Tries to gracefully close communication object.
        /// </summary>        
        private static void TryCloseCommunicationObject(ICommunicationObject commObj)
        {
            if (commObj.State == CommunicationState.Faulted)
            {
                commObj.Abort();
            }
            else if (commObj.State != CommunicationState.Closed)
            {
                try
                {
                    commObj.Close();
                }
                catch (CommunicationException)
                {
                    commObj.Abort();
                }
            }
        }
    }
}