using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Risk
{
    class ConsoleUserInterface : IUserInterface
    {
        public TextBox _textbox { get; }
        private ConsoleColor[] _consoleColors = new ConsoleColor[]
        {
            ConsoleColor.Blue,
            ConsoleColor.Green,
            ConsoleColor.Cyan,
            ConsoleColor.Red,
            ConsoleColor.Magenta,
            ConsoleColor.Yellow
        };

        public ConsoleUserInterface(TextBox textBox)
        {
            _textbox = textBox;
        }

        public Player SetUpPlayer(int playerNumber)
        {
            Console.WriteLine($"Player {playerNumber}, please enter your name:");

            var name = Console.ReadLine();
            var color = _consoleColors[playerNumber - 1].ToString();

            return new Player(name, color);
        }

        public void Render(Dictionary<Country, CountryInfo> countries, IEnumerable<Link> links)
        {
            //if (_gameQuality == GameQuality.Optimised)
            //    RenderOptimised();
            //else
            //    RenderDegraded();
            RenderOptimised(countries, links);
        }

        public void RenderOptimised(Dictionary<Country, CountryInfo> countries, IEnumerable<Link> links)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetWindowPosition(0, 0);
            Console.Clear();

            RenderLinks(links);
            RenderCountries(countries);
        }

        private void RenderCountries(Dictionary<Country, CountryInfo> countries)
        {
            foreach (var country in countries)
            {
                RenderCountry(country.Value);
            }
        }

        private void RenderLinks(IEnumerable<Link> links)
        {
            foreach (var link in links)
            {
                var link1 = link;
                var link2 = new Link(link.Neighbour, link.Country, link.LinkTypes);

                RenderLinkToRemoteNeighbour(link);
                RenderLinkToRemoteNeighbour(link2); // necessary for indirect links
            }
        }

        private void RenderLinkToRemoteNeighbour(Link link)
        {
            var links = new Link[]
            {   new NorthLink(link.Country, link.Neighbour, link.LinkTypes),
                new SouthLink(link.Country, link.Neighbour, link.LinkTypes),
                new EastLink(link.Country, link.Neighbour, link.LinkTypes),
                new WestLink(link.Country, link.Neighbour, link.LinkTypes)
            };

            var correctLink = links.Where(l => l.IsThisDirection()).First();

            if (correctLink.Orientation == LinkType.Horizontal)
                RenderHorizontalLink(correctLink);
            else
                RenderVerticalLink(correctLink);
        }

        private void RenderHorizontalLink(Link link)
        {
            var step = link.Displacement / Math.Abs(link.Displacement);
            for (var i = 0; i != link.Displacement; i += step)
            {
                Console.SetCursorPosition(link.Node.Column + i, link.Node.Row);
                Console.Write('_');
            }
        }

        private void RenderVerticalLink(Link link)
        {
            var step = link.Displacement / Math.Abs(link.Displacement);
            for (var i = 0; i != link.Displacement; i += step)
            {
                Console.SetCursorPosition(link.Node.Column, link.Node.Row + i);
                Console.Write('|');
            }
        }

        private void RenderCountry(CountryInfo country)
        {
            var start = country.StateSpace.TopLeft;
            var end = country.StateSpace.BottomRight;
            var width = end.Column - start.Column + 1;
            var height = end.Row - start.Row;
            var marker = '\u2588';

            var horizontalBorder = String.Join("", new char[width].Select(ch => marker));
            var internalRow = String.Join("", new char[width].Select((ch, i) => i == 0 || i == width - 1 ? marker : ' '));

            //Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), country.Continent.Color);
            Console.ForegroundColor = ConsoleColor.Gray;

            // Horizontal borders
            Console.SetCursorPosition(start.Column, start.Row);
            Console.Write(horizontalBorder);

            Console.SetCursorPosition(start.Column, end.Row);
            Console.Write(horizontalBorder);

            // Vertical borders
            for (var i = 1; i < height; i++)
            {
                Console.SetCursorPosition(start.Column, start.Row + i);
                Console.Write(internalRow);
            }

            // Country Information
            RenderCountryInformation(country, country.Occupier.Color);           
        }

        private void RenderCountryInformation(CountryInfo country, string color)
        {
            var start = country.StateSpace.TopLeft;
            var end = country.StateSpace.BottomRight;
            var width = end.Column - start.Column + 1;
            var height = end.Row - start.Row;

            var infoComponents = Regex.Split(country.Name.ToString(), @"(?<!^)(?=[A-Z])").ToList(); // Regex splits by capital letter
            infoComponents[0] = $"{(int)country.Name}. {infoComponents[0]}";
            infoComponents.Add($"{'\u2694'} {country.Armies.ToString().PadLeft(2, '0')}");

            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color);
            var informationStart = new CoOrdinate
            (
                start.Row + ((height + 1) / 2) - (infoComponents.Count / 2),
                start.Column + ((width - infoComponents.Max(e => e.Length)) / 2)
            );

            foreach (var component in infoComponents)
            {
                Console.SetCursorPosition(informationStart.Column, informationStart.Row + infoComponents.FindIndex(e => e == component));
                Console.Write(component);
            }

            // Consider handling this outside of the Render() method
            country.StateSpace.ArmyPosition = new CoOrdinate(Console.CursorTop, informationStart.Column + 2);
        }

        public List<Deployment> DistributeArmies(Player player, Dictionary<Country, CountryInfo> countries, int armies)
        {
            var remainingArmies = armies;
            var armyDistributions = new List<Deployment>();
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), player.Color);

            while (remainingArmies > 0)
            {
                _textbox.Write($"Distribute ({remainingArmies} armies):");
                var response = _textbox.Read();
                _textbox.Write();

                var distribution = ValidateDistribution(response, remainingArmies, player, countries);

                if (distribution.IsValid)
                {
                    armyDistributions.Add(distribution.Object);
                    IncrementArmies(distribution.Object);
                    remainingArmies -= distribution.Object.Armies;
                    continue;
                }

                HandleError(distribution.Error);
            }

            return armyDistributions;
        }

        private ValidationResult<Deployment> ValidateDistribution(string response, int remainingArmies, Player player, Dictionary<Country, CountryInfo> countries)
        {
            var responseValidation = new ResponseValidationBuilder<Deployment, int>()
                    .Parameter(response)
                    .Parameter(remainingArmies)
                    .Parameter(player)
                    .Parameter(countries)
                    .MatchBuilder(GetIntegerMatches)
                    .TestObjectBuilder(CreateDistribution)  // TODO: Can this be handled better with a closure?
                    .ErrorChecks(
                        Check.ValidCountryIds,
                        Check.PlayerHoldsDeploymentCountry,
                        Check.SufficientArmies)
                    .Build();

            return responseValidation.CheckErrors();
        }

        public void IncrementArmies(Deployment distribution)
        {
            Console.CursorVisible = false;

            // consider using async
            for (var i = 0; i <= Math.Abs(distribution.Armies); i++)
            {
                var armyPosition = distribution.To.StateSpace.ArmyPosition;
                var sign = distribution.Armies / Math.Abs(distribution.Armies);

                Console.SetCursorPosition(armyPosition.Column, armyPosition.Row);
                Console.Write((distribution.To.Armies + sign * i).ToString().PadLeft(2, '0'));

                Thread.Sleep(20);
            }

            Console.CursorVisible = true;                   
        }

        public List<Card> ManageCards(List<Card> cards, bool hasValidSet)
        {
            _textbox.Write($"You have {cards.Count} cards.");
            _textbox.Write();

            if (cards.Count == 5)
            {
                _textbox.Write("You must trade 3 of them in. Press [Enter] to view.");
                _textbox.Read();
                _textbox.Write();

                ViewCards(cards);
                return TradeCards(cards);
            }

            var playerWantsToView = GetDecisionToView(cards);
            if (playerWantsToView) ViewCards(cards);

            if (playerWantsToView && hasValidSet)
            {
                var playerWantsToTrade = GetDecisionToTrade();
                return playerWantsToTrade ? TradeCards(cards) : new List<Card>();
            }

            return new List<Card>();
        }

        private bool GetDecisionToView(List<Card> cards)
        {
            if (!cards.Any()) return false;

            _textbox.Write($"View cards\n(Press 'y' to view or 'n' to skip)");
            var selection = _textbox.Read().ToLower();

            _textbox.Write();


            switch (selection)
            {
                case "y":
                    return true;
                case "n":
                    return false;
                default:
                    _textbox.Write("Response not recognised. Please try again.");
                    _textbox.Write();
                    return GetDecisionToView(cards);
            }         
        }

        private void ViewCards(List<Card> cards)
        {
            _textbox.Write("ID\tCARDTYPE\tCOUNTRY");
            _textbox.Write("--------------------------------");

            foreach (var card in cards)
            {
                _textbox.Write($"{card.Id.ToString().PadLeft(2, '0')}\t{card.CardType.ToString().PadRight(9, ' ')}\t{card.CountryName}");
            }
            _textbox.Write();
        }

        private bool GetDecisionToTrade()
        {
            _textbox.Write("Trade\n(Press 'y' to trade or 'n' to skip)");
            var selection = _textbox.Read().ToLower();

            _textbox.Write();

            switch (selection)
            {
                case "y":
                    return true;
                case "n":
                    return false;
                default:
                    _textbox.Write("Response not recognised. Please try again.");
                    _textbox.Write();
                    return GetDecisionToTrade();
            }
        }

        private List<Card> TradeCards(List<Card> cards)
        {
            while (true)
            {
                _textbox.Write("Select card IDs for trade, separated by a comma");
                if (cards.Count < 5) _textbox.Write("(or 'q' to skip)");

                var response = _textbox.Read().ToLower();
                _textbox.Write();

                if (response == "q")
                {
                    if (cards.Count >= 5)
                    {
                        _textbox.Write("Cannot quit - you must trade when holding at least 5 cards");
                        _textbox.Write();
                        continue;
                    }
                    else
                    {
                        return new List<Card>();
                    }   
                }

                var selection = ValidateCardTrade(cards, response);

                if (selection.IsValid)
                    return selection.Object;

                HandleError(selection.Error);
            }
        }

        private ValidationResult<List<Card>> ValidateCardTrade(List<Card> cards, string response)
        {
            var responseValidation = new ResponseValidationBuilder<List<Card>, int>()
                .Parameter(response)
                .Parameter(cards)
                .MatchBuilder(GetIntegerMatches)
                .TestObjectBuilder((matches, validationParameter)
                    => cards.Where(c => matches.Contains(c.Id)).ToList())
                .ErrorChecks(
                    Check.ThreeSelectedFromOwnCards,
                    Check.ValidSet)
                .Build();

            return responseValidation.CheckErrors();
        }

        public void PrepareUiForPlayer(Player player)
        {
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), player.Color);
        }

        public Deployment GetAttackParameters(Player player, Dictionary<Country, CountryInfo> countries, Deployment previousAttackParameters)
        {
            ValidationResult<Deployment> nextAttackParameters = null;

            while (true)
            {
                bool previousAttackParametersAreValid =
                    previousAttackParameters == null ? false :
                    previousAttackParameters.From.Armies < previousAttackParameters.Armies ? false :
                    true;

                _textbox.Write("Attack:");         
                if (previousAttackParametersAreValid) _textbox.Write("- [Enter]: continue with same attack parameters");
                _textbox.Write("- [<x> armies from <country_A> to <country_B>]");
                _textbox.Write("- [q]: to skip");

                var response = _textbox.Read().ToLower();
                _textbox.Write();

                switch (response)
                {
                    case "q":
                        return null;
                    case "":
                        nextAttackParameters = ValidatePreviousAttackParameters(previousAttackParameters, response);
                        break;
                    default:
                        nextAttackParameters = ValidateNewAttackParameters(player, countries, response);
                        break;
                }

                if (nextAttackParameters.IsValid)
                    return nextAttackParameters.Object;

                HandleError(nextAttackParameters.Error);     
            };        
        }

        private void HandleError(string error)
        {
            _textbox.Write(error);
            _textbox.Write();

            //foreach (var error in nextAttackParameters.Errors)
            //{
            //    _textbox.Write(error);
            //}

            //_textbox.Write("Please try again.");
            //_textbox.Write();
        }

        // TODO: Should this be combined with GetAttackParameters?
        public Deployment GetFortificationParameters(Player player, Dictionary<Country, CountryInfo> countries)
        {
            ValidationResult<Deployment> fortifcation;
            while (true)
            {
                _textbox.Write("Fortify:");
                _textbox.Write("- [<x> armies from <country_A> to <country_B>]");
                _textbox.Write("- [q]: to skip");

                var response = _textbox.Read().ToLower();
                _textbox.Write();

                switch (response)
                {
                    case "q":
                        return null;
                    default:
                        fortifcation = ValidateFortificationParameters(player, countries, response);
                        break;
                }

                if (fortifcation.IsValid)
                    return fortifcation.Object;

                HandleError(fortifcation.Error);
            }         
        }

        private ValidationResult<Deployment> ValidateFortificationParameters(Player player, Dictionary<Country, CountryInfo> countries, string response)
        {
            var responseValidation = new ResponseValidationBuilder<Deployment, int>()
                .Parameter(response)
                .Parameter(player)
                .Parameter(countries)
                .MatchBuilder(GetIntegerMatches)
                .TestObjectBuilder(CreateDeployment)
                .ErrorChecks(
                    Check.ValidCountryIds,
                    Check.PlayerHoldsAttackingCountry,
                    Check.PlayerHoldsDeploymentCountry,
                    Check.DeploymentToNeighbouringCountry,
                    Check.SufficientArmies)
                .Build();

            return responseValidation.CheckErrors();
        }

        public Deployment GetArmyTransfer(Deployment deployment)
        {
            while (true)
            {
                _textbox.Write($"How many armies do you wish to transfer?");
                var response = _textbox.Read();
                _textbox.Write();

                var armyTransfer = ValidateArmyTransfer(deployment, response);

                if (armyTransfer.IsValid)
                    return armyTransfer.Object;

                HandleError(armyTransfer.Error);
            }
        }

        private ValidationResult<Deployment> ValidateArmyTransfer(Deployment deployment, string response)
        {
            var responseValidation = new ResponseValidationBuilder<Deployment, int>()
                .Parameter(response)
                .Parameter(deployment)
                .MatchBuilder(GetIntegerMatches)
                .TestObjectBuilder((matches, validationParameter) => new Deployment
                    {
                        Armies = matches[0],
                        From = deployment.From,
                        To = deployment.To
                    })
                .ErrorChecks(
                    Check.SufficientArmies)
                .Build();

            return responseValidation.CheckErrors();
        }

        public void RenderArmyTransfer(Deployment armyTransfer)
        {
            var invadingDistribution = new Deployment
            {
                Armies = -armyTransfer.Armies,
                To = armyTransfer.From
            };

            var defendingDistribution = new Deployment
            {
                Armies = armyTransfer.Armies,
                To = armyTransfer.To
            };

            // flashing country info
            for (var i = 0; i < 5; i++)
            {
                var playerColor = i % 2 == 0 ? armyTransfer.From.Occupier.Color : armyTransfer.To.Occupier.Color;
                RenderCountryInformation(armyTransfer.To, playerColor);
                Thread.Sleep(200);
            }

            // army incrementation
            IncrementArmies(invadingDistribution);
            IncrementArmies(defendingDistribution);
        }

        private ValidationResult<Deployment> ValidatePreviousAttackParameters(Deployment attackParameters, string response)
        {
            var responseValidation = new ResponseValidationBuilder<Deployment, int>()
                .Parameter(response)
                .Parameter(attackParameters)
                .TestObjectBuilder((matches, validationParameter) => attackParameters)
                .ErrorChecks(
                    Check.PreviousAttackParametersExist,
                    Check.SufficientArmies)
                .Build();

            _textbox.Write($"Repeating attack with {attackParameters.Armies} armies from {attackParameters.From.Name} to {attackParameters.To.Name}");
            _textbox.Write();

            return responseValidation.CheckErrors();
        }

        public Deployment Skirmish(Deployment attackParameters)
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
                        invadingTo.Armies -= 1;
                    else
                        invadingFrom.Armies -= 1;
                }

                foreach (var country in new CountryInfo[] { invadingTo, invadingFrom })
                {
                    Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), country.Occupier.Color);

                    var armyPosition = country.StateSpace.ArmyPosition;
                    Console.SetCursorPosition(armyPosition.Column, armyPosition.Row);

                    Console.WriteLine(country.Armies.ToString().PadLeft(2, '0'));
                }

                Thread.Sleep(100);
            }

            return new Deployment
            {
                Armies = armies,
                From = invadingFrom,
                To = invadingTo
            };
        }

        public void RenderSkirmish(Deployment attackParameters)
        {
            foreach (var country in new CountryInfo[] { attackParameters.To, attackParameters.From })
            {
                Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), country.Occupier.Color);

                var armyPosition = country.StateSpace.ArmyPosition;
                Console.SetCursorPosition(armyPosition.Column, armyPosition.Row);

                Console.WriteLine(country.Armies.ToString().PadLeft(2, '0'));
            }

            Thread.Sleep(100);
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

        private ValidationResult<Deployment> ValidateNewAttackParameters(Player player, Dictionary<Country, CountryInfo> countries, string response)
        {
            var responseValidation = new ResponseValidationBuilder<Deployment, int>()
                .Parameter(response)
                .Parameter(player)
                .Parameter(countries)
                .MatchBuilder(GetIntegerMatches)
                .TestObjectBuilder(CreateDeployment)
                .ErrorChecks(
                    Check.ValidCountryIds,
                    Check.PlayerHoldsAttackingCountry,
                    Check.PlayerDoesNotHoldDeploymentCountry,
                    Check.DeploymentToNeighbouringCountry,
                    Check.SufficientArmies)
                .Build();

            return responseValidation.CheckErrors();
        }

        private Deployment CreateDeployment(int[] matches, ValidationParameter<Deployment> validationParameter)
        {
            return new Deployment
            {
                Armies = matches[0],
                From = validationParameter.Countries[(Country)matches[1]],
                To = validationParameter.Countries[(Country)matches[2]]
            };
        }

        private Deployment CreateDistribution(int[] matches, ValidationParameter<Deployment> validationParameter)
        {
            return new Deployment
            {
                Armies = matches[0],
                From = new CountryInfo
                {
                    Armies = validationParameter.ArmiesToDistribute
                },
                To = validationParameter.Countries[(Country)matches[1]]
            };
        }

        private int[] GetIntegerMatches(string response)
        {
            return Regex.Matches(response, @"\d+")
                .Select(m => int.Parse(m.ToString()))
                .ToArray();
        }

        public void ManageBattleVictory(Player occupier)
        {
            _textbox.Write($"Victory to {occupier.Name} !!!".ToUpper());
            _textbox.Write();
        }

        public void AnnounceWinner(Player currentPlayer)
        {
            _textbox.Clear();
            _textbox.Write($"{currentPlayer} wins!!!".ToUpper());
            _textbox.Write();
        }

        public void DisplayArmyIncome(int fromCountries, int fromContinents, int fromCards)
        {
            _textbox.Write("Army Income:");
            _textbox.Write($"{fromCountries} from country occupation");
            _textbox.Write($"{fromContinents} from continent occupation");
            _textbox.Write($"{fromCards} from cards");
            _textbox.Write();
        }
    }
}

