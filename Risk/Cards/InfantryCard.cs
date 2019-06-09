using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class InfantryCard : Card
    {
        public override CardType CardType { get; } = CardType.Infantry;
        public override int Id { get; }
        public override string CountryName { get; }
        public override char Icon { get; } = '\u265f';

        public InfantryCard(int id, string countryName)
        {
            Id = id;
            CountryName = countryName;
        }
    }
}
