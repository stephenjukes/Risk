using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class Link
    {
        protected readonly CountryInfo _country;
        protected readonly CountryInfo _neighbour;
        public CoOrdinate Node { get; }
        public int Displacement { get; }
        public virtual LinkType Orientation { get;}

        public Link(CountryInfo country, CountryInfo neighbour)
        {
            _country = country;
            _neighbour = neighbour;
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
    }

    class HorizontalLink : Link
    {
        public HorizontalLink(CountryInfo country, CountryInfo neighbour) : base(country, neighbour) { }

        public override LinkType Orientation { get; } = LinkType.Horizontal;

        protected override int EvaluateNodeRow() 
            =>  Math.Max(_country.StateSpace.TopLeft.Row, _neighbour.StateSpace.TopLeft.Row) / 2 +
                Math.Min(_country.StateSpace.BottomRight.Row, _neighbour.StateSpace.BottomRight.Row) / 2;

        protected override int EvaluateDisplacement()
            => _neighbour.StateSpace.TopLeft.Column - _country.StateSpace.BottomRight.Column;
    }

    class VerticalLink : Link
    {
        public override LinkType Orientation { get; } = LinkType.Vertical;

        public VerticalLink(CountryInfo country, CountryInfo neighbour) : base(country, neighbour) { }

        protected override int EvaluateNodeColumn()
            =>  Math.Max(_country.StateSpace.TopLeft.Column, _neighbour.StateSpace.TopLeft.Column) / 2 +
                Math.Min(_country.StateSpace.BottomRight.Column, _neighbour.StateSpace.BottomRight.Column) / 2;

        protected override int EvaluateDisplacement()
            => _neighbour.StateSpace.TopLeft.Row - _country.StateSpace.BottomRight.Row;
    }

    class WestLink : HorizontalLink
    {
        public LinkType Direction { get; } = LinkType.West;

        public WestLink(CountryInfo country, CountryInfo neighbour) : base(country, neighbour) { }

        public override bool IsThisDirection()
            => _country.StateSpace.TopLeft.Column > _neighbour.StateSpace.BottomRight.Column;

        protected override int EvaluateNodeColumn()
            => _country.StateSpace.TopLeft.Column - 1;
    }

    class EastLink : HorizontalLink
    {
        public LinkType Direction { get; } = LinkType.East;

        public EastLink(CountryInfo country, CountryInfo neighbour) : base(country, neighbour) { }

        public override bool IsThisDirection()
            => _country.StateSpace.BottomRight.Column < _neighbour.StateSpace.TopLeft.Column;

        protected override int EvaluateNodeColumn()
            => _country.StateSpace.BottomRight.Column + 1;
    }

    class NorthLink : VerticalLink
    {
        public LinkType Direction { get; } = LinkType.North;

        public NorthLink(CountryInfo country, CountryInfo neighbour) : base(country, neighbour) { }

        public override bool IsThisDirection()
            => _country.StateSpace.TopLeft.Row > _neighbour.StateSpace.BottomRight.Row;

        protected override int EvaluateNodeRow()
            => _country.StateSpace.BottomRight.Row - 1;
    }

    class SouthLink : VerticalLink
    {
        public LinkType Direction { get; } = LinkType.South;

        public SouthLink(CountryInfo country, CountryInfo neighbour) : base(country, neighbour) { }

        public override bool IsThisDirection()
            => _country.StateSpace.BottomRight.Row < _neighbour.StateSpace.TopLeft.Row;

        protected override int EvaluateNodeRow()
            => _country.StateSpace.BottomRight.Row + 1;
    }

    enum LinkType
    {
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
