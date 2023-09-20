using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using xUtilityPCL;

namespace test1Xamrin
{
    public partial class FEC_CustomersSystem
    {
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand cmdbtnSalva { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand cmdbtnAnnulla { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public string StatoFormRispettoSqlite_IMC { set; get; } = "";

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public string IndirizzoCompleto { get; set; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public string RecapitoUnbound { get; set; }

        //x ogni campo di quelli che si vedono a video
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> TaxIDNumber_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateTaxIDNumberCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> CompanyName_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateCompanyNameCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> Address_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateAddressCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> Telephone_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateTelephoneCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> City_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateCityCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> ZipCode_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateZipCodeCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> County_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateCountyCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> County2_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateCounty2Command { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> Country_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateCountryCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> FiscalCode_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateFiscalCodeCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> Pec_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidatePecCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> Email_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateEmailCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> ContactName_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateContactNameCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> ContactTelephone_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateContactTelephoneCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> ContactEmail_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateContactEmailCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> CIG_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateCIGCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> CUP_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateCUPCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> SID_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateSIDCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> TaxCodeDefault_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateTaxCodeDefaultCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> StateCode_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateStateCodeCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> StateCode2_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateStateCode2Command { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> NumeroOrdine_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateNumeroOrdineCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> NumeroContratto_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateNumeroContrattoCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> NumeroConvenzione_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateNumeroConvenzioneCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> NumeroRicezione_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateNumeroRicezioneCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> DataOrdine_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateDataOrdineCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> DataContratto_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateDataContrattoCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> DataConvenzione_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateDataConvenzioneCommand { set; get; }

        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ValidatableObject<string> DataRicezione_VAL { get; set; }
        [Newtonsoft.Json.JsonIgnore, SQLite.Ignore]
        public ICommand ValidateDataRicezioneCommand { set; get; }
    }
}
