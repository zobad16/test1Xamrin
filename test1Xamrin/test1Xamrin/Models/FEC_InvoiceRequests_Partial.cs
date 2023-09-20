using System;
using System.Collections.Generic;
using System.Text;

namespace test1Xamrin
{
    public partial class FEC_InvoiceRequests
    {
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public string StatoFormRispettoSqlite_IMC { set; get; } = "";

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public string IDCompanyTaxIDNumberCompanyNameUnbound { get; set; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public string IDCompanyIndirizzoCittaUnbound { get; set; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public string UnitaMisuraUnbound { get; set; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public string DescrPrdottoUnbound { get; set; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public string DescrizioneTipoUnbound { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public string NumeroEDataUnbound { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public string RecapitoUnbound { get; set; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public string GenerataFtRicNumDelUnbound { get; set; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public Boolean IsCellaDocRigaGenerataFtRicNumDelVisibleUnbound { get; set; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public Xamarin.Forms.Color ColoreUnbound { get; set; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public Boolean IsMsgRichiestaRespintaVisibleUnbound { get; set; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public string MsgRichiestaRespintaUnbound { get; set; }
    }
}
