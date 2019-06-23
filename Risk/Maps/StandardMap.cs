using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Risk
{
    class StandardMap : Map
    {
        public StandardMap() : base() { }

        public static Dictionary<ContinentName, Continent> Continents { get; } = new Dictionary<ContinentName, Continent>
        {
            { ContinentName.NorthAmerica, new Continent(ContinentName.NorthAmerica, "DarkGray", 5) },
            { ContinentName.SouthAmerica, new Continent(ContinentName.SouthAmerica, "DarkGray", 2) },
            { ContinentName.Europe, new Continent(ContinentName.Europe, "DarkGray", 5) },
            { ContinentName.Africa, new Continent(ContinentName.Africa, "DarkGray", 3) },
            { ContinentName.Asia, new Continent(ContinentName.Asia, "DarkGray", 7) },
            { ContinentName.Oceania, new Continent(ContinentName.Oceania, "DarkGray", 2) }
        };

        public override CountryInfo[] Countries { get; } = new CountryInfo[]
        {
            new CountryInfo(CountryName.Alaska, new StateSpace(new CoOrdinate(0, 3), new CoOrdinate(9, 17)), Continents[ContinentName.NorthAmerica]),
            new CountryInfo(CountryName.NorthWestTerritory, new StateSpace(new CoOrdinate(0, 17), new CoOrdinate(6, 32)), Continents[ContinentName.NorthAmerica]),
            new CountryInfo(CountryName.Alberta, new StateSpace(new CoOrdinate(6, 17), new CoOrdinate(12, 32)), Continents[ContinentName.NorthAmerica]),
            new CountryInfo(CountryName.Ontario, new StateSpace(new CoOrdinate(3, 32), new CoOrdinate(12, 46)), Continents[ContinentName.NorthAmerica]),
            new CountryInfo(CountryName.Quebec, new StateSpace(new CoOrdinate(6, 46), new CoOrdinate(12, 60)), Continents[ContinentName.NorthAmerica]),
            new CountryInfo(CountryName.WesternUnitedStates, new StateSpace(new CoOrdinate(12, 18), new CoOrdinate(19, 38)), Continents[ContinentName.NorthAmerica]),
            new CountryInfo(CountryName.EasternUnitedStates, new StateSpace(new CoOrdinate(12, 38), new CoOrdinate(19, 52)), Continents[ContinentName.NorthAmerica]),
            new CountryInfo(CountryName.CentralAmerica, new StateSpace(new CoOrdinate(19, 27), new CoOrdinate(26, 42)), Continents[ContinentName.NorthAmerica]) ,
            new CountryInfo(CountryName.Greenland, new StateSpace(new CoOrdinate(0, 62), new CoOrdinate(7, 77)), Continents[ContinentName.NorthAmerica]),
            new CountryInfo(CountryName.Venezuala, new StateSpace(new CoOrdinate(28, 24), new CoOrdinate(34, 45)), Continents[ContinentName.SouthAmerica]),
            new CountryInfo(CountryName.Peru, new StateSpace(new CoOrdinate(34, 20), new CoOrdinate(44, 35)), Continents[ContinentName.SouthAmerica]),
            new CountryInfo(CountryName.Brazil, new StateSpace(new CoOrdinate(34, 35), new CoOrdinate(44, 49)), Continents[ContinentName.SouthAmerica]),
            new CountryInfo(CountryName.Argentina, new StateSpace(new CoOrdinate(44, 28), new CoOrdinate(55, 44)), Continents[ContinentName.SouthAmerica]),
            new CountryInfo(CountryName.Iceland, new StateSpace(new CoOrdinate(9, 64), new CoOrdinate(12, 78)), Continents[ContinentName.Europe]),
            new CountryInfo(CountryName.Britain, new StateSpace(new CoOrdinate(14, 64), new CoOrdinate(17, 78)), Continents[ContinentName.Europe]),
            new CountryInfo(CountryName.Scandinavia, new StateSpace(new CoOrdinate(11, 81), new CoOrdinate(14, 100)), Continents[ContinentName.Europe]),
            new CountryInfo(CountryName.NorthernEurope, new StateSpace(new CoOrdinate(16, 81), new CoOrdinate(20, 100)), Continents[ContinentName.Europe]),
            new CountryInfo(CountryName.Ukraine, new StateSpace(new CoOrdinate(11, 100), new CoOrdinate(23, 115)), Continents[ContinentName.Europe]),
            new CountryInfo(CountryName.WesternEurope, new StateSpace(new CoOrdinate(20, 68), new CoOrdinate(26, 83)), Continents[ContinentName.Europe]),
            new CountryInfo(CountryName.SouthernEurope, new StateSpace(new CoOrdinate(20, 83), new CoOrdinate(26, 100)), Continents[ContinentName.Europe]),
            new CountryInfo(CountryName.NorthAfrica, new StateSpace(new CoOrdinate(31, 66), new CoOrdinate(38, 86)), Continents[ContinentName.Africa]),
            new CountryInfo(CountryName.Egypt, new StateSpace(new CoOrdinate(28, 86), new CoOrdinate(34, 105)), Continents[ContinentName.Africa]),
            new CountryInfo(CountryName.Congo, new StateSpace(new CoOrdinate(38, 72), new CoOrdinate(44, 86)), Continents[ContinentName.Africa]),
            new CountryInfo(CountryName.EastAfrica, new StateSpace(new CoOrdinate(34, 86), new CoOrdinate(44, 105)), Continents[ContinentName.Africa]),
            new CountryInfo(CountryName.SouthAfrica, new StateSpace(new CoOrdinate(44, 79), new CoOrdinate(54, 93)), Continents[ContinentName.Africa]),
            new CountryInfo(CountryName.Madagascar, new StateSpace(new CoOrdinate(47, 96), new CoOrdinate(52, 113)), Continents[ContinentName.Africa]),
            new CountryInfo(CountryName.Ural, new StateSpace(new CoOrdinate(0, 117), new CoOrdinate(16, 138)), Continents[ContinentName.Asia]),
            new CountryInfo(CountryName.Algeria, new StateSpace(new CoOrdinate(0, 138), new CoOrdinate(16, 152)), Continents[ContinentName.Asia]),
            new CountryInfo(CountryName.Yakutsk, new StateSpace(new CoOrdinate(0, 152), new CoOrdinate(6, 167)), Continents[ContinentName.Asia]),
            new CountryInfo(CountryName.Irkutsk, new StateSpace(new CoOrdinate(6, 152), new CoOrdinate(12, 167)), Continents[ContinentName.Asia]),
            new CountryInfo(CountryName.Kamchatka, new StateSpace(new CoOrdinate(0, 167), new CoOrdinate(15, 183)), Continents[ContinentName.Asia]),
            new CountryInfo(CountryName.Afghanastan, new StateSpace(new CoOrdinate(16, 117), new CoOrdinate(25, 135)), Continents[ContinentName.Asia]),
            new CountryInfo(CountryName.China, new StateSpace(new CoOrdinate(16, 135), new CoOrdinate(25, 152)), Continents[ContinentName.Asia]),
            new CountryInfo(CountryName.Mongolia, new StateSpace(new CoOrdinate(12, 152), new CoOrdinate(21, 167)), Continents[ContinentName.Asia]),
            new CountryInfo(CountryName.Japan, new StateSpace(new CoOrdinate(18, 170), new CoOrdinate(23, 184)), Continents[ContinentName.Asia]),
            new CountryInfo(CountryName.MiddleEast, new StateSpace(new CoOrdinate(25, 107), new CoOrdinate(34, 120)), Continents[ContinentName.Asia]),
            new CountryInfo(CountryName.India, new StateSpace(new CoOrdinate(25, 120), new CoOrdinate(34, 138)), Continents[ContinentName.Asia]),
            new CountryInfo(CountryName.Siam, new StateSpace(new CoOrdinate(25, 138), new CoOrdinate(34, 150)), Continents[ContinentName.Asia]),
            new CountryInfo(CountryName.Indonesia, new StateSpace(new CoOrdinate(37, 136), new CoOrdinate(44, 152)), Continents[ContinentName.NorthAmerica]),
            new CountryInfo(CountryName.NewGuinea, new StateSpace(new CoOrdinate(38, 155), new CoOrdinate(45, 169)), Continents[ContinentName.NorthAmerica]),
            new CountryInfo(CountryName.Australia, new StateSpace(new CoOrdinate(47, 141), new CoOrdinate(55, 159)), Continents[ContinentName.NorthAmerica])
        };

        public override Enum[][] LinkArray { get; } = new Enum[][]
        {
            new Enum[] {CountryName.Alaska, CountryName.Kamchatka, LinkType.Indirect},
            new Enum[] {CountryName.CentralAmerica, CountryName.Venezuala},
            new Enum[] {CountryName.Greenland, CountryName.NorthWestTerritory, LinkType.North},
            new Enum[] {CountryName.Greenland, CountryName.Ontario},
            new Enum[] {CountryName.Greenland, CountryName.Quebec},
            new Enum[] {CountryName.Greenland, CountryName.Iceland},
            new Enum[] {CountryName.Brazil, CountryName.NorthAfrica},
            new Enum[] {CountryName.Iceland, CountryName.Britain},
            new Enum[] {CountryName.Iceland , CountryName.Scandinavia},
            new Enum[] {CountryName.Britain, CountryName.Scandinavia},
            new Enum[] {CountryName.Britain, CountryName.NorthernEurope},
            new Enum[] {CountryName.Britain, CountryName.WesternEurope},
            new Enum[] {CountryName.Scandinavia, CountryName.NorthernEurope},
            new Enum[] {CountryName.Ukraine, CountryName.Ural},
            new Enum[] {CountryName.Ukraine, CountryName.Afghanastan},
            new Enum[] {CountryName.Ukraine, CountryName.MiddleEast},
            new Enum[] {CountryName.WesternEurope, CountryName.NorthAfrica},
            new Enum[] {CountryName.SouthernEurope, CountryName.NorthAfrica},
            new Enum[] {CountryName.SouthernEurope, CountryName.Egypt},
            new Enum[] {CountryName.SouthernEurope, CountryName.MiddleEast},
            new Enum[] {CountryName.EastAfrica, CountryName.Madagascar},
            new Enum[] {CountryName.Egypt, CountryName.MiddleEast},
            new Enum[] {CountryName.SouthAfrica, CountryName.Madagascar},
            new Enum[] {CountryName.Kamchatka, CountryName.Japan},
            new Enum[] {CountryName.Mongolia, CountryName.Japan},
            new Enum[] {CountryName.Siam, CountryName.Indonesia},
            new Enum[] {CountryName.Indonesia, CountryName.NewGuinea},
            new Enum[] {CountryName.Indonesia, CountryName.Australia},
            new Enum[] {CountryName.NewGuinea, CountryName.Australia}
        };
    }
}
