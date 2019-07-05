using System;

namespace Risk.UserInterface.ConsoleUserInterface
{
    public partial class ConsoleUserInterface : IUserInterface
    {
        public TextBox _textbox { get; }

        public ConsoleUserInterface(TextBox textBox)
        {
            _textbox = textBox;
        }

        public void AnnounceWinner(Player currentPlayer)
        {
            _textbox.Clear();
            _textbox.Write($"{currentPlayer.Name} wins!!!".ToUpper());
            _textbox.Write();
        }
    }
}

