using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    public class WestLink : HorizontalLink
    {
        public LinkType Direction { get; } = LinkType.West;

        public WestLink(CountryInfo country, CountryInfo neighbour, params LinkType[] linkTypes) : base(country, neighbour, linkTypes) { }

        public override bool IsRequiredDirection()
            => IsDirectLink()
                ? Country.StateSpace.TopLeft.Column > Neighbour.StateSpace.BottomRight.Column
                : Country.StateSpace.BottomRight.Column < Neighbour.StateSpace.TopLeft.Column;

        protected override int EvaluateNodeColumn()
            => Country.StateSpace.TopLeft.Column - 1;

        protected override int EvaluateDisplacement()
            => IsDirectLink() ? Neighbour.StateSpace.BottomRight.Column - Country.StateSpace.TopLeft.Column : -3;
    }
}
