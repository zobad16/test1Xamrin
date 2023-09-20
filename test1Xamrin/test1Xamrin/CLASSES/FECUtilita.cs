using System;
using System.Collections.Generic;
using System.Text;
using test1Xamrin;

namespace test1Xamrin
{
    public class FECUtilita
    {
        public FECUtilita()
        {
        }

        public static string getRecapito(FEC_CustomersSystem drCustomer)
        {
            string recapito = "";

            if (string.IsNullOrEmpty(drCustomer.SID) == false && drCustomer.SID != "0000000")
                recapito = drCustomer.SID;
            else
                recapito = drCustomer.Pec;

            return recapito;
        }
    }
}
