using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    public class Continent
    {
        public ContinentName Name { get; set; }
        public string Color { get; set; }
        public int ArmyProvisionForMonpoly { get; set; }
        public int Size { get; set; }

        public Continent() { }

        public Continent(ContinentName name, string color, int armyProvisionForMonopoly)
        {
            Name = name;
            Color = color;
            ArmyProvisionForMonpoly = armyProvisionForMonopoly;
        }
    }
}
