using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Risk
{
    public class ResponseValidation<TestObject, TMatch> where TestObject : new()
    {     
        public Func<string, TMatch[]> _getMatches;
        public Func<TMatch[], ValidationParameter<TestObject>, TestObject> _createTestObject;
        public Func<ValidationParameter<TestObject>, string>[] _errorChecks;
        public ValidationParameter<TestObject> _validationParameter { get; set; }

        public ValidationResult<TestObject> Validate()
        {
            var parameterWithCandidate = CreateValidationCandidate();

            if (parameterWithCandidate == null)
                return new ValidationResult<TestObject>(default(TestObject), false, "Response not recognised.");

            return GetValidationResult(parameterWithCandidate);     
        }

        private ValidationParameter<TestObject> CreateValidationCandidate()
        {
            var matches = _getMatches != null ? _getMatches(_validationParameter.Response) : null;

            try
            {
                _validationParameter.Object = _createTestObject(matches, _validationParameter);
                return _validationParameter;
            }
            catch
            {
                return null;
            }
        }

        private ValidationResult<TestObject> GetValidationResult(ValidationParameter<TestObject> parameterWithCandidate)
        {
            string error = null;
            var validationResult = new ValidationResult<TestObject>(parameterWithCandidate.Object, true, error);

            foreach (var check in _errorChecks)
            {
                error = check(parameterWithCandidate);
                if (error != null)
                {
                    validationResult.IsValid = false;
                    validationResult.Error = error;
                    return validationResult;
                }
            }

            return validationResult;
        }
    }
}
