﻿using Risk.ResponseValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Risk.UserInterface.ConsoleUserInterface
{
    public partial class ConsoleUserInterface
    {
        public List<Card> ManageCards(List<Card> cards, bool hasValidSet)
        {
            _textbox.Write($"You have {cards.Count} cards.");
            _textbox.Write();

            if (cards.Count >= 5)
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

            var userInteraction = new UserInteractionBuilder<bool>()
                .Request(
                    "View cards:",
                    "Press 'y' to view or 'n' to skip")
                .ResponseInterpretations(
                    new ResponseInterpretation<bool>("y", Accept),
                    new ResponseInterpretation<bool>("n", Decline),
                    new ResponseInterpretation<bool>("default", ResponseNotRecognised))
                .Cards(cards)
                .Build();

            return HandleResponse(userInteraction);
        }

        private bool GetDecisionToTrade()
        {
            var userInteraction = new UserInteractionBuilder<bool>()
                .Request(
                    "Trade:",
                    "Press 'y' to trade or 'n' to skip")
                .ResponseInterpretations(
                    new ResponseInterpretation<bool>("y", Accept),
                    new ResponseInterpretation<bool>("n", Decline),
                    new ResponseInterpretation<bool>("default", ResponseNotRecognised))
                .Build();

            return HandleResponse(userInteraction);
        }

        private List<Card> TradeCards(List<Card> cards)
        {
            var userInteraction = new UserInteractionBuilder<List<Card>>()
                .Request("Select card IDs for trade, separated by a comma")
                .ResponseInterpretations(
                    new ResponseInterpretation<List<Card>>("q", vp => cards.Count >= 5 ? ProhibitQuit(vp) : Quit(vp)),
                    new ResponseInterpretation<List<Card>>("default", vp => ValidateCardTrade(vp)))
                .Cards(cards)
                .Build();

            return HandleResponse(userInteraction);
        }

        private ValidationResult<List<Card>> ValidateCardTrade(ValidationParameter<List<Card>> validationParameter)
        {
            var responseValidation = new ResponseValidationBuilder<List<Card>, int>()
                .ValidationParameter(validationParameter)
                .MatchBuilder(GetIntegerMatches)
                .TestObjectBuilder((matches, vp)
                    => vp.Cards.Where(c => matches.Contains((int)c.CountryName)).ToList())
                .ErrorChecks(
                    Check.ThreeSelectedFromOwnCards,
                    Check.ValidSet)
                .Build();

            return responseValidation.Validate();
        }

        private void ViewCards(List<Card> cards)
        {
            _textbox.Write("ID\tCARDTYPE\tCOUNTRY");
            _textbox.Write("--------------------------------");

            foreach (var card in cards)
            {
                _textbox.Write(
                    $"{((int)card.CountryName).ToString().PadLeft(2, '0')}\t" +
                    $"{card.CardType.ToString().PadRight(9, ' ')}\t" +
                    $"{card.CountryName}");
            }
            _textbox.Write();
        }
    }
}
