using System;
using System.Collections.Generic;
using System.Text;
using xUtilityPCL;

namespace test1Xamrin
{
    public class CUPRuleLunghezza<T> : IValidationRule<T>
    {
        public Boolean SoloGlobale { get; set; }
        public string ValidationMessage { get; set; }
        public BaseModel drCorrente { get; set; }

        bool IValidationRule<T>.Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;
            if (str.Length > 15) return false;
            else return true;
        }
    }
}
