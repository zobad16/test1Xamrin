using System;
using System.Collections.Generic;
using System.Text;
using xUtilityPCL;

namespace test1Xamrin 
{ 
    public class TaxIDNumberRuleSoloNumeri<T> : IValidationRule<T>
{
    public string ValidationMessage { get; set; }
    public BaseModel drCorrente { get; set; }
    public Boolean SoloGlobale
    {
        get; set;
    }

    bool IValidationRule<T>.Check(T value)
    {
        if (value == null)
        {
            return true;
        }

        if (value.ToString() == "")
            return true;

        Int64 n;
        var isNumeric = Int64.TryParse(value as string, out n);
        return isNumeric;
    }
}
}
