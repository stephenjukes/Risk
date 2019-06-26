using Risk.ResponseValidation;
using System;
using System.Threading;

namespace Risk.UserInterface.ConsoleUserInterface
{
    public partial class ConsoleUserInterface
    {
        public Deployment GetAttackParameters(Player player, CountryInfo[] countries, Deployment previousAttackParameters)
        {
            var userInteraction = new UserInteraction<Deployment>();

            userInteraction.Request.AddRange(new string[]
                { "Attack:",
                    "- [Enter]: continue with same attack parameters",
                    "- [<x> armies from <country_A> to <country_B>]",
                    "- [q]: to skip" });

            userInteraction.ValidationParameter.Player = player;
            userInteraction.ValidationParameter.Countries = countries;
            userInteraction.ValidationParameter.PreviousDeployment = previousAttackParameters;

            userInteraction.ResponseInterpretation.Add("q", Quit);
            userInteraction.ResponseInterpretation.Add("", vp => ValidatePreviousAttackParameters(vp.PreviousDeployment, vp.Response));
            userInteraction.ResponseInterpretation.Add("default", vp => ValidateNewAttackParameters(vp.Player, vp.Countries, vp.Response));

            return HandleResponse(userInteraction);
        }

        private ValidationResult<Deployment> ValidatePreviousAttackParameters(Deployment attackParameters, string response)
        {
            var responseValidation = new ResponseValidationBuilder<Deployment, int>()
                .Parameter(response)
                .Parameter(attackParameters)
                .TestObjectBuilder((matches, validationParameter) => attackParameters)
                .ErrorChecks(
                    Check.PreviousAttackParametersExist,
                    Check.SufficientArmies)
                .Build();

            _textbox.Write($"Repeating attack with {attackParameters.Armies} armies from {attackParameters.From.Name} to {attackParameters.To.Name}");
            _textbox.Write();

            return responseValidation.CheckErrors();
        }

        private ValidationResult<Deployment> ValidateNewAttackParameters(Player player, CountryInfo[] countries, string response)
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
                    Check.PlayerDoesNotHoldDeploymentCountry,
                    Check.DeploymentToNeighbouringCountry,
                    Check.SufficientArmies)
                .Build();

            return responseValidation.CheckErrors();
        }

        public Deployment Skirmish(Deployment attackParameters)
        {
            var armies = attackParameters.Armies;
            var invadingFrom = attackParameters.From;
            var invadingTo = attackParameters.To;
            var fortifyingArmies = invadingFrom.Armies - armies;

            while ((invadingFrom.Armies > fortifyingArmies) && (invadingTo.Armies > 0))
            {
                var numberOfAttackingDice = (armies >= 3 && invadingFrom.Armies >= 3) ? 3 : armies;
                var numberOfDefendingDice = invadingTo.Armies == 1 ? 1 : 2;
                var diceToCompare = Math.Min(numberOfAttackingDice, numberOfDefendingDice);

                var attackingDice = new int[numberOfAttackingDice];
                var defendingDice = new int[numberOfDefendingDice];

                Roll(attackingDice);
                Roll(defendingDice);

                for (var i = 0; i < diceToCompare; i++)
                {
                    if (attackingDice[i] > defendingDice[i])
                        invadingTo.Armies -= 1;
                    else
                        invadingFrom.Armies -= 1;
                }

                foreach (var country in new CountryInfo[] { invadingTo, invadingFrom })
                {
                    Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), country.Occupier.Color);

                    var armyPosition = country.StateSpace.ArmyPosition;
                    Console.SetCursorPosition(armyPosition.Column, armyPosition.Row);

                    Console.WriteLine(country.Armies.ToString().PadLeft(2, '0'));
                }

                Thread.Sleep(100);
            }

            return new Deployment
            {
                Armies = armies,
                From = invadingFrom,
                To = invadingTo
            };
        }

        public void RenderSkirmish(Deployment attackParameters)
        {
            foreach (var country in new CountryInfo[] { attackParameters.To, attackParameters.From })
            {
                Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), country.Occupier.Color);

                var armyPosition = country.StateSpace.ArmyPosition;
                Console.SetCursorPosition(armyPosition.Column, armyPosition.Row);

                Console.WriteLine(country.Armies.ToString().PadLeft(2, '0'));
            }

            Thread.Sleep(100);
        }

        private void Roll(int[] dice)
        {
            var score = new Random();
            for (var i = 0; i < dice.Length; i++)
            {
                dice[i] = score.Next(0, 7);
            }

            Array.Sort(dice);
            Array.Reverse(dice);
        }

        public void ManageBattleVictory(Player occupier)
        {
            _textbox.Write($"Victory to {occupier.Name} !!!".ToUpper());
            _textbox.Write();
        }

        public void ManagePlayerElimination(Player invader, Player defender)
        {
            _textbox.Write($"{defender.Name} has been eliminated. All of {defender.Name}'s cards have been passed onto {invader.Name}");
        }
    }
}
