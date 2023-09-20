
using Android.App;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using xUtilityANDROID2;

[assembly: ResolutionGroupName("Alterbit")]
[assembly: ExportEffect(typeof(SwitchEffect), "SwitchEffect")]
[assembly: ExportEffect(typeof(TimePickerEffect), "TimePickerEffect")]

namespace xUtilityANDROID2
{
    public class TimePickerEffect : PlatformEffect
    {
        //non funziona perchè mostra la EditText prima che si apra la vera e propria dialog
        //https://forums.xamarin.com/discussion/80456/timepicker-doesnt-respect-local-setting-24hr-formatting
        //https://forums.xamarin.com/discussion/21119/xamarin-forms-timepicker-with-15-minutes-intervals

        public TimePickerEffect()
        {
        }

        protected override void OnAttached()
        {
            try
            {
                var effect = (xUtilityANDROID2.TimePickerEffect)Element.Effects.FirstOrDefault(e => e is xUtilityANDROID2.TimePickerEffect);
                if (effect != null)
                {
                    if (Control is Android.Widget.EditText)
                    {
                        var sw = Control as Android.Widget.EditText;
                        //Activity c = Forms.Context as Activity;
                        Activity c = Android.App.Application.Context as Activity;
                        //sw.SetIs24HourView(Java.Lang.Boolean.True);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());   
            }


        }

        protected override void OnDetached()
        {
            //throw new NotImplementedException ();
        }

    }

    public class SwitchEffect : PlatformEffect
    {


        public SwitchEffect()
        {
        }

        protected override void OnAttached()
        {
            try
            {
                /*old obsolete code
				 * var effect = (xUtilityPCL.SwitchTextEffect)Element.Effects.FirstOrDefault(e => e is xUtilityPCL.SwitchTextEffect);
				if (effect != null)
				{
					if (Control is Android.Widget.Switch)
					{
						var sw = Control as Android.Widget.Switch;
						Activity c = Forms.Context as Activity;
						sw.TextOn = (effect as xUtilityPCL.SwitchTextEffect).TextOn;
						sw.TextOff = (effect as xUtilityPCL.SwitchTextEffect).TextOff;
						//sw.SetIs24HourView(Java.Lang.Boolean.True);
					}
				}*/
                //updated code
                var effect = Element.Effects.FirstOrDefault(e => e is xUtilityPCL.SwitchTextEffect) as xUtilityPCL.SwitchTextEffect;
                if (effect != null)
                {
                    if (Control is Android.Widget.Switch sw)
                    {
                        var context = Android.App.Application.Context;
                        if (context is Activity activity)
                        {
                            sw.TextOn = effect.TextOn;
                            sw.TextOff = effect.TextOff;
                            //sw.SetIs24HourView(Java.Lang.Boolean.True);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex?.ToString());

            }


        }

        protected override void OnDetached()
        {
            //throw new NotImplementedException ();
        }

    }



}