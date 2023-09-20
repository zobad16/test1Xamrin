using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace xUtilityPCL
{
    public partial class bottoneSabicom : ContentView
    {
        #region "Bindable Properties"
        public static readonly BindableProperty ImageNameButtonProperty =
            BindableProperty.Create("ImageNameButton", typeof(string), typeof(bottoneSabicom), "");

        public string ImageNameButton
        {
            get { return (string)GetValue(ImageNameButtonProperty); }
            set { SetValue(ImageNameButtonProperty, value); }
        }

        public static readonly BindableProperty CircleBorderColorProperty =
            BindableProperty.Create("CircleBorderColor", typeof(Color), typeof(bottoneSabicom), Color.Beige);

        public Color CircleBorderColor
        {
            get { return (Color)GetValue(CircleBorderColorProperty); }
            set { SetValue(CircleBorderColorProperty, value); }
        }

        public static readonly BindableProperty CircleHeightProperty =
            BindableProperty.Create("CircleHeight", typeof(double), typeof(bottoneSabicom), 50.0);

        public double CircleHeight
        {
            get { return (double)GetValue(CircleHeightProperty); }
            set { SetValue(CircleHeightProperty, value); }
        }

        public static readonly BindableProperty CircleWidthProperty =
            BindableProperty.Create("CircleWidth", typeof(double), typeof(bottoneSabicom), 50.0);

        public double CircleWidth
        {
            get { return (double)GetValue(CircleWidthProperty); }
            set { SetValue(CircleWidthProperty, value); }
        }

        public static readonly BindableProperty ButtonFontSizeProperty =
            BindableProperty.Create("ButtonFontSize", typeof(double), typeof(bottoneSabicom), 14.0);

        public double ButtonFontSize
        {
            get { return (double)GetValue(ButtonFontSizeProperty); }
            set { SetValue(ButtonFontSizeProperty, value); }
        }

        public static readonly BindableProperty ButtonHeightProperty =
            BindableProperty.Create("ButtonHeight", typeof(double), typeof(bottoneSabicom), 45.0);

        public double ButtonHeight
        {
            get { return (double)GetValue(ButtonHeightProperty); }
            set { SetValue(ButtonHeightProperty, value); }
        }

        public static readonly BindableProperty ButtonWidthProperty =
            BindableProperty.Create("ButtonWidth", typeof(double), typeof(bottoneSabicom), 150.0);

        public double ButtonWidth
        {
            get { return (double)GetValue(ButtonWidthProperty); }
            set { SetValue(ButtonWidthProperty, value); }
        }

        public static readonly BindableProperty LabelButtonProperty =
            BindableProperty.Create("LabelButton", typeof(string), typeof(bottoneSabicom), "");

        public string LabelButton
        {
            get { return (string)GetValue(LabelButtonProperty); }
            set { SetValue(LabelButtonProperty, value); }
        }

        public static readonly BindableProperty TextColorButtonProperty =
            BindableProperty.Create("TextColorButton", typeof(Color), typeof(bottoneSabicom), Color.Beige);

        public Color TextColorButton
        {
            get { return (Color)GetValue(TextColorButtonProperty); }
            set { SetValue(TextColorButtonProperty, value); }
        }

        public static readonly BindableProperty BorderColorButtonProperty =
            BindableProperty.Create("BorderColorButton", typeof(Color), typeof(bottoneSabicom), Color.Beige);

        public Color BorderColorButton
        {
            get { return (Color)GetValue(BorderColorButtonProperty); }
            set { SetValue(BorderColorButtonProperty, value); }
        }




        #endregion
        public bottoneSabicom()
        {
            InitializeComponent();
        }

        public void Handle_Tapped(object sender, System.EventArgs ev)
        {

        }


        #region "Proprietà normali"
        //ICommand _lblBtnTapCommand;

        //public ICommand lblBtnTapCommand
        //{
        //	get { return _lblBtnTapCommand; }
        //	set { _lblBtnTapCommand = value; }
        //}

        public static readonly BindableProperty lblBtnTapCommandProperty =
            BindableProperty.Create(nameof(lblBtnTapCommand), typeof(ICommand), typeof(bottoneSabicom), null);

        public ICommand lblBtnTapCommand
        {
            get { return (ICommand)GetValue(lblBtnTapCommandProperty); }
            set { SetValue(lblBtnTapCommandProperty, value); }
        }

        #endregion
        //normali proprietà esposte FINE
    }
}