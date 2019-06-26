using Risk.ResponseValidation;

namespace Risk.UserInterface.ConsoleUserInterface
{
    public partial class ConsoleUserInterface
    {
        public Deployment GetFortificationParameters(Player player, CountryInfo[] countries)
        {
            var userInteraction = new UserInteraction<Deployment>();

            userInteraction.Request.AddRange(new string[]
                { "Fortify:",
                    "- [<x> armies from <country_A> to <country_B>]",
                    "- [q]: to skip" });

            userInteraction.ValidationParameter.Player = player;
            userInteraction.ValidationParameter.Countries = countries;

            userInteraction.ResponseInterpretation.Add("q", Quit);
            userInteraction.ResponseInterpretation.Add("default", vp => ValidateFortificationParameters(vp.Player, vp.Countries, vp.Response));

            return HandleResponse(userInteraction);
        }

        private ValidationResult<Deployment> ValidateFortificationParameters(Player player, CountryInfo[] countries, string response)
        {
            var responseValidation = new ResponseValidationBuilder<Deployment, int>()
                .Parameter(response)
                .Parameter(player)
                .Parameter(countries)
                .MatchBuilder(GetIntegerMatches)
                .TestObjectBuilder(CreateDeployment)
                .ErrorChecks(
                    Check.ValidCountryIds,
                    Check.PlayerHoldsAttackingCountry,
                    Check.PlayerHoldsDeploymentCountry,
                    Check.DeploymentToNeighbouringCountry,
                    Check.SufficientArmies)
                .Build();

            return responseValidation.CheckErrors();
        }
    }
}
