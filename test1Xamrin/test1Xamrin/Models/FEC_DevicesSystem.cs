using System;
using System.Collections.Generic;
using System.Text;

namespace test1Xamrin
{
    public class FEC_DevicesSystem
    {
        public string DeviceID { get; set; }
        public string OS { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string ActivationCode { get; set; }
        public Nullable<bool> Enabled { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public string MinVersion { get; set; }
        public string MaxVersion { get; set; }
        public string Campo1 { get; set; }
        public string Campo2 { get; set; }
        public string Campo3 { get; set; }
    }
}
