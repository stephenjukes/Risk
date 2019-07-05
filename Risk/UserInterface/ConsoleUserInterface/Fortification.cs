using Risk.ResponseValidation;

namespace Risk.UserInterface.ConsoleUserInterface
{
    public partial class ConsoleUserInterface
    {
        public Deployment GetFortificationParameters(Player player, CountryInfo[] countries)
        {
            var userInteraction = new UserInteractionBuilder<Deployment>()
                .Request(
                    "Fortify:",
                    "- [<x> armies from <country_A> to <country_B>]",
                    "- [q]: to skip")
                .ResponseInterpretations(
                    new ResponseInterpretation<Deployment>("q", Quit),
                    new ResponseInterpretation<Deployment>("default", vp => ValidateFortificationParameters(vp)))
                .Player(player)
                .Countries(countries)
                .Build();

            return HandleResponse(userInteraction);
        }

        private ValidationResult<Deployment> ValidateFortificationParameters(ValidationParameter<Deployment> validationParameter)
        {
            var responseValidation = new ResponseValidationBuilder<Deployment, int>()
                .ValidationParameter(validationParameter)
                .MatchBuilder(GetIntegerMatches)
                .TestObjectBuilder(CreateDeployment)
                .ErrorChecks(
                    Check.ValidCountryIds,
                    Check.PlayerHoldsAttackingCountry,
                    Check.PlayerHoldsDeploymentCountry,
                    Check.DeploymentToNeighbouringCountry,
                    Check.SufficientArmies)
                .Build();

            return responseValidation.Validate();
        }
    }
}
