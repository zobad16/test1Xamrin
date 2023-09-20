using System;
using System.Collections.Generic;
using System.Text;
using test1Xamrin.Resx;
using xUtilityPCL;

namespace test1Xamrin
{
    public class StateCodeRuleObbligatorio<T> : IValidationRule<T> //20180510
    {
        public Boolean SoloGlobale { get; set; } //20180515
        public string ValidationMessage { get; set; }

        public BaseModel drCorrente { get; set; }

        public bool Check(T value)
        {
            if (value != null && value.ToString() == AppResources.FormClientiTestoSelezionaProvincia)
                return false;
            else
                return true;
        }
    }
}
