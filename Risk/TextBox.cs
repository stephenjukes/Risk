﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Risk
{
    class TextBox
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

            //_lines.AddRange(lines);

            var startIndex = 0;
            var count = _lines.Count;
 
            if (count > _height)
            {
                startIndex = count - _height;
                count = _height;
                WipeText();
            }

            var textboxContent = _lines.GetRange(startIndex, count);

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


//private void TextboxWrite(string message)
//{
//    Console.SetCursorPosition(_nextLine.Column, _nextLine.Row);
//    Console.Write(message);

//    _nextLine = new CoOrdinate(Console.CursorTop + 1, _textbox.TopLeft.Column);
//}

//private string TextboxRead()
//{
//    Console.SetCursorPosition(_nextLine.Column, _nextLine.Row);
//    _nextLine = new CoOrdinate(Console.CursorTop + 1, _textbox.TopLeft.Column);

//    return Console.ReadLine();
//}

//private void TextboxReset()
//{
//    var emptyLine = String.Join(" ", new string[_textbox.BottomRight.Column - _textbox.TopLeft.Column + 10]);
//    for (var row = _textbox.TopLeft.Row; row < _textbox.BottomRight.Row; row++)
//    {
//        Console.SetCursorPosition(_textbox.TopLeft.Column, row);
//        Console.WriteLine(emptyLine);
//    }

//    _nextLine = new CoOrdinate(_textbox.TopLeft.Row, _textbox.TopLeft.Column);
//}