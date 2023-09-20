using System;
using System.Collections.Generic;
using System.Text;
using test1Xamrin;
using test1Xamrin.Resx;
using xUtilityPCL;

namespace test1Xamrin
{
    public class TaxIDNumberRuleLunghezza<T> : IValidationRule<T>
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
            var dr = drCorrente as FEC_CustomersSystem;

            if (dr.StateCode2_VAL.Value != "IT")
            {
                //ESTERO
                return true;
            }
            else
            {
                //ITALIA
                if (dr.IsPrivate)
                {
                    ValidationMessage = AppResources.FormDettClienteValidazioniPIVA2Vers16;

                    if (str.Length != 16) return false;
                    else return true;
                }
                else
                {
                    ValidationMessage = AppResources.FormDettClienteValidazioniPIVA2;

                    if (str.Length != 11) return false;
                    else return true;
                }
            }
        }
    }
}
