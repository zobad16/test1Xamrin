using System;
using System.Collections.Generic;
using System.Text;
using test1Xamrin;
using xUtilityPCL;

namespace test1Xamrin
{
    public class SIDRuleObbligatorio<T> : IValidationRule<T>
    {
        public Boolean SoloGlobale { get; set; }
        public string ValidationMessage { get; set; }
        public BaseModel drCorrente { get; set; }

        bool IValidationRule<T>.Check(T value)
        {
            var dr = drCorrente as FEC_CustomersSystem;
            if (dr.PA == true)
            {
                if (value == null)
                {
                    return false;
                }
                else
                {
                    var str2 = value as string;
                    if (str2.Length != 6) return false;
                    else return true;
                }
            }
            else
            {
                if (value == null)
                {
                    return true;
                }
                {
                    var str2 = value as string;
                    if (str2 == "") return true;
                    if (str2.Length != 7) return false;
                    else return true;
                }
            }

            //var str = value as string;

            //return !string.IsNullOrWhiteSpace(str);
        }
    }
}
