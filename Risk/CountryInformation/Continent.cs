using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class Continent
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int ArmyProvisionForMonpoly { get; set; }
        public int Size { get; set; }

        public Continent() { }

        public Continent(string name, string color, int armyProvisionForMonopoly)
        {
            Name = name;
            Color = color;
            ArmyProvisionForMonpoly = armyProvisionForMonopoly;
        }

        //public Continent(string name, int armyProvisionForMonopoly, int size)
        //{
        //    Name = name;
        //    ArmyProvisionForMonpoly = armyProvisionForMonopoly;
        //    Size = size;          
        //}
    }
}
