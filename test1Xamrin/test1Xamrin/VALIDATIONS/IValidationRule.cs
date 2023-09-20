using System;
using System.Collections.Generic;
using System.Text;

namespace xUtilityPCL
{
    public interface IValidationRule<T> //20180510
    {
        string ValidationMessage { get; set; }

        BaseModel drCorrente { get; set; }

        bool Check(T value);

        Boolean SoloGlobale { get; set; } //20180515
    }
}