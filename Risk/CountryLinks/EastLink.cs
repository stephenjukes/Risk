using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    public class EastLink : HorizontalLink
    {
        public LinkType Direction { get; } = LinkType.East;

        public EastLink(CountryInfo country, CountryInfo neighbour, params LinkType[] linkTypes) : base(country, neighbour, linkTypes) { }

        public override bool IsRequiredDirection()
            => IsDirectLink()
                ? Country.StateSpace.BottomRight.Column < Neighbour.StateSpace.TopLeft.Column
                : Country.StateSpace.TopLeft.Column > Neighbour.StateSpace.BottomRight.Column;

        protected override int EvaluateNodeColumn()
            => Country.StateSpace.BottomRight.Column + 1;

        protected override int EvaluateDisplacement()
            => IsDirectLink() ? Neighbour.StateSpace.TopLeft.Column - Country.StateSpace.BottomRight.Column : 3;
    }
}
