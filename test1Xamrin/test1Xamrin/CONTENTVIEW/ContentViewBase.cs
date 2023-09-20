using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

//https://forums.xamarin.com/discussion/124874/access-to-viewmodel-from-viewcell-in-separated-code
//https://forums.xamarin.com/discussion/71462/datatemplate-with-templateselector-bind-to-parent-command
namespace xUtilityPCL
{
    public class ContentViewBase : ContentView //20180529
    {
        public static BindableProperty ParentBindingContextProperty = BindableProperty.Create(nameof(ParentBindingContext), typeof(object),
                                                                                              typeof(ContentViewBase), null);

        public object ParentBindingContext
        {
            get { return GetValue(ParentBindingContextProperty); }
            set
            {
                SetValue(ParentBindingContextProperty, value);
            }
        }
    }
}


