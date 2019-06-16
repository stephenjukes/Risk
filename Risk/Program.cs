using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    class Program
    {
        public static void Main(string[] args)
        {
            //var textBox = new TextBox(new StateSpace(new CoOrdinate(10, 10), new CoOrdinate(20, 20)));
            //textBox.Write("The quick brown\nfox jumps over the lazy dog");

            var northAmerica = new Continent("North America", "DarkGray", 5);
            var southAmerica = new Continent("South America", "Gray", 2);
            var europe = new Continent("Europe", "Gray", 5);
            var africa = new Continent("Africa", "DarkGray", 3);
            var asia = new Continent("Asia", "DarkGray", 7);
            var oceania = new Continent("Oceania", "Gray", 2);

            var countries = new Dictionary<Country, CountryInfo>
            {
                { Country.Alaska, new CountryInfo(new StateSpace(new CoOrdinate(0, 3), new CoOrdinate(9, 17)), northAmerica) },
                { Country.NorthWestTerritory, new CountryInfo(new StateSpace(new CoOrdinate(0, 17), new CoOrdinate(6, 32)), northAmerica) },
                { Country.Alberta, new CountryInfo(new StateSpace(new CoOrdinate(6, 17), new CoOrdinate(12, 32)), northAmerica)},
                { Country.Ontario, new CountryInfo(new StateSpace(new CoOrdinate(3, 32), new CoOrdinate(12, 46)), northAmerica) },
                { Country.Quebec, new CountryInfo(new StateSpace(new CoOrdinate(6, 46), new CoOrdinate(12, 60)), northAmerica)},
                { Country.WesternUnitedStates, new CountryInfo(new StateSpace(new CoOrdinate(12, 18), new CoOrdinate(19, 38)), northAmerica)},
                { Country.EasternUnitedStates, new CountryInfo(new StateSpace(new CoOrdinate(12, 38), new CoOrdinate(19, 52)), northAmerica)},
                { Country.CentralAmerica, new CountryInfo(new StateSpace(new CoOrdinate(19, 27), new CoOrdinate(26, 42)), northAmerica) },
                { Country.Greenland, new CountryInfo(new StateSpace(new CoOrdinate(0, 62), new CoOrdinate(7, 77)), northAmerica)},
                { Country.Venezuala, new CountryInfo(new StateSpace(new CoOrdinate(28, 24), new CoOrdinate(34, 45)), southAmerica)},
                { Country.Peru, new CountryInfo(new StateSpace(new CoOrdinate(34, 20), new CoOrdinate(44, 35)), southAmerica)},
                { Country.Brazil, new CountryInfo(new StateSpace(new CoOrdinate(34, 35), new CoOrdinate(44, 49)), southAmerica)},
                { Country.Argentina, new CountryInfo(new StateSpace(new CoOrdinate(44, 28), new CoOrdinate(55, 44)), southAmerica) },
                { Country.Iceland, new CountryInfo(new StateSpace(new CoOrdinate(9, 64), new CoOrdinate(12, 78)), europe)},
                { Country.Britain, new CountryInfo(new StateSpace(new CoOrdinate(14, 64), new CoOrdinate(17, 78)), europe)},
                { Country.Scandinavia, new CountryInfo(new StateSpace(new CoOrdinate(11, 81), new CoOrdinate(14, 100)), europe)},
                { Country.NorthernEurope, new CountryInfo(new StateSpace(new CoOrdinate(16, 81), new CoOrdinate(20, 100)), europe)},
                { Country.Ukraine, new CountryInfo(new StateSpace(new CoOrdinate(11, 100), new CoOrdinate(23, 115)), europe)},
                { Country.WesternEurope, new CountryInfo(new StateSpace(new CoOrdinate(20, 68), new CoOrdinate(26, 83)), europe)},
                { Country.SouthernEurope, new CountryInfo(new StateSpace(new CoOrdinate(20, 83), new CoOrdinate(26, 100)), europe)},
                { Country.NorthAfrica, new CountryInfo(new StateSpace(new CoOrdinate(31, 66), new CoOrdinate(38, 86)), africa)},
                { Country.Egypt, new CountryInfo(new StateSpace(new CoOrdinate(28, 86), new CoOrdinate(34, 105)), africa)},
                { Country.Congo, new CountryInfo(new StateSpace(new CoOrdinate(38, 72), new CoOrdinate(44, 86)), africa) },
                { Country.EastAfrica, new CountryInfo(new StateSpace(new CoOrdinate(34, 86), new CoOrdinate(44, 105)), africa)},
                { Country.SouthAfrica, new CountryInfo(new StateSpace(new CoOrdinate(44, 79), new CoOrdinate(54, 93)), africa)},
                { Country.Madagascar, new CountryInfo(new StateSpace(new CoOrdinate(47, 96), new CoOrdinate(52, 113)), africa)},
                { Country.Ural, new CountryInfo(new StateSpace(new CoOrdinate(0, 117), new CoOrdinate(16, 138)), asia)},
                { Country.Algeria, new CountryInfo(new StateSpace(new CoOrdinate(0, 138), new CoOrdinate(16, 152)), asia)},
                { Country.Yakutsk, new CountryInfo(new StateSpace(new CoOrdinate(0, 152), new CoOrdinate(6, 167)), asia)},
                { Country.Irkutsk, new CountryInfo(new StateSpace(new CoOrdinate(6, 152), new CoOrdinate(12, 167)), asia)},
                { Country.Kamchatka, new CountryInfo(new StateSpace(new CoOrdinate(0, 167), new CoOrdinate(15, 183)), asia)},
                { Country.Afghanastan, new CountryInfo(new StateSpace(new CoOrdinate(16, 117), new CoOrdinate(25, 135)), asia)},
                { Country.China, new CountryInfo(new StateSpace(new CoOrdinate(16, 135), new CoOrdinate(25, 152)), asia)},
                { Country.Mongolia, new CountryInfo(new StateSpace(new CoOrdinate(12, 152), new CoOrdinate(21, 167)), asia)},
                { Country.Japan, new CountryInfo(new StateSpace(new CoOrdinate(18, 170), new CoOrdinate(23, 184)), asia)},
                { Country.MiddleEast, new CountryInfo(new StateSpace(new CoOrdinate(25, 107), new CoOrdinate(34, 120)), asia)},
                { Country.India, new CountryInfo(new StateSpace(new CoOrdinate(25, 120), new CoOrdinate(34, 138)), asia) },
                { Country.Siam, new CountryInfo(new StateSpace(new CoOrdinate(25, 138), new CoOrdinate(34, 150)), asia)},
                { Country.Indonesia, new CountryInfo(new StateSpace(new CoOrdinate(37, 136), new CoOrdinate(44, 152)), oceania)},
                { Country.NewGuinea, new CountryInfo(new StateSpace(new CoOrdinate(38, 155), new CoOrdinate(45, 169)), oceania)},
                { Country.Australia, new CountryInfo(new StateSpace(new CoOrdinate(47, 141), new CoOrdinate(55, 159)), oceania)}
            };

            // Can these be put together any more succinctly?
            var links = new Link[] 
            {
                new Link(countries[Country.Alaska], countries[Country.Kamchatka], LinkType.Indirect),
                new Link(countries[Country.CentralAmerica], countries[Country.Venezuala]),
                new Link(countries[Country.Greenland], countries[Country.NorthWestTerritory], LinkType.North),
                new Link(countries[Country.Greenland], countries[Country.Ontario]),
                new Link(countries[Country.Greenland], countries[Country.Quebec]),
                new Link(countries[Country.Greenland], countries[Country.Iceland]),
                new Link(countries[Country.Brazil], countries[Country.NorthAfrica]),
                new Link(countries[Country.Iceland], countries[Country.Britain]),
                new Link(countries[Country.Iceland], countries[Country.Scandinavia]),
                new Link(countries[Country.Britain], countries[Country.Scandinavia]),
                new Link(countries[Country.Britain], countries[Country.NorthernEurope]),
                new Link(countries[Country.Britain], countries[Country.WesternEurope]),
                new Link(countries[Country.Scandinavia], countries[Country.NorthernEurope]),
                new Link(countries[Country.Ukraine], countries[Country.Ural]),
                new Link(countries[Country.Ukraine], countries[Country.Afghanastan]),
                new Link(countries[Country.Ukraine], countries[Country.MiddleEast]),
                new Link(countries[Country.WesternEurope], countries[Country.NorthAfrica]),
                new Link(countries[Country.SouthernEurope], countries[Country.NorthAfrica]),
                new Link(countries[Country.SouthernEurope], countries[Country.Egypt]),
                new Link(countries[Country.SouthernEurope], countries[Country.MiddleEast]),
                new Link(countries[Country.EastAfrica], countries[Country.Madagascar]),
                new Link(countries[Country.SouthAfrica], countries[Country.Madagascar]),
                new Link(countries[Country.Kamchatka], countries[Country.Japan]),
                new Link(countries[Country.Mongolia], countries[Country.Japan]),
                new Link(countries[Country.Siam], countries[Country.Indonesia]),
                new Link(countries[Country.Indonesia], countries[Country.NewGuinea]),
                new Link(countries[Country.Indonesia], countries[Country.Australia]),
                new Link(countries[Country.NewGuinea], countries[Country.Australia])
            };

            var textBox = new TextBox(new StateSpace(new CoOrdinate(1, 190), new CoOrdinate(55, 240)));
            var userInterface = new ConsoleUserInterface(textBox);
            var game = new Game(2, countries, links, userInterface);

            game.Run();           
        }
    }

    enum Country
    {
        Alaska = 1, NorthWestTerritory, Alberta, Ontario, Quebec, WesternUnitedStates, EasternUnitedStates, CentralAmerica, Greenland,
        Venezuala, Peru, Brazil, Argentina,
        Iceland, Britain, Scandinavia, NorthernEurope, Ukraine, WesternEurope, SouthernEurope,
        NorthAfrica, Egypt, Congo, EastAfrica, SouthAfrica, Madagascar,
        Ural, Algeria, Yakutsk, Irkutsk, Kamchatka, Afghanastan, China, Mongolia, Japan, MiddleEast, India, Siam,
        Indonesia, NewGuinea, Australia
    }

}

