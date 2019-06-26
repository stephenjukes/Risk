using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    public class StateSpace
    {
        public CoOrdinate TopLeft { get; }
        public CoOrdinate BottomRight { get; }
        public CoOrdinate ArmyPosition { get; set; }

        public StateSpace(CoOrdinate topLeft, CoOrdinate bottomRight)
        {
            TopLeft = topLeft;
            BottomRight = bottomRight;
        }
    }
}
