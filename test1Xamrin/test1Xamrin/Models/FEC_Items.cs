using System;
using System.Collections.Generic;
using System.Text;

namespace test1Xamrin
{
    public partial class FEC_Items
    {
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string TaxCode { get; set; }
        public string ItemColor { get; set; }
        public Nullable<decimal> UnitPriceA { get; set; }
        public Nullable<decimal> UnitPriceB { get; set; }
        public Nullable<bool> PriceDaily { get; set; }
        public Nullable<bool> Disabled { get; set; }
        public Nullable<int> IDCompany { get; set; }
        public string CodiceTipo { get; set; }
        public string CodiceValore { get; set; }
        public string UnitaMisura { get; set; }
        public Nullable<bool> IsFuel { get; set; }
        public Nullable<bool> fromPortal { get; set; } = false;
    }
}
