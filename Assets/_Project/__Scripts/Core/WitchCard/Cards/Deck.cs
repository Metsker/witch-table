using System;
using System.Collections.Generic;
using System.Linq;
using _Project.__Scripts.Core.WitchCard.Cards.Enums;
using _Project.__Scripts.Utilities.Extensions;

namespace _Project.__Scripts.Core.WitchCard.Cards
{
    public class Deck
    {
        public IReadOnlyList<CardModel> Cards => _cards;
        public int Count => _cards.Count;
        
        private readonly List<CardModel> _cards = new ();

        public Deck()
        {
            Reset();
        }

        public CardModel[] TakeAndRemove(int count)
        {
            CardModel[] cards = _cards.Take(count).ToArray();
            _cards.RemoveRange(0, count);
            return cards;
        }

        public void Reset()
        {
            foreach (CardModel card in EnumerateAllCards())
                _cards.Add(card);

            _cards.Shuffle();
        }

        public static IEnumerable<CardModel> EnumerateAllCards()
        {
            foreach (string suitName in Enum.GetNames(typeof(CardSuit)))
            {
                foreach (string valueName in Enum.GetNames(typeof(CardValue)))
                {
                    CardSuit suit = Enum.Parse<CardSuit>(suitName);
                    CardValue value = Enum.Parse<CardValue>(valueName);

                    if (suit == CardSuit.Unknown || value == CardValue.Unknown)
                        continue;
                    
                    CardModel cardModel = new (suit, value);
                    yield return cardModel;
                }
            }
        }
    }
}
