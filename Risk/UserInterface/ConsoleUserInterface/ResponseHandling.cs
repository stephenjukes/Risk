using Risk.ResponseValidation;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Risk.UserInterface.ConsoleUserInterface
{
    public partial class ConsoleUserInterface
    {
        private TestObject HandleResponse<TestObject>(UserInteraction<TestObject> userInteraction) where TestObject : new()
        {
            var request = userInteraction.Request;
            var validationParameter = userInteraction.ValidationParameter;
            var responseInterpretation = userInteraction.ResponseInterpretation;
            ValidationResult<TestObject> validationResult;

            while (true)
            {
                foreach (var line in request)
                {
                    _textbox.Write(line);
                }

                validationParameter.Response = _textbox.Read().ToLower();
                _textbox.Write();

                if (responseInterpretation.ContainsKey(validationParameter.Response))
                    validationResult = responseInterpretation[validationParameter.Response](validationParameter);
                else
                {
                    try
                    {
                        validationResult = responseInterpretation["default"](validationParameter);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("No 'default' key found in dictionary: responseInterpreter", ex);
                    }
                }

                if (validationResult.IsValid)
                    return validationResult.Object;

                _textbox.Write(validationResult.Error);
                _textbox.Write();
            }
        }

        private int[] GetIntegerMatches(string response)
        {
            return Regex.Matches(response, @"\d+")
                .Select(m => int.Parse(m.ToString()))
                .ToArray();
        }

        private Deployment CreateDeployment(int[] matches, ValidationParameter<Deployment> validationParameter)
        {
            return new Deployment
            {
                Armies = matches[0],
                From = Array.Find(validationParameter.Countries, c => c.Name == (CountryName)matches[1]),
                To = Array.Find(validationParameter.Countries, c => c.Name == (CountryName)matches[2]),
            };
        }

        private ValidationResult<TestObject> Quit<TestObject>(ValidationParameter<TestObject> validationParameter) where TestObject : new()
        {
            return new ValidationResult<TestObject>
            {
                IsValid = true,
                IsRequired = false
            };
        }

        private ValidationResult<TestObject> ResponseNotRecognised<TestObject>(ValidationParameter<TestObject> validationParameter) where TestObject : new()
        {
            return new ValidationResult<TestObject>
            {
                IsValid = false,
                Error = "Response not recognised.Please try again."
            };
        }

        private ValidationResult<bool> Accept(ValidationParameter<bool> validationParameter)
        {
            return new ValidationResult<bool>
            {
                Object = true,
                IsValid = true,
            };
        }

        private ValidationResult<bool> Decline(ValidationParameter<bool> validationParameter)
        {
            return new ValidationResult<bool>
            {
                Object = false,
                IsValid = true,
            };
        }

        private ValidationResult<TestObject> ProhibitQuit<TestObject>(ValidationParameter<TestObject> validationParameter) where TestObject : new()
        {
            return new ValidationResult<TestObject>
            {
                IsValid = false,
                Error = "Cannot quit - you must trade when holding at least 5 cards"
            };
        }

        private void HandleError(string error)
        {
            _textbox.Write(error);
            _textbox.Write();
        }
    }
}
