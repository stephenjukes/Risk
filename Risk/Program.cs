using System;
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

            var countries = new CountryInfo[]
            {
                new CountryInfo(1, "Alaska", new StateSpace(new CoOrdinate(0, 3), new CoOrdinate(9, 17)), northAmerica, "Kamchatka"),
                new CountryInfo(2, "NorthWest Territory", new StateSpace(new CoOrdinate(0, 17), new CoOrdinate(6, 32)), northAmerica, "Greenland"),
                new CountryInfo(3, "Alberta", new StateSpace(new CoOrdinate(6, 17), new CoOrdinate(12, 32)), northAmerica),
                new CountryInfo(4, "Ontario", new StateSpace(new CoOrdinate(3, 32), new CoOrdinate(12, 46)), northAmerica, "Greenland"),
                new CountryInfo(5, "Quebec", new StateSpace(new CoOrdinate(6, 46), new CoOrdinate(12, 60)), northAmerica, "Greenland"),
                new CountryInfo(6, "Western United States", new StateSpace(new CoOrdinate(12, 18), new CoOrdinate(19, 38)), northAmerica),
                new CountryInfo(7, "Eastern United States", new StateSpace(new CoOrdinate(12, 38), new CoOrdinate(19, 52)), northAmerica),
                new CountryInfo(8, "Central America", new StateSpace(new CoOrdinate(19, 27), new CoOrdinate(26, 42)), northAmerica, "Venezuala"),
                new CountryInfo(9, "Greenland", new StateSpace(new CoOrdinate(0, 62), new CoOrdinate(7, 77)), northAmerica, "NorthWest Territory", "Ontario", "Quebec", "Iceland"),
                new CountryInfo(10 , "Venezuala", new StateSpace(new CoOrdinate(28, 24), new CoOrdinate(34, 45)), southAmerica, "Central America"),
                new CountryInfo(11, "Peru", new StateSpace(new CoOrdinate(34, 20), new CoOrdinate(44, 35)), southAmerica),
                new CountryInfo(12, "Brazil", new StateSpace(new CoOrdinate(34, 35), new CoOrdinate(44, 49)), southAmerica, "North Africa"),
                new CountryInfo(13, "Argentina", new StateSpace(new CoOrdinate(44, 28), new CoOrdinate(55, 44)), southAmerica),
                new CountryInfo(14, "Iceland", new StateSpace(new CoOrdinate(9, 64), new CoOrdinate(12, 78)), europe, "Greenland", "Britain", "Scandinavia"),
                new CountryInfo(15, "Britain", new StateSpace(new CoOrdinate(14, 64), new CoOrdinate(17, 78)), europe, "Iceland", "Scandinavia", "Northern Europe", "Western Europe"),
                new CountryInfo(16, "Scandinavia", new StateSpace(new CoOrdinate(11, 81), new CoOrdinate(14, 100)), europe, "Iceland", "Britain", "Northern Europe"),
                new CountryInfo(17, "Northern Europe", new StateSpace(new CoOrdinate(16, 81), new CoOrdinate(20, 100)), europe, "Britain", "Scandinavia"),
                new CountryInfo(18, "Ukraine", new StateSpace(new CoOrdinate(11, 100), new CoOrdinate(23, 115)), europe, "Ural", "Afghanastan", "Middle East"),
                new CountryInfo(19, "Western Europe", new StateSpace(new CoOrdinate(20, 68), new CoOrdinate(26, 83)), europe, "Britain", "North Africa"),
                new CountryInfo(20, "Southern Europe", new StateSpace(new CoOrdinate(20, 83), new CoOrdinate(26, 100)), europe, "North Africa", "Egypt", "Middle East"),
                new CountryInfo(21, "North Africa", new StateSpace(new CoOrdinate(31, 66), new CoOrdinate(38, 86)), africa, "Western Europe", "Southern Europe", "Brazil"),
                new CountryInfo(22, "Egypt", new StateSpace(new CoOrdinate(28, 86), new CoOrdinate(34, 105)), africa, "Southern Europe", "Middle East"),
                new CountryInfo(23, "Congo", new StateSpace(new CoOrdinate(38, 72), new CoOrdinate(44, 86)), africa),
                new CountryInfo(24, "East Africa", new StateSpace(new CoOrdinate(34, 86), new CoOrdinate(44, 105)), africa, "Madagascar"),
                new CountryInfo(25, "South Africa", new StateSpace(new CoOrdinate(44, 79), new CoOrdinate(54, 93)), africa, "Madagascar"),
                new CountryInfo(26, "Madagascar", new StateSpace(new CoOrdinate(47, 96), new CoOrdinate(52, 113)), africa, "East Africa", "South Africa"),
                new CountryInfo(27, "Ural", new StateSpace(new CoOrdinate(0, 117), new CoOrdinate(16, 138)), asia, "Ukraine"),
                new CountryInfo(28, "Algeria", new StateSpace(new CoOrdinate(0, 138), new CoOrdinate(16, 152)), asia),
                new CountryInfo(29, "Yakutsk", new StateSpace(new CoOrdinate(0, 152), new CoOrdinate(6, 167)), asia),
                new CountryInfo(30, "Irkutsk", new StateSpace(new CoOrdinate(6, 152), new CoOrdinate(12, 167)), asia),
                new CountryInfo(31, "Kamchatka", new StateSpace(new CoOrdinate(0, 167), new CoOrdinate(15, 183)), asia, "Alaska", "Japan"),
                new CountryInfo(32, "Afghanastan", new StateSpace(new CoOrdinate(16, 117), new CoOrdinate(25, 135)), asia, "Ukraine"),
                new CountryInfo(33, "China", new StateSpace(new CoOrdinate(16, 135), new CoOrdinate(25, 152)), asia),
                new CountryInfo(34, "Mongolia", new StateSpace(new CoOrdinate(12, 152), new CoOrdinate(21, 167)), asia, "Japan"),
                new CountryInfo(35, "Japan", new StateSpace(new CoOrdinate(18, 170), new CoOrdinate(23, 184)), asia, "Mongolia", "Kamchatka"),
                new CountryInfo(36, "Middle East", new StateSpace(new CoOrdinate(25, 107), new CoOrdinate(34, 120)), asia, "Ukraine"),
                new CountryInfo(37, "India", new StateSpace(new CoOrdinate(25, 120), new CoOrdinate(34, 138)), asia),
                new CountryInfo(38, "Siam", new StateSpace(new CoOrdinate(25, 138), new CoOrdinate(34, 150)), asia, "Indonesia"),
                new CountryInfo(39, "Indonesia", new StateSpace(new CoOrdinate(37, 136), new CoOrdinate(44, 152)), oceania, "Siam", "Australia", "New Guinea"),
                new CountryInfo(40, "New Guinea", new StateSpace(new CoOrdinate(38, 155), new CoOrdinate(45, 169)), oceania, "Indonesia", "Australia"),
                new CountryInfo(41, "Australia", new StateSpace(new CoOrdinate(47, 141), new CoOrdinate(55, 159)), oceania, "Indonesia", "New Guinea")
            };

            var textBox = new TextBox(new StateSpace(new CoOrdinate(1, 190), new CoOrdinate(55, 240)));
            var userInterface = new ConsoleUserInterface(textBox);
            var game = new Game(2, countries, userInterface);

            game.Run();

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
            // Links
                // Ensure North West Territory - Greenland link is shown
                // Link independently of country formation so links are not duplicated
                // Handle how Alaska and Kamchatka are linked (with a bool?)
                // Refactor RenderLinks method
            // Add country transposing functionality
            // ~~~ amend to find 'y' and 'n' with regex (to handle approximate responses)
            // Ensure that no two players have the same name --or-- introduce unique player Ids
            // split ConsoleUserInterface into partial classes
            // have the ui ask how many players
            
        }
    }
}
