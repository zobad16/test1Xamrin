using System;
using System.Collections.Generic;
using System.Text;
using test1Xamrin;
using xUtilityPCL;

namespace test1Xamrin
{
    public class PecRuleObbligatoria<T> : IValidationRule<T>
    {
        public Boolean SoloGlobale { get; set; }
        public string ValidationMessage { get; set; }
        public BaseModel drCorrente { get; set; }

        bool IValidationRule<T>.Check(T value)
        {
            var dr = drCorrente as FEC_CustomersSystem;
            if (string.IsNullOrEmpty(dr.SID_VAL.Value) || dr.SID_VAL.Value == "000000" || dr.SID_VAL.Value == "0000000")//20181217 
                                                                                                                        //SID vuoto, PEC obbligatoria
            {
                if (value == null || value.ToString() == "")
                    return false;
                else
                    return true;
            }
            else //SID presente, PEC NON obbligatoria
            {
                return true; //20181217

                /* 20181217 inizo commento
               if (value == null || value.ToString() == "")
                  return true;
               else
                  return false;
               */
            }
        }
    }
}
