using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.__Scripts.Core.WitchCard.Cards
{
    [CreateAssetMenu(fileName = "CardDataTable", menuName = "Data/CardDataTable")]
    public class CardDataTable : SerializedScriptableObject
    {
        [SerializeField] private Dictionary<CardModel, Sprite> deckTable;

        public Sprite GetSprite(CardModel model) =>
            deckTable[model];
        
        public Sprite GetSpriteForMock() =>
            deckTable[CardModel.Mock];
        
        [Button]
        private void ResetTable()
        {
            deckTable.Clear();

            foreach (CardModel card in Deck.EnumerateAllCards())
                deckTable.Add(card, null);
        }
    }
}
