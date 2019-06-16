using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Risk
{
    class Link
    {
        public CountryInfo Country { get; }
        public CountryInfo Neighbour { get; }
        public CoOrdinate Node { get; }
        public int Displacement { get; }
        public LinkType[] LinkTypes { get; set; } = new LinkType[0];
        public virtual LinkType Orientation { get;}

        public Link(CountryInfo country, CountryInfo neighbour, params LinkType[] linkTypes)
        {
            Country = country;
            Neighbour = neighbour;
            LinkTypes = linkTypes;
            Node = EvaluateNode();
            Displacement = EvaluateDisplacement();         
        }

        protected virtual int EvaluateNodeColumn() => default(int);
        protected virtual int EvaluateNodeRow() => default(int);
        protected virtual int EvaluateDisplacement() => default(int);
        public virtual bool IsThisDirection() => default(bool);

        protected CoOrdinate EvaluateNode()
        {
            return new CoOrdinate
            {
                Row = EvaluateNodeRow(),
                Column = EvaluateNodeColumn()
            };
        }

        protected LinkType GetLinkType(params LinkType[] requestedLinkTypes)
            => requestedLinkTypes.Where(l => LinkTypes.Contains(l)).FirstOrDefault();


        protected bool IsDirectLink()
            => GetLinkType(LinkType.Indirect) != LinkType.Indirect;
    }

    class HorizontalLink : Link
    {
        public HorizontalLink(CountryInfo country, CountryInfo neighbour, params LinkType[] linkTypes) : base(country, neighbour, linkTypes) { }

        public override LinkType Orientation { get; } = LinkType.Horizontal;

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

    class VerticalLink : Link
    {
        public override LinkType Orientation { get; } = LinkType.Vertical;

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

    class WestLink : HorizontalLink
    {
        public LinkType Direction { get; } = LinkType.West;

        public WestLink(CountryInfo country, CountryInfo neighbour, params LinkType[] linkTypes) : base(country, neighbour, linkTypes) { }

        public override bool IsThisDirection()
            => Country.StateSpace.TopLeft.Column > Neighbour.StateSpace.BottomRight.Column;

        protected override int EvaluateNodeColumn()
            => IsDirectLink() ? Country.StateSpace.TopLeft.Column - 1 : Country.StateSpace.BottomRight.Column + 1;

        protected override int EvaluateDisplacement()
            => IsDirectLink() ? Neighbour.StateSpace.BottomRight.Column - Country.StateSpace.TopLeft.Column : 3;
    }

    class EastLink : HorizontalLink
    {
        public LinkType Direction { get; } = LinkType.East;

        public EastLink(CountryInfo country, CountryInfo neighbour, params LinkType[] linkTypes) : base(country, neighbour, linkTypes) { }

        public override bool IsThisDirection()
            => Country.StateSpace.BottomRight.Column < Neighbour.StateSpace.TopLeft.Column;

        protected override int EvaluateNodeColumn()
            => IsDirectLink() ? Country.StateSpace.BottomRight.Column + 1 : Country.StateSpace.TopLeft.Column - 1;

        protected override int EvaluateDisplacement()
            => IsDirectLink() ? Neighbour.StateSpace.TopLeft.Column - Country.StateSpace.BottomRight.Column : -3;
    }

    class NorthLink : VerticalLink
    {
        public LinkType Direction { get; } = LinkType.North;

        public NorthLink(CountryInfo country, CountryInfo neighbour, params LinkType[] linkTypes) : base(country, neighbour, linkTypes) { }

        public override bool IsThisDirection()
            => Country.StateSpace.TopLeft.Row > Neighbour.StateSpace.BottomRight.Row;

        protected override int EvaluateNodeRow()
            => Country.StateSpace.BottomRight.Row - 1;
    }

    class SouthLink : VerticalLink
    {
        public LinkType Direction { get; } = LinkType.South;

        public SouthLink(CountryInfo country, CountryInfo neighbour, params LinkType[] linkTypes) : base(country, neighbour, linkTypes) { }

        public override bool IsThisDirection()
            => Country.StateSpace.BottomRight.Row < Neighbour.StateSpace.TopLeft.Row;

        protected override int EvaluateNodeRow()
            => Country.StateSpace.BottomRight.Row + 1;
    }

    enum LinkType
    {
        Default = 1,
        North,
        South,
        East,
        West,
        Horizontal,
        Vertical,
        Direct,
        Indirect
    }
}
