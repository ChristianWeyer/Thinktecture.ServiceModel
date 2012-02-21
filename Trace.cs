#region Copyright (c) 2011, thinktecture (http://www.thinktecture.com)

/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using SD = System.Diagnostics;

namespace Thinktecture.ServiceModel.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public static class Trace
    {
        /// <summary>
        /// Information trace of the specified message.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="message">The message.</param>
        public static void Information(string traceSource, string message)
        {
            TraceEvent(traceSource, SD.TraceEventType.Information, message);
        }

        /// <summary>
        /// Warning trace of the specified message.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="message">The message.</param>
        public static void Warning(string traceSource, string message)
        {
            TraceEvent(traceSource, SD.TraceEventType.Warning, message);
        }

        /// <summary>
        /// Error trace of the specified message.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="message">The message.</param>
        public static void Error(string traceSource, string message)
        {
            TraceEvent(traceSource, SD.TraceEventType.Error, message);
        }

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="exception">The exception.</param>    
        public static void LogException(string traceSource, Exception exception)
        {
            StringBuilder sb = BuildExceptionTrace(exception);

            Error(traceSource, sb.ToString());
        }

        private static StringBuilder BuildExceptionTrace(Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            Stack<Exception> exceptions = new Stack<Exception>();
            int counter = 0;
            exceptions.Push(exception);

            while (exceptions.Count != 0)
            {
                counter++;
                Exception currentEx = exceptions.Pop();
                sb.AppendLine("Exception: " + counter.ToString(CultureInfo.InvariantCulture));
                sb.AppendLine(currentEx.Message);
                if (currentEx.InnerException != null)
                    exceptions.Push(currentEx.InnerException);
                sb.AppendLine("------------");
                sb.AppendLine("");
            }

            sb.AppendLine("Stack trace");
            sb.AppendLine(exception.StackTrace);
         
            return sb;
        }

        /// <summary>
        /// Traces the event.
        /// </summary>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        public static void TraceEvent(string traceSource, SD.TraceEventType type, string message)
        {
            SD.TraceSource ts = new SD.TraceSource(traceSource);

            if (SD.Trace.CorrelationManager.ActivityId == Guid.Empty)
            {
                SD.Trace.CorrelationManager.ActivityId = Guid.NewGuid();
            }

            ts.TraceEvent(type, 0, message);
        }
    }
}