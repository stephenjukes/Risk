using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Risk
{
    class ResponseValidationBuilder<TestObject, TMatch> where TestObject : new()
    {
        public string _response { get; set; }
        public Func<string, TMatch[]> _matchBuilder { get; set; }
        public Func<TMatch[], ValidationParameter<TestObject>, TestObject> _testObjectBuilder { get; set; }
        public Func<ValidationParameter<TestObject>, string>[] _errorChecks { get; set; }
        public Player _player { get; set; }
        public CountryInfo[] _countries { get; set; }
        public Deployment _previousDeployment { get; set; }
        public int _armiesToDistribute { get; set; }    // Can't we use Armies from 'From'?


        public ResponseValidationBuilder<TestObject, TMatch> Parameter<Parameter>(Parameter parameter)
        {
            if (parameter == null) return this;

            var property = this.GetType().GetProperties().Where(p => p.PropertyType == parameter.GetType()).FirstOrDefault();
            property.SetValue(this, parameter);

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
                _response = _response,
                _armiesToDistribute = _armiesToDistribute,
                _getMatches = _matchBuilder,
                _createTestObject = _testObjectBuilder,
                _errorChecks = _errorChecks,
                _player = _player,
                _countries = _countries,
                _previousDeployment = _previousDeployment
            };
        }
    }
}
