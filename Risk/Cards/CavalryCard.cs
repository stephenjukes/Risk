using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class CavalryCard : Card
    {
        public override CardType CardType { get; } = CardType.Cavalry;
        public override CountryName CountryName { get; }
        public override char Icon { get; } = '\u265e';

        public CavalryCard(CountryName countryName)
        {
            CountryName = countryName;
        }
    }
}
