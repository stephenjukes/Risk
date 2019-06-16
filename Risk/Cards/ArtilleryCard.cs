using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class ArtilleryCard : Card
    {
        public override CardType CardType { get; } = CardType.Artillery;
        public override CountryName CountryName { get; }
        public override char Icon { get; } = '\u265c';

        public ArtilleryCard(CountryName countryName)
        {
            CountryName = countryName;
        }
    }
}
