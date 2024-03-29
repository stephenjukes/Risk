﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Risk
{
    public class CountryInfo
    {
        public CountryName Name { get; set; }
        public StateSpace StateSpace { get; }
        public Continent Continent { get; }
        public List<CountryInfo> RemoteNeighbours { get; } = new List<CountryInfo>();
        public List<CountryInfo> Neighbours { get; } = new List<CountryInfo>();
        public Player Occupier { get; set; }
        public int Armies { get; set; } = 1;

        public CountryInfo() { }

        public CountryInfo(CountryName name, StateSpace stateSpace, Continent continent)
        {
            Name = name;
            StateSpace = stateSpace;
            Continent = continent;
        }
    }
}
