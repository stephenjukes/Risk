using Risk.ResponseValidation;
using System;
using System.Threading;

namespace Risk.UserInterface.ConsoleUserInterface
{
    public partial class ConsoleUserInterface
    {
        public Deployment GetAttackParameters(Player player, CountryInfo[] countries, Deployment previousAttackParameters)
        {
            var userInteraction = new UserInteractionBuilder<Deployment>()
                .Request(
                    "Attack:",
                    "- [Enter]: continue with same attack parameters",
                    "- [<x> armies from <country_A> to <country_B>]",
                    "- [q]: to skip")
                .ResponseInterpretations(
                    new ResponseInterpretation<Deployment>("q", Quit),
                    new ResponseInterpretation<Deployment>("", vp => ValidatePreviousAttackParameters(vp)),
                    new ResponseInterpretation<Deployment>("default", vp => ValidateNewAttackParameters(vp)))
                .Player(player)
                .Countries(countries)
                .Deployment(previousAttackParameters)
                .Build();

            return HandleResponse(userInteraction);
        }

        private ValidationResult<Deployment> ValidatePreviousAttackParameters(ValidationParameter<Deployment> validationParameter)
        {
            var previousDeployment = validationParameter.PreviousDeployment;

            var responseValidation = new ResponseValidationBuilder<Deployment, int>()
                .ValidationParameter(validationParameter)
                .TestObjectBuilder((matches, vp) => vp.PreviousDeployment)
                .ErrorChecks(
                    Check.PreviousAttackParametersExist,
                    Check.SufficientArmies)
                .Build();

            _textbox.Write($"Repeating attack with {previousDeployment.Armies} armies from {previousDeployment.From.Name} to {previousDeployment.To.Name}");
            _textbox.Write();

            return responseValidation.Validate();
        }

        private ValidationResult<Deployment> ValidateNewAttackParameters(ValidationParameter<Deployment> validationParameter)
        {
            var responseValidation = new ResponseValidationBuilder<Deployment, int>()
                .ValidationParameter(validationParameter)
                .MatchBuilder(GetIntegerMatches)
                .TestObjectBuilder(CreateDeployment)
                .ErrorChecks(
                    Check.ValidCountryIds,
                    Check.PlayerHoldsAttackingCountry,
                    Check.PlayerDoesNotHoldDeploymentCountry,
                    Check.DeploymentToNeighbouringCountry,
                    Check.SufficientArmies)
                .Build();

            return responseValidation.Validate();
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
