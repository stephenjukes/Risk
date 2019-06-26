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
            var userInteraction = new UserInteraction<int>();

            userInteraction.Request
                .Add("How many players?");

            userInteraction.ResponseInterpretation
                .Add("default", vp => ValidateNumberOfPlayers(vp.Response));

            return HandleResponse(userInteraction);
        }

        private ValidationResult<int> ValidateNumberOfPlayers(string response)
        {
            var responseValidation = new ResponseValidationBuilder<int, int>()
                .Parameter(response)
                .MatchBuilder(GetIntegerMatches)
                .TestObjectBuilder((matches, validationParameter) => matches[0])
                .ErrorChecks(
                    Check.ValidNumberOfPlayers)
                .Build();

            return responseValidation.CheckErrors();
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
            //_textbox.Clear();
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), player.Color);
            _textbox.Write($"{player.Name} ...");
            _textbox.Write();
        }
    }
}
