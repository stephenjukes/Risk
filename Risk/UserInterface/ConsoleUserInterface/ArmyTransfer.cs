using Risk.ResponseValidation;
using System;
using System.Threading;

namespace Risk.UserInterface.ConsoleUserInterface
{
    public partial class ConsoleUserInterface
    {
        public Deployment GetArmyTransfer(Deployment deployment)
        {
            var userInteraction = new UserInteractionBuilder<Deployment>()
                .Request("How many armies do you wish to transfer?")
                .ResponseInterpretations(
                    new ResponseInterpretation<Deployment>("default", vp => ValidateArmyTransfer(vp)))
                .Deployment(deployment)
                .Build();

            return HandleResponse(userInteraction);
        }

        private ValidationResult<Deployment> ValidateArmyTransfer(ValidationParameter<Deployment> validationParameter)
        {
            var responseValidation = new ResponseValidationBuilder<Deployment, int>()
                .ValidationParameter(validationParameter)
                .MatchBuilder(GetIntegerMatches)
                .TestObjectBuilder((matches, vp) => new Deployment
                {
                    Armies = matches[0],
                    From = vp.PreviousDeployment.From,
                    To = vp.PreviousDeployment.To
                })
                .ErrorChecks(
                    Check.SufficientArmies)
                .Build();

            return responseValidation.Validate();
        }

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

            TransformDefendingCountry(armyTransfer);
            IncrementArmies(invadingDistribution, defendingDistribution);
        }

        private void TransformDefendingCountry(Deployment armyTransfer)
        {
            if (armyTransfer.From.Occupier.Id == armyTransfer.To.Occupier.Id)
                return;

            for (var i = 0; i < 5; i++)
            {
                var playerColor = i % 2 == 0 ? armyTransfer.From.Occupier.Color : armyTransfer.To.Occupier.Color;
                RenderCountryInformation(armyTransfer.To, playerColor);
                Thread.Sleep(200);
            }
        }

        // Todo: Consider making this private and implementing through ArmyTransfer
        public void IncrementArmies(params Deployment[] distributions)
        {
            foreach(var distribution in distributions)
            {
                IncrementArmy(distribution);
            }
        }

        private void IncrementArmy(Deployment distribution)
        {
            Console.CursorVisible = false;

            for (var i = 0; i <= Math.Abs(distribution.Armies); i++)
            {
                var armyPosition = distribution.To.StateSpace.ArmyPosition;
                var sign = distribution.Armies / Math.Abs(distribution.Armies);

                Console.SetCursorPosition(armyPosition.Column, armyPosition.Row);
                Console.Write((distribution.To.Armies + sign * i).ToString().PadLeft(2, '0'));

                Thread.Sleep(50);
            }

            Console.CursorVisible = true;
        }
    }
}
