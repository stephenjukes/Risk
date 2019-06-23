using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class Card
    {
        public CardType CardType { get; }
        public CountryName CountryName { get; }
        public char Icon { get; }

        public Card(CardType cardType, CountryName countryName)
        {
            CardType = cardType;
            CountryName = countryName;
        }
    }
}
