using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class Continent
    {
        public string Name { get; }
        public int ArmyProvisionForMonpoly { get; }
        public int Size { get; }

        public Continent(string name, int armyProvisionForMonopoly) : this(name, armyProvisionForMonopoly, 0) { }

        public Continent(string name, int armyProvisionForMonopoly, int size)
        {
            Name = name;
            ArmyProvisionForMonpoly = armyProvisionForMonopoly;
            Size = size;          
        }
    }
}
