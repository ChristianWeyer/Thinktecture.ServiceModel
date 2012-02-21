/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.IO;
using System.ServiceModel.Channels;

namespace Thinktecture.ServiceModel.Tracing
{
    internal class MessageSizeDetectionEncoder : MessageEncoder
    {
        private readonly MessageEncoder innerEncoder;        

        public MessageSizeDetectionEncoder(MessageEncoder innerEncoder)
        {
            this.innerEncoder = innerEncoder;
        }

        public override string ContentType
        {
            get { return innerEncoder.ContentType; }
        }

        public override string MediaType
        {
            get { return innerEncoder.MediaType; }
        }

        public override MessageVersion MessageVersion
        {
            get { return innerEncoder.MessageVersion; }
        }

        public override T GetProperty<T>()
        {
            return innerEncoder.GetProperty<T>();
        }

        public override bool IsContentTypeSupported(string contentType)
        {
            return innerEncoder.IsContentTypeSupported(contentType);
        }

        public override Message ReadMessage(ArraySegment<byte> buffer,
            BufferManager bufferManager, string contentType)
        {
            Message message = this.innerEncoder.ReadMessage(buffer,
                bufferManager, contentType);

            var msWriter = MessageSizeWriter.Create(message);
            msWriter.MessageSize = buffer.Count;            
            
            return message;
        }

        public override Message ReadMessage(Stream stream,
            int maxSizeOfHeaders, string contentType)
        {            
            var lmStream = new LengthMeasurableStream(stream);
            var message = this.innerEncoder.ReadMessage(
                lmStream, maxSizeOfHeaders, contentType);
            var msWriter = MessageSizeWriter.Create(message);
            msWriter.BindToLengthMeasurableStream(lmStream);

            return message;
        }

        public override ArraySegment<byte> WriteMessage(Message message,
            int maxMessageSize, BufferManager bufferManager, int messageOffset)
        {
            ArraySegment<byte> buffer = innerEncoder.WriteMessage(message,
                maxMessageSize, bufferManager, messageOffset);
            var msWriter = MessageSizeWriter.Read(message);

            if (msWriter != null)
            {
                msWriter.MessageSize = buffer.Count;
                msWriter.Write();
            }

            return buffer;
        }

        public override void WriteMessage(Message message, Stream stream)
        {
            var target = stream;
            var msWriter = MessageSizeWriter.Read(message);
            
            if (msWriter != null)
            {
                var lmStream = new LengthMeasurableStream(stream);
                msWriter.BindToLengthMeasurableStream(lmStream);
                target = lmStream;
            }            
            
            innerEncoder.WriteMessage(message, target);            
        }
    }
}