// BUGS

// v1.
// DONE: income from continents not working
// DONE: some of the remote neighbours not working(eg: 16 - 17)36
// DONE: cards not rendered nicely
// DONE: players should have a minimum of 3 armies to distribute
// DONE: on elimination:
// DONE:    player remains active
// DONE:    cards are not transferred
// DONE: textbox not clearing top line(only ?)
// DONE: textbox instructions to be paragraphed
// DONE: asking how many armies to transfer after stating in deployment parameters
// DONE: don't tell players they have a full set if they don't view their cards
// DONE: Attack with same parameters no longer recognised, (due attempting to build model with no parameters)
// DONE: When transferring a number higher that held, the appropriate error message is rendered, but eventually the incorrect number of armies are transferred.
// DONE: Sometimes attacking with one more army than asked
// DONE: Exception thrown when unidentified country selected during attack
// DONE: Exception thrown when too many armies are selected for attack
// DONE: After receipt of 3 cards, player asked to trade even after refusing to view cards
// DONE: cards: correct number but incorrect ids shows 'incorrect number of parameters provided'
// DONE: show how army income is calculated at the beginning of each turn.
// DONE: player should not be allowed to quit trading cards if they have 5 cards
// DONE: ensure that all countries start with 1 army
// DONE: cards are not distributed randomly

// v2.
// DONE: Links
// DONE: Refactor RenderLinks method

// v2.1
// Ensure North West Territory - Greenland link is shown
// DONE: Link independently of country formation so links are not duplicated
// Handle how Alaska and Kamchatka are linked (with a bool?)

// Add country transposing functionality
// ~~~ amend to find 'y' and 'n' with regex (to handle approximate responses)
// Ensure that no two players have the same name --or-- introduce unique player Ids
// split ConsoleUserInterface into partial classes
// have the ui ask how many players
