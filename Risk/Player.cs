﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    public class Player
    {
        public int Id { get; }
        public string Name { get; set; }
        public string Color { get; set; }
        public bool IsActive { get; set; } = true;
        public List<Card> Cards { get; set; } = new List<Card>();
        public int OpeningIncome { get; set; }
        public int CardSetIncome { get; set; } = 2;
        public bool hasEarnedCard { get; set; }

        public Player() { }

        public Player(int id, string name, string color)
        {
            Id = id;
            Name = name;
            Color = color;
        }
    }
}
