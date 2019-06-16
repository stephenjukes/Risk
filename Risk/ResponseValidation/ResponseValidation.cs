using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Risk
{
    class ResponseValidation<TestObject, TMatch> where TestObject : new()
    {
        public string _response;      
        public Func<string, TMatch[]> _getMatches;
        public Func<TMatch[], ValidationParameter<TestObject>, TestObject> _createTestObject;
        public Func<ValidationParameter<TestObject>, string>[] _errorChecks;

        public Player _player;
        public Dictionary<Country, CountryInfo> _countries;
        public Deployment _previousDeployment;
        public int _armiesToDistribute { get; set; }
        public List<Card> _cards { get; set; }


        public ValidationResult<TestObject> CheckErrors()
        {
            var validationParameter = CreateValidationParameter();

            if (validationParameter == null)
                return new ValidationResult<TestObject>(default(TestObject), false, "Response not recognised.");

            //var errors = _errorChecks.Select(check => check(validationParameter)).Where(e => e != null).ToArray();
            //var isValid = !errors.Any();

            string error = null;
            var validationResult = new ValidationResult<TestObject>(validationParameter.Object, true, error);

            foreach(var check in _errorChecks)
            {
                error = check(validationParameter);
                if (error != null)
                {
                    validationResult.IsValid = false;
                    validationResult.Error = error;
                    return validationResult;
                }
            }

            return validationResult;
        }

        private ValidationParameter<TestObject> CreateValidationParameter()
        {
            TestObject testObject;
            var matches = _getMatches != null ? _getMatches(_response) : null;

            var validationParameter = new ValidationParameter<TestObject>
            {
                Player = _player,
                Countries = _countries,
                PreviousDeployment = _previousDeployment,
                ArmiesToDistribute = _armiesToDistribute
            };

            try
            {
                testObject = _createTestObject(matches, validationParameter);
            }
            catch
            {
                return null;
            }

            validationParameter.Object = testObject;
            return validationParameter;
        }
    }
}
