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
        public CoOrdinate Node { get; set; }
        public int Displacement { get; set; }
        public LinkType[] LinkTypes { get; set; } = new LinkType[0];

        public Link(CountryInfo country, CountryInfo neighbour, params LinkType[] linkTypes)
        {
            Country = country;
            Neighbour = neighbour;
            LinkTypes = linkTypes;       
        }

        protected virtual int EvaluateNodeColumn() => default(int);
        protected virtual int EvaluateNodeRow() => default(int);
        protected virtual int EvaluateDisplacement() => default(int);
        public virtual bool IsRequiredDirection() => default(bool);

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
            => !LinkTypes.Contains(LinkType.Indirect);

        public Link EvaluateParameters()
        {
            Node = EvaluateNode();
            Displacement = EvaluateDisplacement();
            return this;
        }
    }
}
