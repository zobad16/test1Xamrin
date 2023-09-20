using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace xUtilityPCL
{
    public class SwitchTextEffect : RoutingEffect
    {
        public string TextOn { get; set; }
        public string TextOff { get; set; }
        public SwitchTextEffect() : base("Alterbit.SwitchEffect")
        {
        }
    }
}
