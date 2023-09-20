using Android;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Xamarin.Essentials;

namespace xUtilityAndroid
{
    public class Measures
    {
        static Measures()
        {
        }

        static Typeface tfBlack;

        public static Typeface GetTFBlack(Context ctx)
        {
            return tfBlack ?? (tfBlack = Typeface.CreateFromAsset(ctx.Assets, "Fonts/RobotoBlack.ttf"));
        }

        static Typeface tfBold;

        public static Typeface GetTFBold(Context ctx)
        {
            return tfBold ?? (tfBold = Typeface.CreateFromAsset(ctx.Assets, "Fonts/RobotoBold.ttf"));
        }

        static Typeface tfLight;

        public static Typeface GetTFLight(Context ctx)
        {
            return tfLight ?? (tfLight = Typeface.CreateFromAsset(ctx.Assets, "Fonts/RobotoLight.ttf"));
        }

        static Typeface tfMedium;

        public static Typeface GetTFMedium(Context ctx)
        {
            return tfMedium ?? (tfMedium = Typeface.CreateFromAsset(ctx.Assets, "Fonts/RobotoMedium.ttf"));
        }

        static Typeface tfRegular;

        public static Typeface GetTFRegular(Context ctx)
        {
            return tfRegular ?? (tfRegular = Typeface.CreateFromAsset(ctx.Assets, "Fonts/RobotoRegular.ttf"));
        }


        static Typeface tfRegularSlab;

        public static Typeface GetTFRegularSlab(Context ctx)
        {
            return tfRegularSlab ?? (tfRegularSlab = Typeface.CreateFromAsset(ctx.Assets, "Fonts/RobotoSlab-Regular.ttf"));
        }


        public static float ScaleFactor
        {
            get
            {
                return calculatedScaleFactor;
            }
        }

        static float screenWidth = -1;

        public static System.Drawing.Size ScreenSize
        {
            get;
            private set;
        }

        public static float ScreenWidth
        {
            get
            {
                return screenWidth;
            }
        }

        public static float ScreenHeight
        {
            get;
            private set;
        }

        public static float Density
        {
            get;
            private set;
        }

        static float calculatedScaleFactor = -1;

        public static void UpdateScaleFactor(Activity ctx)
        {
            if (calculatedScaleFactor < 0)
            {
                if (screenWidth < 0)
                {

                    screenWidth =
                        (ctx.Resources.Configuration.Orientation == Android.Content.Res.Orientation.Landscape) ?
                            (ctx.WindowManager).DefaultDisplay.Height :
                            (ctx.WindowManager).DefaultDisplay.Width;

                    ScreenHeight =
                        (ctx.Resources.Configuration.Orientation != Android.Content.Res.Orientation.Landscape) ?
                            (ctx.WindowManager).DefaultDisplay.Height :
                            (ctx.WindowManager).DefaultDisplay.Width;


                    ScreenSize = new System.Drawing.Size((int)ScreenWidth, (int)ScreenHeight);

                }
                //var density = ctx.GetDensity();
                var density = getDensity(ctx);
                Density = density;
                calculatedScaleFactor = (screenWidth / density) / (480f / 1.5f);
            }
        }


        public static float getDensity(Context c)
        {
            float ScreenDensity;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                ScreenDensity = Application.Context.Resources.DisplayMetrics.Density;
            }
            else
            {
                var wndMngr = Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
                var dm = new DisplayMetrics();
                wndMngr.DefaultDisplay.GetMetrics(dm);
                ScreenDensity = (float)dm.Density;
            }
            return ScreenDensity;

            /*Display d = (c as Activity).WindowManager.DefaultDisplay;
            DisplayMetrics dm = new DisplayMetrics();

            d.GetMetrics(dm);
            return dm.Density;
*/

            /*
			if (density >= 4.0) {
				return "xxxhdpi";
			}
			if (density >= 3.0) {
				return "xxhdpi";
			}
			if (density >= 2.0) {
				return "xhdpi";
			}
			if (density >= 1.5) {
				return "hdpi";
			}
			if (density >= 1.0) {
				return "mdpi";
			}
			return "ldpi";
			*/
        }


        public static float getWidthDpi(Context c)
        {

            Display d = (c as Activity).WindowManager.DefaultDisplay;
            DisplayMetrics dm = new DisplayMetrics();
            d.GetMetrics(dm);
            return dm.WidthPixels / dm.Density;
        }

        public static float getHeightDpi(Context c)
        {
            //it returns the screen H (for some phones that have soft navigation bar it substracts the softbar (48 dip))
            //note: in forms..page,Height subtracts also status bar (25) and top navigation bar (48)
            Display d = (c as Activity).WindowManager.DefaultDisplay;
            DisplayMetrics dm = new DisplayMetrics();

            d.GetMetrics(dm);
            return dm.HeightPixels / dm.Density;
        }




    }
}

