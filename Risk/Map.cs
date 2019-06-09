using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class Map
    {
        public char[,] StringArrayImage { get; set; }
        char _marker;

        public Map(byte[,] worldByteArray, char marker)
        {
            _marker = marker;
            StringArrayImage = CreateStringArrayImage(worldByteArray);
        }


        public char[,] CreateStringArrayImage(byte[,] worldByteArray)
        {
            var worldArray = new char[worldByteArray.GetUpperBound(0), worldByteArray.GetUpperBound(1)];
            for (var row = 0; row < worldByteArray.GetUpperBound(0); row++)
            {
                for (var value = 0; value < worldByteArray.GetUpperBound(1); value++)
                {
                    worldArray[row, value] = worldByteArray[row, value] == 1 ? _marker : ' ';
                }
            }

            return worldArray;
        }

        public string CreateStringImage()
        {
            var worldString = new StringBuilder();

            for (var row = 0; row <= StringArrayImage.GetUpperBound(0); row++)
            {
                for (var value = 0; value <= StringArrayImage.GetUpperBound(1); value++)
                {
                    worldString.Append(StringArrayImage[row, value]);
                }
                worldString.Append('\n');
            }

            return worldString.ToString();
        }
    }
}
