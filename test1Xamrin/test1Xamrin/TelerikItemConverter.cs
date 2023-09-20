using System;
using System.Collections.Generic;
using System.Text;
using Telerik.XamarinForms.ConversationalUI;

namespace test1Xamrin
{
    public class TelerikItemConverter : IChatItemConverter
    {
        //from datarow to control
        public ChatItem ConvertToChatItem(object dataItem, ChatItemConverterContext context)
        {
            //SimpleChatItem item = (SimpleChatItem)dataItem;
            //TextMessage textMessage = new TextMessage()
            //{
            //    Data = dataItem,
            //    Author = item.Author,
            //    Text = item.Text
            //};
            //return textMessage;
            return dataItem as ChatItem;
        }
        public object ConvertToDataItem(object message, ChatItemConverterContext context)
        {
            //ViewModel vm = (ViewModel)context.Chat.BindingContext;
            //return new SimpleChatItem { Author = vm.Me, Text = (string)message, Category = MessageCategory.Normal };
            //return new TextMessage() { da Author = vm.Me, Text = (string)message, Category = MessageCategory.Normal };
            return null;
        }
    }
}
