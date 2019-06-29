using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Risk.ResponseValidation
{
    class UserInteractionBuilder<TestObject> where TestObject : new()
    {
        private List<string> _request { get; set; }
            = new List<string>();
        private ValidationParameter<TestObject> _validationParameter { get; set; }
            = new ValidationParameter<TestObject>();
        private Dictionary<string, Func<ValidationParameter<TestObject>, ValidationResult<TestObject>>> _responseInterpretations { get; set; }
            = new Dictionary<string, Func<ValidationParameter<TestObject>, ValidationResult<TestObject>>>();

        public UserInteractionBuilder<TestObject> Request(params string[] request)
        {
            _request.AddRange(request);
            return this;
        }

        public UserInteractionBuilder<TestObject> ResponseInterpretations(params ResponseInterpretation<TestObject>[] interactions)
        {
            foreach (var interaction in interactions)
            {
                _responseInterpretations.Add(interaction.Response, interaction.Interpretation);
            }

            return this;
        }

        public UserInteractionBuilder<TestObject> Player(Player player)
        {
            _validationParameter.Player = player;
            return this;
        }

        public UserInteractionBuilder<TestObject> Countries(CountryInfo[] countries)
        {
            _validationParameter.Countries = countries;
            return this;
        }

        public UserInteractionBuilder<TestObject> Deployment(Deployment deployment)
        {
            _validationParameter.PreviousDeployment = deployment;
            return this;
        }

        public UserInteractionBuilder<TestObject> Cards(List<Card> cards)
        {
            _validationParameter.Cards = cards;
            return this;
        }

        public UserInteractionBuilder<TestObject> ArmiesToDistribute(int armiesToDistribute)
        {
            _validationParameter.ArmiesToDistribute = armiesToDistribute;
            return this;
        }

        public UserInteraction<TestObject> Build()
        {
            return new UserInteraction<TestObject>
            {
                Request = _request,
                ValidationParameter = _validationParameter,
                ResponseInterpretation = _responseInterpretations
            };
        }
    }
}
