using Risk.ResponseValidation;
using System;
using System.Collections.Generic;

namespace Risk.UserInterface.ConsoleUserInterface
{
    public partial class ConsoleUserInterface
    {
        public void DisplayArmyIncome(int fromCountries, int fromContinents, int fromCards)
        {
            _textbox.Write("Army Income:");
            _textbox.Write($"{fromCountries} from country occupation");
            _textbox.Write($"{fromContinents} from continent occupation");
            _textbox.Write($"{fromCards} from cards");
            _textbox.Write();
        }

        public List<Deployment> DistributeArmies(Player player, CountryInfo[] countries, int armies)
        {
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), player.Color);
            var armyDistributions = new List<Deployment>();
            
            while(armies > 0)
            {
                var armyDistribution = GetArmiesForDistribution(player, countries, armies);
                armyDistributions.Add(armyDistribution);
                IncrementArmies(armyDistribution);
                armies -= armyDistribution.Armies;
            }

            return armyDistributions;
        }

        private Deployment GetArmiesForDistribution(Player player, CountryInfo[] countries, int armies)
        {
            var userInteraction = new UserInteractionBuilder<Deployment>()
                .Request($"Distribute ({armies} armies):")
                .ResponseInterpretations
                    (new ResponseInterpretation<Deployment>(
                        "default", vp => ValidateDistribution(vp.Player, vp.Countries, vp.ArmiesToDistribute, vp.Response)))
                .Player(player)
                .Countries(countries)
                .ArmiesToDistribute(armies)
                .Build();

            return HandleResponse(userInteraction);
        }

        private ValidationResult<Deployment> ValidateDistribution(Player player, CountryInfo[] countries, int remainingArmies, string response)
        {
            var responseValidation = new ResponseValidationBuilder<Deployment, int>()
                    .Parameter(response)
                    .Parameter(remainingArmies)
                    .Parameter(player)
                    .Parameter(countries)
                    .MatchBuilder(GetIntegerMatches)
                    .TestObjectBuilder(CreateDistribution)
                    .ErrorChecks(
                        Check.ValidCountryIds,
                        Check.PlayerHoldsDeploymentCountry,
                        Check.SufficientArmies)
                    .Build();

            return responseValidation.CheckErrors();
        }

        private Deployment CreateDistribution(int[] matches, ValidationParameter<Deployment> validationParameter)
        {
            return new Deployment
            {
                Armies = matches[0],
                From = new CountryInfo
                {
                    Armies = validationParameter.ArmiesToDistribute
                },
                To = Array.Find(validationParameter.Countries, c => c.Name == (CountryName)matches[1])
            };
        }
    }
}
