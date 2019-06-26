using Risk.ResponseValidation;
using System;
using System.Threading;

namespace Risk.UserInterface.ConsoleUserInterface
{
    public partial class ConsoleUserInterface
    {
        public Deployment GetArmyTransfer(Deployment deployment)
        {
            var userInteraction = new UserInteraction<Deployment>();

            userInteraction.Request
                .Add("How many armies do you wish to transfer?");

            userInteraction.ValidationParameter.PreviousDeployment = deployment;

            userInteraction.ResponseInterpretation
                .Add("default", vp => ValidateArmyTransfer(vp.PreviousDeployment, vp.Response));

            return HandleResponse(userInteraction);
        }

        private ValidationResult<Deployment> ValidateArmyTransfer(Deployment deployment, string response)
        {
            var responseValidation = new ResponseValidationBuilder<Deployment, int>()
                .Parameter(response)
                .Parameter(deployment)
                .MatchBuilder(GetIntegerMatches)
                .TestObjectBuilder((matches, validationParameter) => new Deployment
                {
                    Armies = matches[0],
                    From = deployment.From,
                    To = deployment.To
                })
                .ErrorChecks(
                    Check.SufficientArmies)
                .Build();

            return responseValidation.CheckErrors();
        }

        // Army Transfer
        public void RenderArmyTransfer(Deployment armyTransfer)
        {
            var invadingDistribution = new Deployment
            {
                Armies = -armyTransfer.Armies,
                To = armyTransfer.From
            };

            var defendingDistribution = new Deployment
            {
                Armies = armyTransfer.Armies,
                To = armyTransfer.To
            };

            // flashing country info
            for (var i = 0; i < 5; i++)
            {
                var playerColor = i % 2 == 0 ? armyTransfer.From.Occupier.Color : armyTransfer.To.Occupier.Color;
                RenderCountryInformation(armyTransfer.To, playerColor);
                Thread.Sleep(200);
            }

            // army incrementation
            IncrementArmies(invadingDistribution);
            IncrementArmies(defendingDistribution);
        }

        // Todo: Consider making this private and implementing through ArmyTransfer
        public void IncrementArmies(Deployment distribution)
        {
            Console.CursorVisible = false;

            for (var i = 0; i <= Math.Abs(distribution.Armies); i++)
            {
                var armyPosition = distribution.To.StateSpace.ArmyPosition;
                var sign = distribution.Armies / Math.Abs(distribution.Armies);

                Console.SetCursorPosition(armyPosition.Column, armyPosition.Row);
                Console.Write((distribution.To.Armies + sign * i).ToString().PadLeft(2, '0'));

                Thread.Sleep(20);
            }

            Console.CursorVisible = true;
        }
    }
}
