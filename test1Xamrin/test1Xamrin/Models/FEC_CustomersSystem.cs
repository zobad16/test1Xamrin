using xUtilityPCL;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Text;

namespace test1Xamrin
{
    [AddINotifyPropertyChangedInterface]
    public partial class FEC_CustomersSystem : BaseModel
    {
        public FEC_CustomersSystem()
        {
            this.TaxIDNumber_VAL = new ValidatableObject<string>();
            this.CompanyName_VAL = new ValidatableObject<string>();
            this.Address_VAL = new ValidatableObject<string>();
            this.Telephone_VAL = new ValidatableObject<string>();
            this.City_VAL = new ValidatableObject<string>();
            this.ZipCode_VAL = new ValidatableObject<string>();
            this.County_VAL = new ValidatableObject<string>();
            this.County2_VAL = new ValidatableObject<string>();
            this.Country_VAL = new ValidatableObject<string>();
            this.StateCode_VAL = new ValidatableObject<string>();
            this.StateCode2_VAL = new ValidatableObject<string>();
            this.FiscalCode_VAL = new ValidatableObject<string>();
            this.Pec_VAL = new ValidatableObject<string>();
            this.Email_VAL = new ValidatableObject<string>();
            this.ContactName_VAL = new ValidatableObject<string>();
            this.ContactTelephone_VAL = new ValidatableObject<string>();
            this.ContactEmail_VAL = new ValidatableObject<string>();
            this.CIG_VAL = new ValidatableObject<string>();
            this.CUP_VAL = new ValidatableObject<string>();
            this.SID_VAL = new ValidatableObject<string>();
            this.TaxCodeDefault_VAL = new ValidatableObject<string>();
            this.NumeroOrdine_VAL = new ValidatableObject<string>();
            this.NumeroContratto_VAL = new ValidatableObject<string>();
            this.NumeroConvenzione_VAL = new ValidatableObject<string>();
            this.NumeroRicezione_VAL = new ValidatableObject<string>();
            this.DataOrdine_VAL = new ValidatableObject<string>();
            this.DataContratto_VAL = new ValidatableObject<string>();
            this.DataConvenzione_VAL = new ValidatableObject<string>();
            this.DataRicezione_VAL = new ValidatableObject<string>();
        }

        public string TaxIDNumber { get; set; }
        public Nullable<int> IDCompany { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string County { get; set; }
        public string Country { get; set; } = "Italia";
        public string FiscalCode { get; set; }
        public string Pec { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string ContactTelephone { get; set; }
        public string ContactEmail { get; set; }
        public string CIG { get; set; }
        public string CUP { get; set; }
        public string SID { get; set; }
        public string TaxCodeDefault { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public Nullable<bool> Disabled { get; set; }
        public string ERPCode { get; set; }
        public string IPACode { get; set; }
        public Nullable<bool> PA { get; set; } = false;
        public Nullable<int> IDCustomer { get; set; }
        public string NumeroOrdine { get; set; }
        public Nullable<System.DateTime> DataOrdine { get; set; }
        public string StateCode { get; set; } = "IT";
        public string NumeroContratto { get; set; }
        public Nullable<System.DateTime> DataContratto { get; set; }
        public string NumeroConvenzione { get; set; }
        public Nullable<System.DateTime> DataConvenzione { get; set; }
        public string NumeroRicezione { get; set; }
        public Nullable<System.DateTime> DatiRicezione { get; set; }
        public Nullable<bool> fromPortal { get; set; } = false;
        public Nullable<System.Guid> guid { get; set; }
        public bool IsPrivate { get; set; } = false;
        public bool IsLiableForWithholdingTaxPercentageCompanyType2 { get; set; } = false;
        public Nullable<bool> isSplitPayment { get; set; } = false;
        public Nullable<int> Codiva1 { get; set; }
        public string Codiva1Description { get; set; }
        public Nullable<int> Codiva2 { get; set; }
        public string Codiva2Description { get; set; }
        public Nullable<int> Codiva3 { get; set; }
        public string Codiva3Description { get; set; }
        public Nullable<int> Codiva4 { get; set; }
        public string Codiva4Description { get; set; }
        public Nullable<int> CodivaN1 { get; set; }
        public string CodivaN1Description { get; set; }
        public Nullable<int> CodivaN2 { get; set; }
        public string CodivaN2Description { get; set; }
        public Nullable<int> CodivaN3 { get; set; }
        public string CodivaN3Description { get; set; }
        public Nullable<int> CodivaN4 { get; set; }
        public string CodivaN4Description { get; set; }
        public Nullable<int> CodivaN5 { get; set; }
        public string CodivaN5Description { get; set; }
        public Nullable<int> CodivaN6 { get; set; }
        public string CodivaN6Description { get; set; }
        public Nullable<int> CodivaN7 { get; set; }
        public string CodivaN7Description { get; set; }
        public Nullable<int> PaymentTermsIDDefault { get; set; } //20190114
    }
}
