using System;
using Risk.ResponseValidation;

namespace Risk.UserInterface.ConsoleUserInterface
{
    public partial class ConsoleUserInterface
    {
        private ConsoleColor[] _consoleColors = new ConsoleColor[]
        {
            ConsoleColor.Blue,
            ConsoleColor.Green,
            ConsoleColor.Cyan,
            ConsoleColor.Red,
            ConsoleColor.Magenta,
            ConsoleColor.Yellow
        };

        public int SetUpPlayers()
        {
            var userInteraction = new UserInteractionBuilder<int>()
                .Request
                    ("How many players?")
                .ResponseInterpretations
                    (new ResponseInterpretation<int>("default", vp => ValidateNumberOfPlayers(vp)))
                .Build();

            return HandleResponse(userInteraction);
        }

        private ValidationResult<int> ValidateNumberOfPlayers(ValidationParameter<int> validationParameter)
        {
            var responseValidation = new ResponseValidationBuilder<int, int>()
                .ValidationParameter(validationParameter)
                .MatchBuilder(GetIntegerMatches)
                .TestObjectBuilder((matches, vp) => matches[0])
                .ErrorChecks(
                    Check.ValidNumberOfPlayers)
                .Build();

            return responseValidation.Validate();
        }

        public Player SetUpPlayer(int playerId)
        {
            _textbox.Write($"Player {playerId}, please enter your name:");

            var name = _textbox.Read();
            _textbox.Write();

            var color = _consoleColors[playerId - 1].ToString();

            return new Player(playerId, name, color);
        }

        public void PrepareUiForPlayer(Player player)
        {
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), player.Color);
            _textbox.Write($"{player.Name} ...");
            _textbox.Write();
        }
    }
}
