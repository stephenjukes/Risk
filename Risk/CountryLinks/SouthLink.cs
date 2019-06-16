using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class SouthLink : VerticalLink
    {
        public LinkType Direction { get; } = LinkType.South;

        public SouthLink(CountryInfo country, CountryInfo neighbour, params LinkType[] linkTypes) : base(country, neighbour, linkTypes) { }

        public override bool IsRequiredDirection()
            => Country.StateSpace.BottomRight.Row < Neighbour.StateSpace.TopLeft.Row;

        protected override int EvaluateNodeRow()
            => Country.StateSpace.BottomRight.Row + 1;
    }
}
