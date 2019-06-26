using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    public class RemoteNeighbour : CountryInfo
    {
        public RemoteNeighbour() { }
        public RemoteNeighbour(CountryName name, StateSpace stateSpace, Continent continent) : base(name, stateSpace, continent) { }
    }
}
