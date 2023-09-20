using System;
using System.Collections.Generic;
using System.Text;

namespace test1Xamrin
{ 
    public class ReturnClassValidazioneTaxIDNumber
    {
        public Boolean valid { get; set; } = false;
        public string name { get; set; } = "";
        public string address { get; set; } = "";
        public string zipCode { get; set; } = "";
        public string city { get; set; } = "";
        public string county { get; set; } = "";
        public string esitoOK_KO { get; set; } = "";
        public string errorMessageKO { get; set; } = "";
        public string codeMessageKO { get; set; } = "";
    }
}
