using OZeroBlackjack.Business.Constants;
using OZeroBlackjack.Business.RequestModel;
using System.Collections.Generic;
using System.Linq;

namespace OZeroBlackjack.Business
{
    public class DecisionProcessor : IDecisionProcessor
    {
        private const int BetAmount = 100;
        private Dictionary<string, DeckCard> DeckStatus { get; set; }

        public string Process(TurnParams parameters)
        {
            var currentScore = GetScore(parameters.CurrentCards);

            var finalDecision = GetBasicDecision(currentScore);

            finalDecision = GetCountedDecision(finalDecision, currentScore, parameters.CardHistory);

            if (finalDecision == Move.DOUBLE && (parameters.MoneyInHand < (BetAmount * 2) || parameters.CurrentCards.Length != 2))
            {
                finalDecision = Move.HIT;
            }

            if (finalDecision != Move.DOUBLE && finalDecision != Move.HIT && finalDecision != Move.STAND)
            {
                finalDecision = Move.STAND;
            }

            return finalDecision.ToString();
        }

        private char GetCountedDecision(char currentDecision, int currentScore, IEnumerable<string> cardHistory)
        {
            var move = currentDecision;

            if (move == Move.NONE)
            {
                move = Move.STAND;
            }

            var trueCount = GetTrueCount(cardHistory);

            ProcessCardHistory(cardHistory);

            var hasMoreLowCards = trueCount > 0;
            var hasMoreHighCards = trueCount < 0;

            var hasMostLowCards = trueCount > 2;
            var hasMostHighCards = trueCount < -2;

            var hasNeutralCards = trueCount == 0;

            if (currentScore == 8)
            {
                move = Move.HIT;

                if (HasFavourableCardsPresent(currentScore))
                {
                    move = Move.DOUBLE;
                }
            }
            else if (currentScore == 9)
            {
                move = Move.HIT;

                if (hasMostLowCards)
                {
                    move = Move.DOUBLE;
                }
            }
            else if (currentScore == 10)
            {
                move = Move.HIT;

                if (hasNeutralCards || hasMoreLowCards)
                {
                    move = Move.DOUBLE;
                }
            }
            else if (currentScore == 11)
            {
                move = Move.HIT;

                if (hasMostHighCards || HasFavourableCardsPresent(currentScore))
                {
                    move = Move.DOUBLE;
                }
            }
            else if (currentScore == 12 && !hasMostHighCards && HasFavourableCardsPresent(currentScore))
            {
                move = Move.HIT;
            }
            else if (currentScore == 13 && !hasMostHighCards && HasFavourableCardsPresent(currentScore))
            {
                move = Move.HIT;
            }
            else if (currentScore > 13 && currentScore <= 16 && !hasMostHighCards && HasFavourableCardsPresent(currentScore))
            {
                move = Move.HIT;
            }

            return move;
        }

        private void ProcessCardHistory(IEnumerable<string> cardHistory)
        {
            DeckStatus = new Dictionary<string, DeckCard>();

            var cardGroups = cardHistory.GroupBy(x => x);

            foreach (var cardGroup in cardGroups)
            {
                var card = Cards.CardsWithValue[cardGroup.Key.ToUpper()];

                var currentCount = cardGroup.Count();

                var newDeckCard = new DeckCard
                {
                    CountValue = card.CountValue,
                    Name = card.Name,
                    ProbabilityInNextDraw = 0,
                    InCount = currentCount,
                    OutCount = 4 - currentCount
                };

                var probabilityOfCard = GetProbabilityOfCard(newDeckCard, cardHistory);

                newDeckCard.ProbabilityInNextDraw = probabilityOfCard;

                DeckStatus.Add(newDeckCard.Name, newDeckCard);
            }

            foreach (var card in Cards.CardsWithValue)
            {
                if (DeckStatus.ContainsKey(card.Key)) continue;

                var newDeckCard = new DeckCard
                {
                    CountValue = card.Value.CountValue,
                    Name = card.Value.Name,
                    ProbabilityInNextDraw = 0,
                    InCount = 0,
                    OutCount = 4
                };

                var probabilityOfCard = GetProbabilityOfCard(newDeckCard, cardHistory);

                newDeckCard.ProbabilityInNextDraw = probabilityOfCard;

                DeckStatus.Add(newDeckCard.Name, newDeckCard);
            }
        }

        private char GetBasicDecision(int currentScore)
        {
            var move = Move.NONE;

            if (currentScore > 16)
            {
                move = Move.STAND;
            }
            else if (currentScore < 8)
            {
                move = Move.HIT;
            }
            else if (currentScore == 11)
            {
                move = Move.DOUBLE;
            }

            return move;
        }

        private int GetScore(IEnumerable<string> cards)
        {
            return cards.Sum(currentCard => Cards.CardsWithValue[currentCard].Value);
        }

        private int GetTrueCount(IEnumerable<string> cards)
        {
            const int numberOfDecks = 1;

            return cards.Sum(card => Cards.CardsWithValue[card].CountValue) / numberOfDecks;
        }

        private float GetProbabilityOfCard(DeckCard deckCard, IEnumerable<string> history)
        {
            var cardsOutOfDeck = history.Count();

            return (float)(4 - deckCard.InCount) / (52 - cardsOutOfDeck);
        }

        private bool HasFavourableCardsPresent(int currentScore, bool forDouble = false)
        {
            if (currentScore == 8)
            {
                var favourableCards = new List<string> { "10", "J", "Q", "K" };

                var probability = favourableCards.Sum(x => DeckStatus[x].ProbabilityInNextDraw) * 100;

                return probability > 80;
            }

            if (currentScore == 11)
            {
                var favourableCards = new List<string> { "10", "J", "Q", "K" };

                var probability = favourableCards.Sum(x => DeckStatus[x].ProbabilityInNextDraw) * 100;

                return probability > 80;
            }

            if (currentScore == 12)
            {
                var favourableCards = new List<string> { "7", "8", "9" };

                var probability = favourableCards.Sum(x => DeckStatus[x].ProbabilityInNextDraw) * 100;

                return probability > 80;
            }

            if (currentScore == 13)
            {
                var favourableCards = new List<string> { "7", "8", "6" };

                var probability = favourableCards.Sum(x => DeckStatus[x].ProbabilityInNextDraw) * 100;

                return probability > 50;
            }

            if (currentScore > 13 && currentScore <= 16)
            {
                var favourableCards = new List<string> { "4", "3", "2" };

                var probability = favourableCards.Sum(x => DeckStatus[x].ProbabilityInNextDraw) * 100;

                return probability > 80;
            }

            return false;
        }

        private int? GetOpponentScore(List<string> cardHistory, List<string> currentCards)
        {
            if (currentCards.Count != 2)
            {
                return null;
            }

            var opponentsCards = cardHistory.Skip(cardHistory.Count - 4).Where(x => !x.Equals(currentCards[0]) && !x.Equals(currentCards[1])).ToList();

            return GetScore(opponentsCards);
        }
    }
}