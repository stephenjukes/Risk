using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class CavalryCard : Card
    {
        public override CardType CardType { get; } = CardType.Cavalry;
        public override int Id { get; }
        public override string CountryName { get; }
        public override char Icon { get; } = '\u265e';

        public CavalryCard(int id, string countryName)
        {
            Id = id;
            CountryName = countryName;
        }
    }
}
