using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace xUtilityPCL
{
    public class ListTemplate1
    {
        public static Switch getSwitchWithEffect(string textON, string textOFF)
        {
            var sw = new Switch();
            //sw.Effects.Add(Effect.Resolve("Alterbit.SwitchEffect"));
            sw.Effects.Add(new SwitchTextEffect { TextOn = textON, TextOff = textOFF });
            return sw;

        }
    }
}
