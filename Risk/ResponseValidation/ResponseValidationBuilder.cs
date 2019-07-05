using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Risk
{
    public class ResponseValidationBuilder<TestObject, TMatch> where TestObject : new()
    {
        public string _response { get; set; }
        public Func<string, TMatch[]> _matchBuilder { get; set; }
        public Func<TMatch[], ValidationParameter<TestObject>, TestObject> _testObjectBuilder { get; set; }
        public Func<ValidationParameter<TestObject>, string>[] _errorChecks { get; set; }

        public ValidationParameter<TestObject> _validationParameter;

        public ResponseValidationBuilder<TestObject, TMatch> ValidationParameter(ValidationParameter<TestObject> validationParameter)
        {
            _validationParameter = validationParameter;
            return this;
        }

        public ResponseValidationBuilder<TestObject, TMatch> Response(string response)
        {
            _response = response;
            return this;
        }

        public ResponseValidationBuilder<TestObject, TMatch> Player<Parameter>(string response)
        {
            _response = response;
            return this;
        }

        public ResponseValidationBuilder<TestObject, TMatch> Countries<Parameter>(string response)
        {
            _response = response;
            return this;
        }

        public ResponseValidationBuilder<TestObject, TMatch> Deployment<Parameter>(string response)
        {
            _response = response;
            return this;
        }

        public ResponseValidationBuilder<TestObject, TMatch> Cards<Parameter>(string response)
        {
            _response = response;
            return this;
        }

        public ResponseValidationBuilder<TestObject, TMatch> ErrorChecks(params Func<ValidationParameter<TestObject>, string>[] errorChecks)
        {
            _errorChecks = errorChecks;
            return this;
        }

        public ResponseValidationBuilder<TestObject, TMatch> MatchBuilder(Func<string, TMatch[]> matchBuilder)
        {
            _matchBuilder = matchBuilder;
            return this;
        }

        public ResponseValidationBuilder<TestObject, TMatch> TestObjectBuilder(Func<TMatch[], ValidationParameter<TestObject>, TestObject> testObjectBuilder)
        {
            _testObjectBuilder = testObjectBuilder;
            return this;
        }

        public ResponseValidation<TestObject, TMatch> Build()
        {
            return new ResponseValidation<TestObject, TMatch>
            {
                _validationParameter = _validationParameter,
                _getMatches = _matchBuilder,
                _createTestObject = _testObjectBuilder,
                _errorChecks = _errorChecks,
            };
        }
    }
}
