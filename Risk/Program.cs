using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Risk
{
    class Program
    {
        public static void Main(string[] args)
        {
            var map = new StandardMap();
            var textBox = new TextBox(new StateSpace(new CoOrdinate(1, 190), new CoOrdinate(55, 240)));
            var userInterface = new ConsoleUserInterface(textBox);

            var game = new Game(2, map, userInterface);
            game.Run();           
        }
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
// DONE: Ensure North West Territory - Greenland link is shown
// DONE: Link independently of country formation so links are not duplicated
// DONE: Handle how Alaska and Kamchatka are linked (with a bool?)

// v2.2
// DONE: Use opposite link if indirect
// DONE: Complete Link parameters only after link selected
// DONE: Store country, continent and link information in a Map class.


// Add country transposing functionality
// ~~~ amend to find 'y' and 'n' with regex (to handle approximate responses)
// Ensure that no two players have the same name --or-- introduce unique player Ids
// split ConsoleUserInterface into partial classes
// have the ui ask how many players
