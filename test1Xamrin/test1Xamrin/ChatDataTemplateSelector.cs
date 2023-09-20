using System;
using System.Collections.Generic;
using System.Text;
using test1Xamrin.Framework;
using Xamarin.Forms;

namespace test1Xamrin
{
    public class ChatDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ServerTemplate { get; set; }

        public DataTemplate ClientTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var model = item as Message;

            switch (model.MessageType)
            {
                case MessageType.Sent:
                    return ClientTemplate;
                case MessageType.Received:
                default:
                    return ServerTemplate;
            }
        }
    }
}
