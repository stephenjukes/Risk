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
            while (true)
            {
                userInteraction.ValidationParameter.Response = GetResponse(userInteraction);
                _textbox.Write();

                var responseInterpretation = InterpretResponse(userInteraction);
                var validationResult = responseInterpretation(userInteraction.ValidationParameter);   // Todo: try to separate concerns

                if (validationResult.IsValid)
                    return validationResult.Object;

                _textbox.Write(validationResult.Error);
                _textbox.Write();
            }
        }

        private Func<ValidationParameter<TestObject>, ValidationResult<TestObject>> InterpretResponse<TestObject>(
            UserInteraction<TestObject> userInteraction) where TestObject : new()
        {
            var responseInterpretation = userInteraction.ResponseInterpretation;
            var validationParameter = userInteraction.ValidationParameter;

            if (responseInterpretation.ContainsKey(validationParameter.Response))
                return responseInterpretation[validationParameter.Response];

            try
            {
                return responseInterpretation["default"];
            }
            catch (Exception ex)
            {
                throw new Exception("No 'default' key found in dictionary: responseInterpreter", ex);
            }
        }

        private string GetResponse<TestObject>(UserInteraction<TestObject> userInteraction) where TestObject : new()
        {
            foreach (var line in userInteraction.Request)
            {
                _textbox.Write(line);
            }

            return _textbox.Read().ToLower();           
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
