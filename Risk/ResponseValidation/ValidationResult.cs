using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    public class ValidationResult<TRiskObject> where TRiskObject : new()
    {
        public TRiskObject Object { get; set; }
        public bool IsRequired { get; set; }
        public bool IsValid { get; set; }
        public string Error { get; set; }

        public ValidationResult() { }

        public ValidationResult(TRiskObject obj, bool isValid, string error)
        {
            Object = obj;
            IsValid = isValid;
            Error = error;
        }
    }
}
