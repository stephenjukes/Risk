using System;
using System.Collections.Generic;
using System.Linq;

namespace Risk
{
    abstract public class Map
    {
        public Map() { Links = CreateLinks(LinkArray); }

        public static Dictionary<ContinentName, Continent> Continents { get; }
        public abstract CountryInfo[] Countries { get; }
        public abstract Enum[][] LinkArray { get; }
        public Link[] Links { get; }

        private Link[] CreateLinks(Enum[][] linkArrays)
        {
            return linkArrays.Select(l =>
            {
                var country = Array.Find(Countries, c => c.Name == (CountryName)l[0]);
                var neighbour = Array.Find(Countries, c => c.Name == (CountryName)l[1]);
                var linkTypes = l
                    .Where(e => e is LinkType)
                    .Select(e => (LinkType)Enum.Parse(typeof(LinkType), e.ToString()))
                    .ToArray();

                return new Link(country, neighbour, linkTypes);
            }).ToArray();
        }
    }
}