using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class VerticalLink : Link
    {
        public VerticalLink(CountryInfo country, CountryInfo neighbour, params LinkType[] linkTypes) : base(country, neighbour, linkTypes) { }

        protected override int EvaluateNodeColumn()
        {
            var linkPosition = GetLinkType(LinkType.East, LinkType.West);
            var leftMostEastCoast = Math.Min(Country.StateSpace.BottomRight.Column, Neighbour.StateSpace.BottomRight.Column);
            var rightMostWestCoast = Math.Max(Country.StateSpace.TopLeft.Column, Neighbour.StateSpace.TopLeft.Column);

            switch (linkPosition)
            {
                case LinkType.East:
                    return leftMostEastCoast;
                case LinkType.West:
                    return rightMostWestCoast;
                default:
                    return leftMostEastCoast / 2 + rightMostWestCoast / 2;
            }
        }

        protected override int EvaluateDisplacement()
            => Neighbour.StateSpace.TopLeft.Row - Country.StateSpace.BottomRight.Row;
    }
}
