/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Thinktecture.ServiceModel.Tracing
{
    /// <summary>
    /// 
    /// </summary>
    public class ColorConsoleTraceListener : ConsoleTraceListener
    {
        private Dictionary<TraceEventType, ConsoleColor> eventColor = new Dictionary<TraceEventType, ConsoleColor>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorConsoleTraceListener"/> class.
        /// </summary>
        public ColorConsoleTraceListener()
        {
            eventColor.Add(TraceEventType.Verbose, ConsoleColor.DarkGray);
            eventColor.Add(TraceEventType.Information, ConsoleColor.White);
            eventColor.Add(TraceEventType.Warning, ConsoleColor.Yellow);
            eventColor.Add(TraceEventType.Error, ConsoleColor.Red);
            eventColor.Add(TraceEventType.Critical, ConsoleColor.DarkRed);
            eventColor.Add(TraceEventType.Start, ConsoleColor.DarkCyan);
            eventColor.Add(TraceEventType.Stop, ConsoleColor.DarkCyan);
        }

        /// <summary>
        /// Writes trace information, a message, and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="message">A message to write.</param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            TraceEvent(eventCache, source, eventType, id, "{0}", message);
        }

        /// <summary>
        /// Writes trace information, a formatted array of objects and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="T:System.Diagnostics.TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="T:System.Diagnostics.TraceEventType"/> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="format">A format string that contains zero or more format items, which correspond to objects in the <paramref name="args"/> array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = getEventColor(eventType, originalColor);

            base.TraceEvent(eventCache, source, eventType, id, format, args);

            Console.ForegroundColor = originalColor;
        }

        private ConsoleColor getEventColor(TraceEventType eventType, ConsoleColor defaultColor)
        {
            if (!eventColor.ContainsKey(eventType))
            {
                return defaultColor;
            }

            return eventColor[eventType];
        }
    }
}
