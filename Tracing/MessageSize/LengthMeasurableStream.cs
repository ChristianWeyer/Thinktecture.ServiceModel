/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.IO;

namespace Thinktecture.ServiceModel.Tracing
{
    internal class LengthMeasurableStream : Stream
    {
        private readonly Stream innerStream;
        private long bytesTransfered;
        private MessageSizeWriter messageSizeWriter;

        public LengthMeasurableStream(Stream innerStream)
        {
            this.innerStream = innerStream;            
        }
        
        public override bool CanRead
        {
            get { return this.innerStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return this.innerStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return this.innerStream.CanWrite; }
        }

        public override void Close()
        {
            this.innerStream.Close();
            messageSizeWriter.MessageSize = bytesTransfered;
            messageSizeWriter.Write();
        }

        public override void Flush()
        {
            this.innerStream.Flush();
            messageSizeWriter.MessageSize = bytesTransfered;
            messageSizeWriter.Write();
        }

        public override long Length
        {
            get { return this.innerStream.Length; }
        }

        public override long Position
        {
            get { return this.innerStream.Position; }
            set { this.innerStream.Position = value; }
        }

        public long BytesTransfered
        {
            get { return this.bytesTransfered; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = this.innerStream.Read(buffer, offset, count);
            bytesTransfered += bytesRead;
            return bytesRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.innerStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this.innerStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.innerStream.Write(buffer, offset, count);
            bytesTransfered += count;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {                
                innerStream.Dispose();
            }
            base.Dispose(disposing);            
        }

        public MessageSizeWriter MessageSizeWriter
        {
            get { return messageSizeWriter; }
            set { messageSizeWriter = value; }
        }
    }
}
