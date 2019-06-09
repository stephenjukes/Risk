using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class Player
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public bool IsActive { get; set; } = true;
        public List<Card> Cards { get; set; } = new List<Card>();
        public int CardSetIncome { get; set; } = 2;
        public bool hasEarnedCard { get; set; }

        public Player() { }

        public Player(string name, string color)
        {
            Name = name;
            Color = color;
        }
    }
}
