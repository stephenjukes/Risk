using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class Card
    {
        public virtual CardType CardType { get; }
        public virtual Country CountryName { get; }
        public virtual int Id { get; }
        public virtual char Icon { get; }
    }
}
