using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Risk
{
    public class TextBox
    {
        private StateSpace _stateSpace;
        private int _width;
        private int _height;
        private CoOrdinate _nextLine;
        private List<string> _lines = new List<string>();

        public TextBox(StateSpace stateSpace)
        {
            _stateSpace = stateSpace;
            _nextLine = stateSpace.TopLeft;
            _width = stateSpace.BottomRight.Column - stateSpace.TopLeft.Column;
            _height = stateSpace.BottomRight.Row - stateSpace.TopLeft.Row + 1;
        }

        public void Write(string message = "")
        {
            var lineBreaks = message.Split('\n');
            foreach (var lineBreak in lineBreaks)
            {
                ConstructLines(lineBreak);             
            }

            var textboxContent = ExtractTextboxContent();
            PrintToTextbox(textboxContent);          
        }

        private List<string> ExtractTextboxContent()
        {
            var startIndex = 0;
            var count = _lines.Count;

            if (count > _height)
            {
                startIndex = count - _height;
                count = _height;
                WipeText();
            }

            return _lines.GetRange(startIndex, count);
        }

        private void ConstructLines(string lineBreak)
        {
            var textboxLine = new StringBuilder();
            var words = lineBreak.Split(" ");
            foreach (var word in words)
            {
                if ($"{textboxLine} {word}".Length <= _width)
                {
                    textboxLine.Append($" {word}");
                }
                else
                {
                    _lines.Add(textboxLine.ToString().Trim(' '));
                    textboxLine = new StringBuilder(word);
                }
            }

            _lines.Add(textboxLine.ToString().Trim(' ')); // is there a better way than just to duplicate the else block?
        }

        private void PrintToTextbox(List<string> textboxContent)
        {
            var i = _stateSpace.TopLeft.Row;
            foreach (var line in textboxContent)
            {
                Console.SetCursorPosition(_stateSpace.TopLeft.Column, i);
                Console.WriteLine(line);
                i++;
            }
        }

        public string Read()
        {
            var row = _lines.Count < _height ? _lines.Count : _height;
            Console.SetCursorPosition(_stateSpace.TopLeft.Column, row + 1);

            var response = Console.ReadLine();

            _lines.Add(response);
            return response;
        }

        public void Clear()
        {
            _lines.Clear();
            WipeText();
        }

        private void WipeText()
        {
            var top = _stateSpace.TopLeft.Row;
            var blankLine = String.Join(" ", new string[_width]) + " ";

            for (var i = 0; i <= _stateSpace.BottomRight.Row; i++)
            {
                Console.SetCursorPosition(_stateSpace.TopLeft.Column, top + i);
                Console.WriteLine(blankLine);
            };
        }


    }
}