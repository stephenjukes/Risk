using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Risk
{
    class CountryInfo
    {
        public int Id { get; }
        public string Name { get; }
        public StateSpace StateSpace { get; }
        public Continent Continent { get; }
        public string[] UnattachedNeighbourNames { get; }
        public List<CountryInfo> Neighbours { get; } = new List<CountryInfo>();
        public Player Occupier { get; set; }
        public int Armies { get; set; }

        public CountryInfo() { }

        public CountryInfo(int id, string name, StateSpace stateSpace, Continent continent, params string[] unattachedNeighbours)
        {
            Id = id;
            Name = name;
            StateSpace = stateSpace;
            Continent = continent;
            UnattachedNeighbourNames = unattachedNeighbours;
        }
    }
}
