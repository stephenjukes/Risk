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
            var remainingArmies = armies;
            var armyDistributions = new List<Deployment>();
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), player.Color);
            //PrepareUiForPlayer(player);

            while (remainingArmies > 0)
            {
                _textbox.Write($"Distribute ({remainingArmies} armies):");
                var response = _textbox.Read();
                _textbox.Write();

                var distribution = ValidateDistribution(response, remainingArmies, player, countries);

                if (distribution.IsValid)
                {
                    armyDistributions.Add(distribution.Object);
                    IncrementArmies(distribution.Object);
                    remainingArmies -= distribution.Object.Armies;
                    continue;
                }

                HandleError(distribution.Error);
            }

            return armyDistributions;
        }

        private ValidationResult<Deployment> ValidateDistribution(string response, int remainingArmies, Player player, CountryInfo[] countries)
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
