using System;
using System.Collections.Generic;
using System.Text;

namespace test1Xamrin
{
    public partial class FEC_Items
    {
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public bool selfColumnIsVisibleUnbound { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public string titoloColonnaServitoUnbound { set; get; }
    }
}
