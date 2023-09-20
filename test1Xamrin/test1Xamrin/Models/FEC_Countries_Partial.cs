using System;
using System.Collections.Generic;
using System.Text;

namespace test1Xamrin
{
    public partial class FEC_Countries
    {
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public string DescrStatoCompletaUnbound { set; get; }
    }
}
