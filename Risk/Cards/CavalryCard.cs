using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class CavalryCard : Card
    {
        public override CardType CardType { get; } = CardType.Cavalry;
        public override Country CountryName { get; }
        public override char Icon { get; } = '\u265e';

        public CavalryCard(Country countryName)
        {
            CountryName = countryName;
        }
    }
}
