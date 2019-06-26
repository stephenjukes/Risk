using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Risk
{
    public class Game
    {
        private Player[] _players;
        private IUserInterface _ui;       
        private CountryInfo[] _countries;
        private IEnumerable<Link> _links;
        private List<Card> _cards;
        private Random _random = new Random();
        private IEnumerable<Continent> _continents;     
        private Dictionary<int, int> armiesOnSetup = new Dictionary<int, int>() { {2, 40}, {3, 35}, {4, 30}, {5, 25}, {6, 20} };
        private bool gameActive = true;

        public Game(Map map, IUserInterface ui)
        {
            _countries = map.Countries;
            _links = map.Links;
            _ui = ui;
        }

        public void Run()
        {
            Player currentPlayer = null;

            SetUpGame();

            while (gameActive)
            {
                foreach (var player in _players)
                {
                    if (!player.IsActive) continue;
                    currentPlayer = player;

                    Play(player);
                    _ui._textbox.Clear();   // Add to ConsoleUserInterface later.
                }
            }

            _ui.AnnounceWinner(currentPlayer);
        }

        private void SetUpGame()
        {
            PromptForFullScreen();
            SetUpContinents();
            SetUpCards();
            DefineNeighbours();
            SetUpPlayers();
            AllocateCountries();
            DetermineOpeningIncome();
            Render();
            SetUpArmies();
        }

        private void PromptForFullScreen()
        {
            Console.WriteLine("Please set your console to maximum size, and then press any key");
            Console.ReadLine();
        }

        private void DetermineOpeningIncome()
        {
            var armiesBeforeCountryAllocation = armiesOnSetup[_players.Length];
            var countriesByPlayer = _countries.GroupBy(c => c.Occupier);

            foreach (var group in countriesByPlayer)
            {
                var player = group.Key;
                player.OpeningIncome = armiesBeforeCountryAllocation - group.Count();
            }
        }

        private void Play(Player player)
        {         
            _ui.PrepareUiForPlayer(player);
            player.hasEarnedCard = false;

            var armyIncome = DefineArmyIncome(player);
            DistributeArmies(player, armyIncome);

            Attack(player);
            Fortify(player);
            ProvideCard(player);
        }

        private void ProvideCard(Player player)
        {
            if (player.hasEarnedCard)
            {
                var drawnCard = _cards[0];
                player.Cards.Add(drawnCard);

                _cards.RemoveAt(0);               
            }
        }

        private void Fortify(Player player)
        {
            var fortificationParameters = _ui.GetFortificationParameters(player, _countries);

            if (fortificationParameters != null)
            {
                TransferArmies(fortificationParameters);
            }
        }

        private void Attack(Player player)
        {
            Deployment previousAttackParameters = null;

            while (true)
            {
                var attackParameters = _ui.GetAttackParameters(player, _countries, previousAttackParameters);
                if (attackParameters == null) break;

                previousAttackParameters = new Deployment
                {
                    Armies = attackParameters.Armies,
                    From = attackParameters.From,
                    To = attackParameters.To
                };

                EngageBattle(attackParameters);

                if (attackParameters.To.Armies == 0)
                {
                    ManageBattleVictory(attackParameters, previousAttackParameters);                  
                }
            }
        }

        private void ManageBattleVictory(Deployment attackParameters, Deployment previousAttackParameters)
        {
            var invader = attackParameters.From.Occupier;
            var defender = Array.Find(_players, p => p.Id == attackParameters.To.Occupier.Id); // prevents the defender from being changed during Army Transfer  

            _ui.ManageBattleVictory(attackParameters.From.Occupier);

            var deployment = _ui.GetArmyTransfer(attackParameters);
            TransferArmies(deployment);
            previousAttackParameters = null;
            attackParameters.From.Occupier.hasEarnedCard = true;

            var hasEliminatedPlayer = !_countries.Any(c => c.Occupier.Id == defender.Id);
            if (hasEliminatedPlayer)
            {
                ManagePlayerElimination(invader, defender);             
            }
        }

        private void ManagePlayerElimination(Player invader, Player defender)
        {
            defender.IsActive = false;
            invader.Cards.AddRange(defender.Cards);

            _ui.ManagePlayerElimination(invader, defender);
            ManageCards(invader);

            var activePlayers = _players.Where(p => p.IsActive).Count();
            if (activePlayers == 1)
            {
                gameActive = false;
            }
        }

        private void TransferArmies(Deployment deployment)
        {
            _ui.RenderArmyTransfer(deployment);

            deployment.To.Occupier = deployment.From.Occupier;
            deployment.To.Armies = deployment.Armies;
            deployment.From.Armies -= deployment.Armies;
        }

        private void EngageBattle(Deployment attackParameters)
        {
            var armies = attackParameters.Armies;
            var invadingFrom = attackParameters.From;
            var invadingTo = attackParameters.To;
            var fortifyingArmies = invadingFrom.Armies - armies;

            while ((invadingFrom.Armies > fortifyingArmies) && (invadingTo.Armies > 0))
            {
                var numberOfAttackingDice = (armies >= 3 && invadingFrom.Armies >= 3) ? 3 : armies;
                var numberOfDefendingDice = invadingTo.Armies == 1 ? 1 : 2;
                var diceToCompare = (new int[] { numberOfAttackingDice, numberOfDefendingDice }).Min();

                var attackingDice = new int[numberOfAttackingDice];
                var defendingDice = new int[numberOfDefendingDice];

                Roll(attackingDice);
                Roll(defendingDice);

                for (var i = 0; i < diceToCompare; i++)
                {
                    if (attackingDice[i] > defendingDice[i])
                        attackParameters.To.Armies -= 1;
                    else
                    {
                        armies -= 1;
                        attackParameters.From.Armies -= 1;
                    }
                        
                }

                _ui.RenderSkirmish(attackParameters);              
            }
        }

        private void Roll(int[] dice)
        {
            var score = new Random();
            for (var i = 0; i < dice.Length; i++)
            {
                dice[i] = score.Next(0, 7);
            }

            Array.Sort(dice);
            Array.Reverse(dice);
        }

        private void Render() => _ui.Render(_countries, _links);

        private void SetUpCards()
        {
            //var cardTypes = new Type[] { typeof(InfantryCard), typeof(CavalryCard), typeof(ArtilleryCard) };
            var cardTypes = new CardType[] { CardType.Infantry, CardType.Cavalry, CardType.Artillery, CardType.Wild };

            _cards = _countries
                .OrderBy(c => _random.Next())   // shuffles countries
                .Select((country, index) =>
                    {
                        var cardType = cardTypes[index % cardTypes.Length];
                        return new Card(cardType, country.Name);
                    })
                .OrderBy(c => _random.Next())   // shuffles card types
                .ToList();
        }

        private void SetUpContinents()
        {
            _continents = _countries
                .GroupBy(country => country.Continent)
                .Select(group => new Continent
                    {
                        Name = group.Key.Name,
                        Color = group.First().Continent.Color,
                        ArmyProvisionForMonpoly = group.First().Continent.ArmyProvisionForMonpoly,
                        Size = group.Count()
                    });
        }

        private int DefineArmyIncome(Player player)
        {
            var occupied = _countries
                .Where(c => c.Occupier.Id == player.Id)
                .Select(c => c);
            
            var fromCountries = DefineIncomeByCountryOccupation(occupied.Count());
            var fromContinents = DefineIncomeByContinentOccupation(occupied);
            var fromCards = ManageCards(player);

            _ui.DisplayArmyIncome(fromCountries, fromContinents, fromCards);

            return fromCountries + fromContinents + fromCards;

        }

        private int ManageCards(Player player)
        {
            var cards = player.Cards;

            var hasValidSet = DetermineValidSet(cards);
            var tradedCards = _ui.ManageCards(cards, hasValidSet);

            if (!tradedCards.Any()) return 0;

            player.Cards.RemoveAll(c => tradedCards.Contains(c));
            _cards.AddRange(tradedCards);

            var tradedCountries = tradedCards.Select(c => c.CountryName);
            var occupied = _countries.Where(c => tradedCountries.Contains(c.Name) && c.Occupier.Id == player.Id);

            foreach(var country in occupied)
            {
                IncrementArmies(country, 2);
            }

            // TODO: Try to amend so that cards are incremented after return (allowing the initial SetIncome to be set at 4 instead of 2)
            IncrementIncomeByCards(player);

            return player.CardSetIncome;
        }

        private void IncrementIncomeByCards(Player player)
        {
            var income = player.CardSetIncome;

            player.CardSetIncome +=
                income < 12 ? 2
                : income == 12 ? 3
                : 5;
        }

        private bool DetermineValidSet(List<Card> cards)
        {
            var groupByCardType = cards.GroupBy(c => c.CardType);

            if (groupByCardType.Count() == 3) return true;

            foreach(var cardType in groupByCardType)
            {
                if (cardType.Count() >= 3) return true;
            }

            return false;
        }

        private int DefineIncomeByContinentOccupation(IEnumerable<CountryInfo> occupied)
        {
            var occupiedByContinent = occupied.GroupBy(c => c.Continent);
            var income = 0;

            foreach(var occupiedInContinent in occupiedByContinent)
            {
                var continentSize = _continents
                    .Where(c => c.Name == occupiedInContinent.Key.Name)
                    .First().Size;

                if (occupiedInContinent.Count() == continentSize)
                    income += occupiedInContinent.Key.ArmyProvisionForMonpoly;
            }

            return income;
        }

        private int DefineIncomeByCountryOccupation(int occupied)
        {
            var income = occupied / 3;
            return income > 3 ? income : 3;
        }

        private void DefineNeighbours()
        {
            AddRemoteNeighbours();

            foreach (var country in _countries)
            {
                var remoteNeighbours = country.RemoteNeighbours;
                country.Neighbours.AddRange(remoteNeighbours);

                AddNeighbours(country);
            }           
        }

        //private void AddUnattachedNeighbours(CountryInfo country)
        //{
        //    var unattachedNeighbourInfo = _countries.Where(c => country.RemoteNeighbours.Contains(c.Value)).ToList();

        //    if (unattachedNeighbourInfo.Count() != country.RemoteNeighbours.Length)
        //    {
        //        var invalidNeighbours = country.RemoteNeighbours
        //            .Where(name => !unattachedNeighbourInfo.Select(info => info.Value.Name).Contains(name));

        //        throw new Exception($"Unattached country names: {String.Join(", ", invalidNeighbours)} could not be found as a neighbour of {country.Name}. Please check spelling.");
        //    }

        //    country.Neighbours.AddRange(
        //        unattachedNeighbourInfo.Select(n => n.Value));
        //}

        private void AddRemoteNeighbours()
        {
            foreach (var link in _links)
            {
                var country = Array.Find(_countries, c => c.Name == link.Country.Name);  
                var neighbour = Array.Find(_countries, c => c.Name == link.Neighbour.Name);

                country.RemoteNeighbours.Add(neighbour);
                neighbour.RemoteNeighbours.Add(country);
            }
        }

        private void AddNeighbours(CountryInfo country)
        {
            foreach (var potentialNeighbour in _countries)
            {
                var WestCoastIsDistinctToEastCoast = potentialNeighbour.StateSpace.TopLeft.Column > country.StateSpace.BottomRight.Column;
                var EastCoastIsDistinctToWestCoast = potentialNeighbour.StateSpace.BottomRight.Column < country.StateSpace.TopLeft.Column;
                var NorthCoastDistinctToSouthCoast = potentialNeighbour.StateSpace.BottomRight.Row < country.StateSpace.TopLeft.Row;
                var SouthCoastDistinctToNorthCoast = potentialNeighbour.StateSpace.TopLeft.Row > country.StateSpace.BottomRight.Row;
                var NorthWestCornerIsDistinctToSouthEastCorner = potentialNeighbour.StateSpace.TopLeft.Row == country.StateSpace.BottomRight.Row && potentialNeighbour.StateSpace.TopLeft.Column == country.StateSpace.BottomRight.Column;
                var SouthEastCornerIsDistinctToNorthWestCorner = potentialNeighbour.StateSpace.BottomRight.Row == country.StateSpace.TopLeft.Row && potentialNeighbour.StateSpace.BottomRight.Column == country.StateSpace.TopLeft.Column;
                var NorthEastCornerIsDistinctToSouthWestCorner = potentialNeighbour.StateSpace.TopLeft.Row == country.StateSpace.BottomRight.Row && potentialNeighbour.StateSpace.BottomRight.Column == country.StateSpace.TopLeft.Column;
                var SouthWestCornerIsDistinctToNorthEastCorner = potentialNeighbour.StateSpace.BottomRight.Row == country.StateSpace.TopLeft.Row && potentialNeighbour.StateSpace.BottomRight.Column == country.StateSpace.TopLeft.Column;

                var countriesAreIsolated =
                    WestCoastIsDistinctToEastCoast || EastCoastIsDistinctToWestCoast || NorthCoastDistinctToSouthCoast || SouthCoastDistinctToNorthCoast ||
                    NorthWestCornerIsDistinctToSouthEastCorner || SouthEastCornerIsDistinctToNorthWestCorner || NorthEastCornerIsDistinctToSouthWestCorner || SouthWestCornerIsDistinctToNorthEastCorner ||
                    country.Name == potentialNeighbour.Name;

                if (!countriesAreIsolated) country.Neighbours.Add(potentialNeighbour);
            }

            //if (country.Neighbours.Count == 0)
            //    throw new Exception($"{country.Name} has no neighbours. Please check statespace co-ordinates or remove country.");
        }



        private void SetUpPlayers()
        {
            var playerQuantity = _ui.SetUpPlayers();
            _players = new Player[playerQuantity];

            for (var i = 0; i < _players.Length; i++)
            {
                _players[i] = _ui.SetUpPlayer(i + 1);

                _players[i].Cards.AddRange(_cards.Where(card => (int)card.CountryName % (i + 5) == 0)); // delete when not required for testing
            }

            _ui._textbox.Clear();   // move to ui later
        }

        private void AllocateCountries()
        {
            var shuffledCountries = _countries.OrderBy(c =>_random.Next()).ToArray();
            var i = 1;

            foreach(var country in shuffledCountries)
            {
                var playerNumber = i % _players.Length;
                country.Occupier = _players[playerNumber];
                i++;
            }
        }

        // Consider whether to retain original OpeningIncomes in players instead of mutating.
        private void SetUpArmies()
        {
            bool armiesStillToDistribute = true;

            while (armiesStillToDistribute)
            {
                var distributionPerTurn = 10;               
                foreach (var player in _players)
                {
                    var distributionForThisTurn = player.OpeningIncome < distributionPerTurn ? player.OpeningIncome : distributionPerTurn;

                    DistributeArmies(player, distributionForThisTurn);
                    player.OpeningIncome -= distributionForThisTurn;

                    // TODO: Try to add this to ConsoleUserInterface later.
                    _ui._textbox.Clear();
                }

                armiesStillToDistribute = _players.Any(p => p.OpeningIncome > 0);
            }
        }

        private void DistributeArmies(Player player, int armies)
        {
            var armiesToDistribute = _ui.DistributeArmies(player, _countries, armies);

            foreach (var distribution in armiesToDistribute)
            {
                var targetCountry = Array.Find(_countries, c => c.Name == distribution.To.Name);
                targetCountry.Armies += distribution.Armies;
            }
        }

        private void IncrementArmies(CountryInfo country, int armies)
        {
            var distribution = new Deployment
            {
                Armies = armies,
                To = country
            };

            _ui.IncrementArmies(distribution);

            country.Armies += 2;
        }
    }
}
