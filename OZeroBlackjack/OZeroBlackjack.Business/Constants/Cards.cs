using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OZeroBlackjack.Business.Constants
{
    public static class Cards
    {
        public static readonly IDictionary<string, Card> CardsWithValue = new ReadOnlyDictionary<string, Card>(new Dictionary<string, Card>()
        {
            {"A", new Card { Name = "A", Value = 1, CountValue = 1 }},
            {"2", new Card { Name = "2", Value = 2, CountValue = 1 }},
            {"3", new Card { Name = "3", Value = 3, CountValue = 1 }},
            {"4", new Card { Name = "4", Value = 4, CountValue = 1 }},
            {"5", new Card { Name = "5", Value = 5, CountValue = 1 }},
            {"6", new Card { Name = "6", Value = 6, CountValue = 1 }},
            {"7", new Card { Name = "7", Value = 7, CountValue = 0 }},
            {"8", new Card { Name = "8", Value = 8, CountValue = 0 }},
            {"9", new Card { Name = "9", Value = 9, CountValue = 0 }},
            {"10", new Card { Name = "10", Value = 10, CountValue = -1 }},
            {"J", new Card { Name = "J", Value = 10, CountValue = -1 }},
            {"Q", new Card { Name = "Q", Value = 10, CountValue = -1 }},
            {"K", new Card { Name = "K", Value = 10, CountValue = -1 }}
        });
    }

    public class Card
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public int CountValue { get; set; }
    }

    public class DeckCard : Card
    {
        public int InCount { get; set; }
        public int OutCount { get; set; }
        public float ProbabilityInNextDraw { get; set; }
    }
}