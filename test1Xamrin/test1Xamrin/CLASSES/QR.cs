using System;
using System.Collections.Generic;
using System.Text;

namespace test1Xamrin.QR
{
    public class DomFisc
    {
        public string prov { get; set; }
        public string cap { get; set; }
        public string com { get; set; }
        public string ind { get; set; }
        public string naz { get; set; }
    }

    public class Anag
    {
        public string naz { get; set; }
        public string cf { get; set; }
        public string piva { get; set; }
        public string denom { get; set; }
        public DomFisc domFisc { get; set; }
    }

    public class SDI
    {
        public string pec { get; set; }
        public string cod { get; set; }
    }

    public class RootObject
    {
        public Anag anag { get; set; }
        public DateTime dtGenQr { get; set; }
        public SDI SDI { get; set; }
    }
}
