using System;
using System.Collections.Generic;
using System.Text;

namespace Risk.ResponseValidation
{
    public class UserInteraction<TestObject> where TestObject : new()
    {
        public List<string> Request { get; set; } 
            = new List<string>();
        public ValidationParameter<TestObject> ValidationParameter { get; set; } 
            = new ValidationParameter<TestObject>();
        public Dictionary<string, Func<ValidationParameter<TestObject>, ValidationResult<TestObject>>> ResponseInterpretation { get; set; }
            = new Dictionary<string, Func<ValidationParameter<TestObject>, ValidationResult<TestObject>>>();
    }
}
