using System;
using System.Collections.Generic;
using System.Text;
using Telerik.XamarinForms.ConversationalUI;
using Xamarin.Forms;

namespace test1Xamrin
{
    public class TelerikItemTemplateSelector : ChatItemTemplateSelector
    {
        public DataTemplate ImportantMessageTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            ChatItem chatItem = item as ChatItem;
            //IMG-Path:

            try
            {

                if ((chatItem as TextMessage).Text.StartsWith("IMG-Path:"))
                {
                    return this.ImportantMessageTemplate;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return base.OnSelectTemplate(item, container);

            //var myItem = chatItem?.Data as SimpleChatItem;
            //if (myItem != null && myItem.Category == MessageCategory.Important)
            //{
            //    return this.ImportantMessageTemplate;
            //}

        }
    }
}
