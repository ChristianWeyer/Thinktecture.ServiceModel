/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.ServiceModel.Channels;

namespace Thinktecture.ServiceModel.Tracing
{
    internal class MessageSizeDetectionEncoderFactory : MessageEncoderFactory
    {
        private readonly MessageSizeDetectionEncoder encoder;
        private readonly MessageEncoderFactory innerFactory;
        
        public MessageSizeDetectionEncoderFactory(
            MessageEncoderFactory innerFactory)
        {
            this.innerFactory = innerFactory;
            this.encoder = new MessageSizeDetectionEncoder(
                innerFactory.Encoder);
        }

        public override MessageEncoder Encoder
        {
            get { return this.encoder; }
        }

        public override MessageVersion MessageVersion
        {
            get { return this.innerFactory.MessageVersion; }
        }

        public override MessageEncoder CreateSessionEncoder()
        {
            return new MessageSizeDetectionEncoder(
                innerFactory.CreateSessionEncoder());
        }
    }
}
