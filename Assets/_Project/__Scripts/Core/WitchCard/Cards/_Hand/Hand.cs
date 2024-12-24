using System;
using System.Collections.Generic;
using System.Linq;
using _Project.__Scripts.Core.WitchCard.Entities.Players.Components;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.__Scripts.Core.WitchCard.Cards._Hand
{
    public class Hand : IHand
    {
        private const float SwapDuration = 2f;
        
        public IReadOnlyList<CardController> Cards => _cards;
        public bool Empty => _cards.Count == 0;
        public Vector3 NextCardWorldPosition => _cards[^1].transform.position + _playerHandNetworkBehaviour.Origin.root.transform.right * _cards[^1].View.Wight ;

        private readonly List<CardController> _cards = new();
        private readonly PlayerHandNetworkBehaviour _playerHandNetworkBehaviour;

        public Hand(PlayerHandNetworkBehaviour playerHandNetworkBehaviour)
        {
            _playerHandNetworkBehaviour = playerHandNetworkBehaviour;
        }

        public void Init(CardModel[] cardModels)
        {
            foreach (CardModel model in cardModels)
                Add(model);
            
            Rearrange();
        }

        public void InitMock(int mockCount)
        {
            for (int i = 0; i < mockCount; i++)
                Add(CardModel.Mock);
            
            Rearrange();
        }

        public void Add(CardModel cardModel)
        {
            CardController card = _playerHandNetworkBehaviour.Spawn();
                
            card.Init(cardModel, _playerHandNetworkBehaviour);
            _cards.Add(card);
        }
        
        public void Swap(CardModel cardModel, CardController card, Action onComplete = null)
        {
            Vector3 nextPos = NextCardWorldPosition;
            
            _cards.Add(card);
            card.transform.SetParent(_playerHandNetworkBehaviour.Origin);
            card.transform
                .DOMove(nextPos, SwapDuration)
                .OnComplete(() =>
                {
                    card.Init(cardModel, _playerHandNetworkBehaviour);
                    onComplete?.Invoke();
                });

        }

        public void Rearrange()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                CardController card = _cards[i];
                Transform origin = _playerHandNetworkBehaviour.Origin;

                Vector3 pos = origin.position 
                    + origin.root.transform.right
                    * card.View.Wight 
                    * i;
                
                card.transform.SetPositionAndRotation(pos, origin.root.rotation);
            }
        }

        public void AllowPick() =>
            _cards.ForEach(c => c.AllowPick());

        public void DisallowPick() =>
            _cards.ForEach(c => c.DisallowPick());

        public void TrimPairs()
        {
            List<CardController> cards = Cards.ToList();
            List<CardController> pairs = new ();

            foreach (CardController card in cards)
            {
                if (pairs.Contains(card))
                    continue;

                CardController pair = cards
                    .Where(c => c.Model != card.Model && !pairs.Contains(c))
                    .FirstOrDefault(c => card.Model.IsPair(c.Model));

                if (pair == null)
                    continue;
                
                pairs.AddRange(new[] { card, pair });
            }
            Debug.Log("Pairs: " + pairs.Count);
            
            _playerHandNetworkBehaviour.TrimPairsRpc(pairs.Select(p => _cards.IndexOf(p)).OrderByDescending(c => c).ToArray());
        }

        public int IndexOf(CardController cardController) =>
            _cards.IndexOf(cardController);

        public void DiscardAll()
        {
            for (int i = _cards.Count - 1; i >= 0; i--)
                DiscardAt(i);
        }

        public void RemoveAt(int index) =>
            _cards.RemoveAt(index);

        public Tween DiscardAt(int index)
        {
            var tween = Discard(Cards[index]);
            _cards.RemoveAt(index);
            return tween;
        }

        private Tween Discard(CardController card)
        {
            return card.transform
                .DOMoveY(3, 1)
                .SetRelative()
                .OnComplete(() => Object.Destroy(card.gameObject));
        }
    }
}
