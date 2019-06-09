using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class CoOrdinate
    {
        public int Row { get; }
        public int Column { get; }

        public CoOrdinate() { }

        public CoOrdinate(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
