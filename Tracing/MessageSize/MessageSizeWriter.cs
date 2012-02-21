/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Diagnostics;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Thinktecture.ServiceModel.Tracing
{
    internal class MessageSizeWriter
    {
        public const string Name = "Thinktecture.ServiceModel." +
            "MessageSizeWriter";

        private long messageSize;
        private string action;
        private string operationName;
        private Uri listenerAddress;
        private MessageDirection direction;
        private bool isBoundToLengthMeasurableStream;

        private MessageSizeWriter()
        {
        }

        public long MessageSize
        {
            get { return messageSize; }
            set { messageSize = value; }
        }

        public string Action
        {
            get { return action; }
            set { action = value; }
        }

        public string OperationName
        {
            get { return operationName; }
            set { operationName = value; }
        }

        public Uri ListenerAddress
        {
            get { return listenerAddress; }
            set { listenerAddress = value; }
        }

        public MessageDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public bool IsReady
        {
            get
            {
                return (messageSize > 0 && !string.IsNullOrEmpty(action) &&
                    !string.IsNullOrEmpty(operationName));
            }
        }

        public bool IsBoundToLengthMeasurableStream
        {
            get { return isBoundToLengthMeasurableStream; }
        }

        public void Write()
        {
            if (IsReady)
            {
                string message = string.Format("Tracing the size of {0} " +
                    "message\r\n" +
                    "Date/Time(Endpoint local): {2}\r\n" +
                    "Action: {3}\r\n" +
                    "Operation name: {4}\r\n" +
                    "Endpoint address: {5}\r\n" +
                    "Message size (excluding transport level framing): {6} " +
                    "bytes\r\n",
                    direction, DateTime.UtcNow, DateTime.Now, action,
                    operationName, listenerAddress, messageSize
                    );

                Thinktecture.ServiceModel.Utility.Trace.Information(
                    "Thinktecture.MessageSize",
                    message);
            }
            else
            {
                throw new InvalidOperationException("Cannot trace message " +
                    "without setting all required properties. size");
            }
        }

        public void BindToLengthMeasurableStream(
            LengthMeasurableStream lmStream)
        {
            lmStream.MessageSizeWriter = this;
            isBoundToLengthMeasurableStream = true;
        }

        public static MessageSizeWriter Create(Message message)
        {
            Debug.Assert(!message.Properties.ContainsKey(Name),
                "Cannot create the MessageSizeWriter for the same message " + 
                "twice");

            var msWriter = new MessageSizeWriter();
            message.Properties.Add(Name, msWriter);
            return msWriter;
        }

        public static MessageSizeWriter Read(Message message)
        {
            if (message.Properties.ContainsKey(Name))
            {
                return (MessageSizeWriter)message.Properties[Name];
            }
            return null;
        }
    }
}