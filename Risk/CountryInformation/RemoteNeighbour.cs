using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class RemoteNeighbour : CountryInfo
    {
        public RemoteNeighbour() { }
        public RemoteNeighbour(StateSpace stateSpace, Continent continent) : base(stateSpace, continent) { }
    }
}
