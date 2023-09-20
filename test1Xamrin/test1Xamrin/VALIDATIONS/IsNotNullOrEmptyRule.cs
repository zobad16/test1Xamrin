using System;
using System.Collections.Generic;
using System.Text;

namespace xUtilityPCL
{

    public class IsNotNullOrEmptyRule<T> : IValidationRule<T> //20180510
    {
        public Boolean SoloGlobale { get; set; } //20180515
        public string ValidationMessage { get; set; }

        public BaseModel drCorrente { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;

            return !string.IsNullOrWhiteSpace(str);
        }
    }
}