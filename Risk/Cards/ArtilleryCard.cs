using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class ArtilleryCard : Card
    {
        public override CardType CardType { get; } = CardType.Artillery;
        public override int Id { get; }
        public override string CountryName { get; }
        public override char Icon { get; } = '\u265c';

        public ArtilleryCard(int id, string countryName)
        {
            Id = id;
            CountryName = countryName;
        }
    }
}
