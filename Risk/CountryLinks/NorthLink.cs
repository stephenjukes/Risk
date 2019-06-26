using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    public class NorthLink : VerticalLink
    {
        public LinkType Direction { get; } = LinkType.North;

        public NorthLink(CountryInfo country, CountryInfo neighbour, params LinkType[] linkTypes) : base(country, neighbour, linkTypes) { }

        public override bool IsRequiredDirection()
            => Country.StateSpace.TopLeft.Row > Neighbour.StateSpace.BottomRight.Row;

        protected override int EvaluateNodeRow()
            => Country.StateSpace.BottomRight.Row - 1;
    }
}
