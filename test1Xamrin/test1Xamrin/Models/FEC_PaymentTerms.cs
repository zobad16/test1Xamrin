using System;
using System.Collections.Generic;
using System.Text;

namespace test1Xamrin
{
    public class FEC_PaymentTerms
    {
        public int IDPaymentTerms { get; set; }
        public string PaymentTerm { get; set; }
        public string PaymentDescription { get; set; }
        public Nullable<bool> Disabled { get; set; }
        public string CodicePagamento { get; set; }
        public Nullable<bool> fromPortal { get; set; } = false;
    }
}
