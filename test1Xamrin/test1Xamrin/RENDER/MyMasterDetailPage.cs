using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace xUtilityPCL
{
    public class MyMasterDetailPage : FlyoutPage //20180510
    {
        public MyMasterDetailPage()
        {
        }
        //public static readonly BindableProperty DrawerWidthProperty = BindableProperty.Create<MyMasterDetailPage, int>(p => p.DrawerWidth, default(int));
        public static readonly BindableProperty DrawerWidthProperty = BindableProperty.Create(nameof(DrawerWidth),
            typeof(int),
            typeof(MyMasterDetailPage));
        public int DrawerWidth
        {
            get { return (int)GetValue(DrawerWidthProperty); }
            set { SetValue(DrawerWidthProperty, value); }
        }
    }
}
