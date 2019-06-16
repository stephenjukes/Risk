﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class InfantryCard : Card
    {
        public override CardType CardType { get; } = CardType.Infantry;
        public override CountryName CountryName { get; }
        public override char Icon { get; } = '\u265f';

        public InfantryCard(CountryName countryName)
        {
            CountryName = countryName;
        }
    }
}
