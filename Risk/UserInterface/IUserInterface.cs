using System;
using System.Collections.Generic;
using System.Text;

namespace Risk
{
    interface IUserInterface
    {
        // Remove when finished
        TextBox _textbox { get; }

        Player SetUpPlayer(int playerNumber);
        void Render(CountryInfo[] countries, IEnumerable<Link> links);
        List<Deployment> DistributeArmies(Player player, CountryInfo[] countries, int armies);
        void IncrementArmies(Deployment armyDistribution);
        List<Card> ManageCards(List<Card> cards, bool isValidSet);
        void PrepareUiForPlayer(Player player);
        Deployment GetAttackParameters(Player player, CountryInfo[] countries, Deployment previousAttackParameters);
        void RenderSkirmish(Deployment attackParameters);
        Deployment GetArmyTransfer(Deployment deployment);
        void RenderArmyTransfer(Deployment attackParameters);
        Deployment GetFortificationParameters(Player player, CountryInfo[] countries);
        void ManageBattleVictory(Player occupier);
        void AnnounceWinner(Player currentPlayer);
        void DisplayArmyIncome(int fromCountries, int fromContinents, int fromCards);
    }
}
