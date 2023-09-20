using System;
using System.Collections.Generic;
using System.Text;

namespace test1Xamrin
{
    public partial class FEC_Countries
    {
        public int IDCountry { get; set; }
        public string CountryDescription { get; set; }
        public string iso_3166_2 { get; set; }
        public string iso_3166_3 { get; set; }
        public Nullable<bool> Disabled { get; set; }
    }
}
