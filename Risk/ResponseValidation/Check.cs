using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Risk
{
    static public class Check
    {
        public static string ValidCountryIds(ValidationParameter<Deployment> p)
        {
            var invalidIds = (new CountryInfo[] { p.Object.From, p.Object.To }).Any(c => c == null);
            var error = $"Country Id not recognised.";

            return invalidIds ? error : null;
        }

        public static string SufficientArmies(ValidationParameter<Deployment> p)
        {
            var isDistribution = p.Object.From.Name == 0;
            var areSufficientArmies = isDistribution ? p.Object.Armies <= p.Object.From.Armies : p.Object.Armies < p.Object.From.Armies;
            var error = "Insufficient armies for intended deployment.";

            return !areSufficientArmies ? error : null;
        }

        public static string PlayerHoldsAttackingCountry(ValidationParameter<Deployment> p)
        {
            var isValidCountry = p.Object.From.Occupier.Name == p.Player.Name;
            var error = "You cannot attack from a country you do not already hold.";

            return !isValidCountry ? error : null;
        }

        public static string PlayerHoldsDeploymentCountry(ValidationParameter<Deployment> p)
        {
            var isValidCountry = p.Object.To.Occupier.Name == p.Player.Name;
            var error = "You can only place armies in a country that you already hold.";

            return !isValidCountry ? error : null;
        }

        public static string PlayerDoesNotHoldDeploymentCountry(ValidationParameter<Deployment> p)
        {
            var isValidCountry = p.Object.To.Occupier.Name != p.Player.Name;
            var error = "You cannot attack a country that you already hold.";

            return !isValidCountry ? error : null;
        }

        public static string DeploymentToNeighbouringCountry(ValidationParameter<Deployment> p)
        {
            var neighbours = p.Object.From.Neighbours.Select(n => n.Name);
            var isNeighbour = neighbours.Contains(p.Object.To.Name);
            var error = "You can only attack neighbouring countries.";

            return !isNeighbour ? error : null;
        }

        public static string PreviousAttackParametersExist(ValidationParameter<Deployment> p)
        {
            var error = "No previous attack parameters";
            return p.PreviousDeployment == null ? error : null;
        }

        //public static string QuitOnlyOnLessThanFive(ValidationParameter<List<Card>> p)
        //{
        //    var error = "Cannot quit - you must trade when holding at least 5 cards";
        //    var cardQuantity = p.Cards.Count;
        //    return p.Cards.Count >= 5 ? 
        //}

        public static string ThreeSelectedFromOwnCards(ValidationParameter<List<Card>> p)
        {
            var error = "Invalid selection";
            return p.Object.Count() != 3 ? error : null;
        }

        public static string ValidSet(ValidationParameter<List<Card>> p)
        {
            var error = "Invalid set";
            var distinctCardTypes = p.Object.Select(c => c.CardType).Distinct();
            var containsWildCard = distinctCardTypes.Contains(CardType.Wild);
            var isValidSet = containsWildCard || distinctCardTypes.Count() == 1 || distinctCardTypes.Count() == 3;

            return !isValidSet ? error : null;
        }

        public static string ValidNumberOfPlayers(ValidationParameter<int> p)
        {
            var error = "Players must be between 2 and 6";
            return p.Object < 2 || p.Object > 6 ? error : null;
        }
    }
}


