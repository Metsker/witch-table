using System;
using _Project.__Scripts.Core.WitchCard.Cards.Enums;
using Unity.Netcode;
using UnityEngine;

namespace _Project.__Scripts.Core.WitchCard.Cards
{
    [Serializable]
    public struct CardModel : INetworkSerializable
    {
        public CardSuit CardSuit => cardSuit;
        public CardValue CardValue => cardValue;
        public bool IsWitch => cardValue == CardValue.Queen && CardSuit == CardSuit.Spades;

        [SerializeField] private CardSuit cardSuit;
        [SerializeField] private CardValue cardValue;

        public static readonly CardModel Mock = new ();
        
        public CardModel(CardSuit cardSuit, CardValue cardValue)
        {
            this.cardSuit = cardSuit;
            this.cardValue = cardValue;
        }
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref cardSuit);
            serializer.SerializeValue(ref cardValue);
        }

        public bool IsPair(CardModel cardModel)
        {
            if (IsWitch || cardModel.IsWitch)
                return false;

            return cardValue == cardModel.CardValue;
        }
        
        public static bool operator ==(CardModel m1, CardModel m2) =>
            m1.CardSuit == m2.CardSuit && m1.CardValue == m2.CardValue;

        public static bool operator !=(CardModel m1, CardModel m2) =>
            !(m1 == m2);

        public override bool Equals(object obj)
        {
            if (obj is CardModel cardModel)
                return this == cardModel;
            
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)cardSuit, (int)cardValue);
        }

        public override string ToString() =>
            cardSuit + " : " + cardValue;
    }
}
