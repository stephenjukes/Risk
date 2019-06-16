using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class HorizontalLink : Link
    {
        public HorizontalLink(CountryInfo country, CountryInfo neighbour, params LinkType[] linkTypes) : base(country, neighbour, linkTypes) { }

        protected override int EvaluateNodeRow()
        {
            var linkPosition = GetLinkType(LinkType.North, LinkType.South);
            var lowestNorthCoast = Math.Max(Country.StateSpace.TopLeft.Row, Neighbour.StateSpace.TopLeft.Row);
            var highestSouthCoast = Math.Min(Country.StateSpace.BottomRight.Row, Neighbour.StateSpace.BottomRight.Row);

            switch (linkPosition)
            {
                case LinkType.North:
                    return lowestNorthCoast;
                case LinkType.South:
                    return highestSouthCoast;
                default:
                    return lowestNorthCoast / 2 + highestSouthCoast / 2;
            }
        }
    }
}
