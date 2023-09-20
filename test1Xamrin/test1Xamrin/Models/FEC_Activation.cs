using System;
using System.Collections.Generic;
using System.Text;

namespace test1Xamrin
{
    public partial class FEC_Activation
    {
        public int IDCompany { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string County { get; set; }
        public string TaxIDNumber { get; set; }
        public string Country { get; set; }
        public string FiscalCode { get; set; }
        public string Pec { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string ContactTelephone { get; set; }
        public string ContactEmail { get; set; }
        public Nullable<bool> Disabled { get; set; }
        public string ActivationCode { get; set; }
        public string Series { get; set; }
        public string Logo { get; set; }
        public string UrlApp { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseUser { get; set; }
        public string DatabasePassword { get; set; }
        public string DataBaseServer { get; set; }
        public string ServiceType { get; set; }
        public string DigitalHUBUrl { get; set; }
        public string DigitalHubUser { get; set; }
        public string DigitalHubPassword { get; set; }
        public string DigitalHubCode { get; set; }
        public string SMTPServer { get; set; }
        public string SMTPUserName { get; set; }
        public string SMTPPassword { get; set; }
        public Nullable<bool> PDFwithDEV { get; set; }
        public Nullable<bool> LogPriceList { get; set; }
        public Nullable<int> DayLogPriceList { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<int> CompanyType { get; set; }
        public string OwnerCode { get; set; }
        public string StateCode { get; set; }
        public string Temp { get; set; }
        public string Acronym { get; set; }
        public Nullable<int> MaxDeviceNumber { get; set; }
        public string ResellerCode { get; set; }
        public Nullable<System.DateTime> DataAttivazione { get; set; }
        public Nullable<System.DateTime> DataDecorrenza { get; set; }
        public Nullable<System.DateTime> DataScadenza { get; set; }
        public Nullable<int> SMTPPort { get; set; }
        public Nullable<bool> CreditNoteSameInvoiceNumeration { get; set; }
        public string Iban { get; set; }
        public string PensionFundCode { get; set; }
        public string PensionFundDescription { get; set; }
        public Nullable<decimal> PensionFundPercentage { get; set; }
        public Nullable<decimal> WithholdingTaxPercentageItaly { get; set; }
        public Nullable<decimal> WithholdingTaxPercentageAbroad { get; set; }
        public Nullable<decimal> RevenueStampAmount { get; set; }
        public Nullable<decimal> RevenueStampLimit { get; set; }
        public Nullable<bool> RevenueStampDefault { get; set; }
        public Nullable<bool> PensionFundIsLiableForWithholdingTaxPercentageDefault { get; set; }
        public bool PensioFundChargedDefault { get; set; }
        public string mailSID { get; set; }
        public Nullable<bool> sending_method { get; set; }
        public Nullable<bool> DigitalHub_WithSignature { get; set; }
        public Nullable<bool> IsReceiptNumberMandatory { get; set; } = false;
        public Nullable<bool> IsReceiptdateMandatory { get; set; } = false;
        public Nullable<bool> IsReceiptTimeMandatory { get; set; } = false;
        public Nullable<bool> IsReceiptPicture1Mandatory { get; set; } = false;
        public Nullable<bool> IsReceiptPicture2Mandatory { get; set; } = false;
        public Nullable<bool> IsReceiptPicture3Mandatory { get; set; } = false;
        public Nullable<bool> IsReceiptNoteMandatory { get; set; } = false;
        public Nullable<bool> IsReceiptPointOfSaleMandatory { get; set; } = false;
        public Nullable<bool> IsReceiptTerminalNumberMandatory { get; set; } = false;
        public Nullable<bool> IsReceiptPumpNumberMandatory { get; set; } = false;
        public Nullable<bool> IsReceiptQRMandatory { get; set; } = false;
        public Nullable<bool> IsEmailMandatory { get; set; } //20190114
    }
}
