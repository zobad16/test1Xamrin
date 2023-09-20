using System;
using System.Collections.Generic;
using System.Text;
using xUtilityPCL;

namespace test1Xamrin
{
    public partial class xFatturazioneAttivaLogs : BaseModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public System.DateTime TBCreated { get; set; }
        public string Campo1 { get; set; }
        public string Campo2 { get; set; }
        public string Campo3 { get; set; }
        public DateTime? Data1 { get; set; }
        public DateTime? Data2 { get; set; }
        public DateTime? Data3 { get; set; }
    }
}
