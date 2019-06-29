using System;
using System.Collections.Generic;
using System.Text;

namespace Risk.ResponseValidation
{
    class ResponseInterpretation<TestObject> where TestObject : new()
    {
        public string Response { get; }
        public Func<ValidationParameter<TestObject>, ValidationResult<TestObject>> Interpretation { get; }

        public ResponseInterpretation(string response, Func<ValidationParameter<TestObject>, ValidationResult<TestObject>> interpretation)
        {
            Response = response;
            Interpretation = interpretation;
        }
    }
}
