using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class WildCard : Card
    {
        public override CardType CardType { get; } = CardType.Wild;
        public override int Id { get; }
        public override string CountryName { get; }
        public override char Icon { get; } = '*';

        public WildCard(int id, string countryName)
        {
            Id = id;
            CountryName = countryName;
        }
    }
}
