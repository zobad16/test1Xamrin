using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using xUtilityANDROID2;
using xUtilityPCL;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(ExtendedEntryRenderer))]
namespace xUtilityANDROID2
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        public ExtendedEntryRenderer(Context context):base(context)
        {
                
        }
        /// <summary>
        /// The mi n_ distance
        /// </summary>
        private const int MIN_DISTANCE = 10;
        /// <summary>
        /// The _down x
        /// </summary>
        private float _downX, _downY, _upX, _upY;

        private Drawable originalBackground;

        /// <summary>
        /// Called when [element changed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            //20160610 inizio
            if (Control != null)
            {
                var abc = test1Xamrin.Droid.Resource.Color.customEntryColors;


                Control.SetTextColor(Resources.GetColorStateList(test1Xamrin.Droid.Resource.Color.customEntryColors));

                //Control.SetTextColor(Resource.Color.customEntryColors);
                //Control.ImeOptions = global::Android.Views.InputMethods.ImeAction.Search;
                //Control.ImeOptions = (ImeAction)ImeFlags.NoExtractUi;
            }
            //20160610 fine

            //20180504 inizio
            if (originalBackground == null)
                originalBackground = Control.Background;
            //20180504 fine


            var view = (CustomEntry)Element;

            //var cc= (Android.Widget.EditText)Control;


            SetFont(view);
            SetTextAlignment(view);
            SetBorder(view); //20180504
            SetPlaceholderTextColor(view);

            if (e.NewElement == null)
            {
                this.Touch -= HandleTouch;
            }

            if (e.OldElement == null)
            {
                this.Touch += HandleTouch;
            }
        }

        /// <summary>
        /// Handles the touch.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Android.Views.View.TouchEventArgs"/> instance containing the event data.</param>
        void HandleTouch(object sender, Android.Views.View.TouchEventArgs e)
        {
            var element = this.Element as CustomEntry;
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    _downX = e.Event.GetX();
                    _downY = e.Event.GetY();
                    return;
                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                case MotionEventActions.Move:
                    _upX = e.Event.GetX();
                    _upY = e.Event.GetY();

                    float deltaX = _downX - _upX;
                    float deltaY = _downY - _upY;

                    // swipe horizontal?
                    if (Math.Abs(deltaX) > Math.Abs(deltaY))
                    {
                        if (Math.Abs(deltaX) > MIN_DISTANCE)
                        {
                            // left or right
                            if (deltaX < 0)
                            {
                                element.OnRightSwipe(this, EventArgs.Empty);
                                return;
                            }
                            if (deltaX > 0)
                            {
                                element.OnLeftSwipe(this, EventArgs.Empty);
                                return;
                            }
                        }
                        else
                        {
                            Android.Util.Log.Info("ExtendedEntry", "Horizontal Swipe was only " + Math.Abs(deltaX) + " long, need at least " + MIN_DISTANCE);
                            return; // We don't consume the event
                        }
                    }
                    // swipe vertical?
                    //                    else 
                    //                    {
                    //                        if(Math.abs(deltaY) > MIN_DISTANCE){
                    //                            // top or down
                    //                            if(deltaY < 0) { this.onDownSwipe(); return true; }
                    //                            if(deltaY > 0) { this.onUpSwipe(); return true; }
                    //                        }
                    //                        else {
                    //                            Log.i(logTag, "Vertical Swipe was only " + Math.abs(deltaX) + " long, need at least " + MIN_DISTANCE);
                    //                            return false; // We don't consume the event
                    //                        }
                    //                    }

                    return;
            }
        }

        /// <summary>
        /// Handles the <see cref="E:ElementPropertyChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var view = (CustomEntry)Element;

            if (e.PropertyName == CustomEntry.FontProperty.PropertyName)
                SetFont(view);
            if (e.PropertyName == CustomEntry.XAlignProperty.PropertyName)
                SetTextAlignment(view);
            if (e.PropertyName == CustomEntry.IsLineEntryProperty.PropertyName) //20180504
                SetBorder(view);
            if (e.PropertyName == CustomEntry.LineEntryColorProperty.PropertyName) //20180504
                SetBorder(view);
            if (e.PropertyName == CustomEntry.PlaceholderTextColorProperty.PropertyName)
                SetPlaceholderTextColor(view);
        }

        /// <summary>
        /// Sets the border.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetBorder(CustomEntry view)
        {
            //NotCurrentlySupported: HasBorder peroperty not suported on Android
            //20180504 inizio
            //https://stackoverflow.com/questions/38207168/is-it-possible-to-change-the-colour-of-the-line-below-border-of-a-textbox-ent
            if (view.IsLineEntry == true)
            {
                var ccc = view.LineEntryColor.ToAndroid();
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    Control.BackgroundTintList = ColorStateList.ValueOf(ccc);
                    //Control.BackgroundTintMode = PorterDuff.Mode.SrcAtop;
                }
                else
                    Control.Background.SetColorFilter(ccc, PorterDuff.Mode.SrcAtop);
            }
            else
            {
                Control.SetBackground(originalBackground);
            }
            //20180504 fine
        }

        /// <summary>
        /// Sets the text alignment.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetTextAlignment(CustomEntry view)
        {
            switch (view.XAlign)
            {
                case Xamarin.Forms.TextAlignment.Center:
                    Control.Gravity = GravityFlags.CenterHorizontal;
                    break;
                case Xamarin.Forms.TextAlignment.End:
                    Control.Gravity = GravityFlags.End;
                    break;
                case Xamarin.Forms.TextAlignment.Start:
                    Control.Gravity = GravityFlags.Start;
                    break;
            }
        }

        /// <summary>
        /// Sets the font.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetFont(CustomEntry view)
        {
            if (view.Font != Font.Default)
            {
                Control.TextSize = view.Font.ToScaledPixel();
                //Control.Typeface = view.Font.ToExtendedTypeface(Context);
                Control.Typeface = view.Font.ToTypeface();
            }

            //20180307 inizio
            var fontName = (view as CustomEntry).FontFamily;
            if (string.IsNullOrEmpty(fontName) == false)
            {
                fontName = fontName.Split("#".ToCharArray())[0];
                Typeface font = Typeface.CreateFromAsset(Android.App.Application.Context.Assets, fontName);
                Control.Typeface = font;
            }
            //20180307 fine

        }

        /// <summary>
        /// Sets the color of the placeholder text.
        /// </summary>
        /// <param name="view">The view.</param>
        private void SetPlaceholderTextColor(CustomEntry view)
        {
            if (view.PlaceholderTextColor != Xamarin.Forms.Color.Default)
                Control.SetHintTextColor(view.PlaceholderTextColor.ToAndroid());
        }
    }


}
