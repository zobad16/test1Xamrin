using System;
using System.Collections.Generic;
using System.Text;

namespace test1Xamrin
{
    public class FEC_TaxCode
    {
        public string TaxCode { get; set; }
        public string TaxDescription { get; set; }
        public Nullable<decimal> TaxPerc { get; set; }
        public Nullable<bool> Disabled { get; set; }
        public Nullable<bool> fromPortal { get; set; } = false;
    }
}
