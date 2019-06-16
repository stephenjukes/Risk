using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class WildCard : Card
    {
        public override CardType CardType { get; } = CardType.Wild;
        public override int Id { get; }
        public override Country CountryName { get; }
        public override char Icon { get; } = '*';

        public WildCard(Country countryName)
        {
            CountryName = countryName;
        }
    }
}
