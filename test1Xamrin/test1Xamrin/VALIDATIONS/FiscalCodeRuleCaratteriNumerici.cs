using System;
using System.Collections.Generic;
using System.Text;
using xUtilityPCL;

namespace test1Xamrin
{
    public class FiscalCodeRuleCaratteriNumerici<T> : IValidationRule<T>
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

            if (value.ToString() == "")
                return true;

            var str = value as string;
            if (str.Length == 11)
            {
                Int64 n;
                var isNumeric = Int64.TryParse(value as string, out n);
                return isNumeric;
            }
            else
                return true;
        }
    }
}
