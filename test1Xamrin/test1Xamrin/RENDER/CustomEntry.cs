using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace xUtilityPCL
{

    public class CustomEntry : Entry
    {
        /// <summary>
        /// The font property
        /// </summary>
        public static readonly BindableProperty FontProperty =
            BindableProperty.Create("Font", typeof(Font), typeof(CustomEntry), new Font());

        /// <summary>
        /// The XAlign property
        /// </summary>
        public static readonly BindableProperty XAlignProperty =
            BindableProperty.Create("XAlign", typeof(TextAlignment), typeof(CustomEntry),
                TextAlignment.Start);

        /// <summary>
        /// The HasBorder property
        /// </summary>
        public static readonly BindableProperty HasBorderProperty =
            BindableProperty.Create("HasBorder", typeof(bool), typeof(CustomEntry), true);

        //20180504 inizio
        public static readonly BindableProperty IsLineEntryProperty =
            BindableProperty.Create("IsLineEntry", typeof(bool), typeof(CustomEntry), false);
        public static readonly BindableProperty LineEntryColorProperty =
        BindableProperty.Create("LineEntryColor", typeof(Color), typeof(CustomEntry), Color.DarkGray);
        //20180504 fine


        /// <summary>
        /// The PlaceholderTextColor property
        /// </summary>
        public static readonly BindableProperty PlaceholderTextColorProperty =
            BindableProperty.Create("PlaceholderTextColor", typeof(Color), typeof(CustomEntry), Color.Default);

        /// <summary>
        /// Gets or sets the Font
        /// </summary>
        public Font Font
        {
            get { return (Font)GetValue(FontProperty); }
            set { SetValue(FontProperty, value); }
        }

        /// <summary>
        /// Gets or sets the X alignment of the text
        /// </summary>
        public TextAlignment XAlign
        {
            get { return (TextAlignment)GetValue(XAlignProperty); }
            set { SetValue(XAlignProperty, value); }
        }

        /// <summary>
        /// Gets or sets if the border should be shown or not
        /// </summary>
        public bool HasBorder
        {
            get { return (bool)GetValue(HasBorderProperty); }
            set { SetValue(HasBorderProperty, value); }
        }

        //20180504 inizio
        public bool IsLineEntry
        {
            get { return (bool)GetValue(IsLineEntryProperty); }
            set { SetValue(IsLineEntryProperty, value); }
        }
        public Color LineEntryColor
        {
            get { return (Color)GetValue(LineEntryColorProperty); }
            set { SetValue(LineEntryColorProperty, value); }
        }
        //20180504 fine

        /// <summary>
        /// Sets color for placeholder text
        /// </summary>
        public Color PlaceholderTextColor
        {
            get { return (Color)GetValue(PlaceholderTextColorProperty); }
            set { SetValue(PlaceholderTextColorProperty, value); }
        }

        public EventHandler LeftSwipe;
        public EventHandler RightSwipe;

        public void OnLeftSwipe(object sender, EventArgs e)
        {
            var handler = this.LeftSwipe;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void OnRightSwipe(object sender, EventArgs e)
        {
            var handler = this.RightSwipe;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }


}
