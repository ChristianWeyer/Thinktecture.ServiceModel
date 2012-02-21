/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Thinktecture.ServiceModel.Tracing
{
    internal class MessageSizeDetectionEncodingBindingElement :
            MessageEncodingBindingElement
    {
        private MessageEncodingBindingElement innerBindingElement;
        
        private MessageSizeDetectionEncodingBindingElement()
        {            
        }

        private MessageSizeDetectionEncodingBindingElement(
            MessageSizeDetectionEncodingBindingElement elementToBeCloned)
            : base(elementToBeCloned)
        {
            this.innerBindingElement = elementToBeCloned.innerBindingElement;            
        }

        public override MessageVersion MessageVersion
        {
            get { return innerBindingElement.MessageVersion; }
            set { innerBindingElement.MessageVersion = value; }
        }

        public override IChannelFactory<TChannel>
            BuildChannelFactory<TChannel>(BindingContext context)
        {
            context.BindingParameters.Add(this);
            return base.BuildChannelFactory<TChannel>(context);
        }

        public override IChannelListener<TChannel>
            BuildChannelListener<TChannel>(BindingContext context)
        {
            context.BindingParameters.Add(this);
            return base.BuildChannelListener<TChannel>(context);
        }

        public override bool CanBuildChannelFactory<TChannel>(
            BindingContext context)
        {
            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelFactory<TChannel>();
        }

        public override bool CanBuildChannelListener<TChannel>(
            BindingContext context)
        {
            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelListener<TChannel>();
        }

        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            return new MessageSizeDetectionEncoderFactory(
                innerBindingElement.CreateMessageEncoderFactory());
        }

        public override BindingElement Clone()
        {
            return new MessageSizeDetectionEncodingBindingElement(this);
        }

        public override T GetProperty<T>(BindingContext context)
        {
            return innerBindingElement.GetProperty<T>(context) ??
                context.GetInnerProperty<T>();
        }

        public static void Inject(ServiceDescription description)
        {
            foreach (var endpoint in description.Endpoints)
            {
                var originalElements =
                    endpoint.Binding.CreateBindingElements();
                var originalEncodingElement =
                    originalElements.Find<MessageEncodingBindingElement>();
                var newEncodingElement = 
                    new MessageSizeDetectionEncodingBindingElement();

                newEncodingElement.innerBindingElement =
                    originalEncodingElement;

                var newBinding = new CustomBinding();
                newBinding.CloseTimeout = endpoint.Binding.CloseTimeout;
                newBinding.Name = endpoint.Binding.Name;
                newBinding.Namespace = endpoint.Binding.Namespace;
                newBinding.OpenTimeout = endpoint.Binding.OpenTimeout;
                newBinding.ReceiveTimeout = endpoint.Binding.ReceiveTimeout;
                newBinding.SendTimeout = endpoint.Binding.SendTimeout;

                foreach (var element in originalElements)
                {
                    if (element == originalEncodingElement)
                    {
                        newBinding.Elements.Add(newEncodingElement);
                    }
                    else
                    {
                        newBinding.Elements.Add(element);
                    }
                }
                endpoint.Binding = newBinding;
            }
        }
    }
}
