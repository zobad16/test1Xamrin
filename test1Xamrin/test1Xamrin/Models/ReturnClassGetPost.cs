using System;
using System.Collections.Generic;
using System.Text;

namespace test1Xamrin
{
    public class ReturnClassGetPost
    {
        public string esitoOK_KO { get; set; } = "";
        public string contentOK { get; set; } = "";
        public string errorMessageKO { get; set; } = "";
        public string codeMessageKO { get; set; } = "";
    }

    public class ReturnClassCheckVersione
    {
        public string esitoOK_KO { get; set; } = "";
        public string contentOK { get; set; } = "";
        public string errorMessageKO { get; set; } = "";
        public string codeMessageKO { get; set; } = "";
        public string maxVersionRequired { get; set; } = "";
        public string minVersionRequired { get; set; } = "";
    }
}
