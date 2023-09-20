using System;
using System.Collections.Generic;
using System.Text;


namespace xUtilityPCL
{

    public class MailParams
    {
        public string Office365_SenderEmailforAutoDiscovery { get; set; }
        public string Offile365_SenderPasswordforAutoDiscovery { get; set; }
        public string mailgun_Key { get; set; }
        public string mailgun_Domain { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EmailDisplyaName { get; set; }
        public Boolean IsResponseRequested { get; set; }
        public Boolean IsReadReceiptRequested { get; set; }
        public List<Attachement> Attachements { get; set; }
        public List<string> Recipients { get; set; }
        public List<string> CCs { get; set; }
        public List<string> BCCs { get; set; }

        public MailParams()
        {
            this.Attachements = new List<Attachement>();
            this.Recipients = new List<string>();
            this.CCs = new List<string>();
            this.BCCs = new List<string>();
        }
    }
    public class Attachement
    {
        public string NomeFileSenzaPath { get; set; }
        public Byte[] FileArrayBytes { get; set; }
        public string NomeAllegato { get; set; }
    }



}
