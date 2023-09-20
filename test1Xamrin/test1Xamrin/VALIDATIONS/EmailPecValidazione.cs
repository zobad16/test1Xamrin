using System;
using System.Collections.Generic;
using System.Text;
using xUtilityPCL;

namespace test1Xamrin
{
    public class EmailPecValidazione<T> : IValidationRule<T>
    {
        public Boolean SoloGlobale { get; set; }
        public string ValidationMessage { get; set; }
        public BaseModel drCorrente { get; set; }

        bool IValidationRule<T>.Check(T value)
        {
            if (value == null)
            {
                return true;
            }

            var str = value as string;
            if (str == "")
                return true;

            var isEmail = xUtilityPCL.Utility.isEmail(str);

            if (isEmail) return true;
            else return false;
        }
    }
}
