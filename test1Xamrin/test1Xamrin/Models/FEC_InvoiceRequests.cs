using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;
using xUtilityPCL;

namespace test1Xamrin
{
    [AddINotifyPropertyChangedInterface]
    public partial class FEC_InvoiceRequests : BaseModel
    {
        public int IDInvoiceRequest { get; set; }
        public string ReceiptNumber { get; set; }
        public Nullable<System.DateTime> Receiptdate { get; set; }
        public string ReceiptPicture1 { get; set; }
        public string ReceiptPicture2 { get; set; }
        public string ReceiptPicture3 { get; set; }
        public string ReceiptNote { get; set; }
        public Nullable<int> IDCompany { get; set; }
        public string TaxIDNumber { get; set; }
        public string FiscalCode { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string StateCode { get; set; }
        public string Plate { get; set; }
        public Nullable<int> KM { get; set; }
        public Nullable<System.DateTime> FuelDate { get; set; }
        public Nullable<int> PaymentTermsID { get; set; }
        public string PaymentName { get; set; }
        public string ItemCode { get; set; }
        public string ItemCodeDescription { get; set; }
        public string TaxCode { get; set; }
        public Nullable<decimal> Qty { get; set; } = 0;
        public Nullable<decimal> UnitPrice { get; set; } = 0;
        public Nullable<decimal> UnitPriceVAT { get; set; } = 0;
        public Nullable<decimal> TotalAmount { get; set; } = 0;
        public Nullable<decimal> TaxableAmount { get; set; } = 0;
        public Nullable<decimal> VATAmount { get; set; } = 0;
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> SyncroDate { get; set; }
        public Nullable<bool> fromPortal { get; set; } = false;
        public string DocumentState2 { get; set; }
        public string DocumentStateMessage { get; set; }
        public string SeriesDoc { get; set; }
        public Nullable<int> SaleDocNr { get; set; }
        public Nullable<System.DateTime> SaleDocDate { get; set; }
        public string IDCompanyTaxIDNumber { get; set; }
        public string IDCompanyFiscalCode { get; set; }
        public string IDCompanyCompanyName { get; set; }
        public string IDCompanyAddress { get; set; }
        public string IDCompanyCity { get; set; }
        public string IDCompanyZipCode { get; set; }
        public string IDCompanyCounty { get; set; }
        public string IDCompanyCountry { get; set; }
        public string IDCompanyStateCode { get; set; }
        public string IDCompanyTelephone { get; set; }
        public string IDCompanyEmail { get; set; }
        public string IDCompanyPEC { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string PEC { get; set; }
        public string ReceiptTime { get; set; }
        public string ReceiptPointOfSale { get; set; }
        public string ReceiptTerminalNumber { get; set; }
        public string ReceiptPumpNumber { get; set; }
        public string ReceiptQR { get; set; }
        public Nullable<int> SaleDocType { get; set; }
        public string DeviceID { get; set; }
    }
}
