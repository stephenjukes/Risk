using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    public class CoOrdinate
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public CoOrdinate() { }

        public CoOrdinate(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
