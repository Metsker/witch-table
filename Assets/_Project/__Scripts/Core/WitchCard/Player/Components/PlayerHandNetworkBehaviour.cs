using _Project.__Scripts.Core.WitchCard.Cards;
using _Project.__Scripts.Core.WitchCard.Cards._Hand;
using _Project.__Scripts.Core.WitchCard.Entities.Players.Classes;
using _Project.__Scripts.Core.WitchCard.Entities.Players.Components.Interfaces;
using _Project.__Scripts.Core.WitchCard.Entities.Players.State;
using _Project.__Scripts.Core.WitchCard.Player.State;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace _Project.__Scripts.Core.WitchCard.Entities.Players.Components
{
    public class PlayerHandNetworkBehaviour : NetworkBehaviour, IPlayerStateListener
    {
        [SerializeField] private Transform handOrigin;
        [Space]
        [SerializeField] private CardController cardPrefab;

        public Transform Origin => handOrigin;
        public bool Empty => _hand.Empty;
        public bool IsPickable => !IsOwner && _isPickable;
        public bool IsCropable => IsOwner && isCropable.Value;

        private readonly NetworkVariable<bool> isCropable = new ();

        private Hand _hand;
        private ITurnSystem _turnSystem;
        private bool _isPickable;

        [Inject]
        private void Construct(ITurnSystem turnSystem)
        {
            _turnSystem = turnSystem;
        }
        
        public override void OnNetworkSpawn()
        {
            /*if (IsOwner)
                handOrigin.DOLocalMoveY(-0.4f, 0).SetRelative();*/

            _hand = new Hand(this);
        }
        
        public void AllowPickRpc()
        {
            _isPickable = true;
            
            _hand.AllowPick();
        }
        
        public void DisallowPick()
        {
            _isPickable = false;
            
            _hand.DisallowPick();
        }
        
        //Move to factory
        public CardController Spawn() =>
            Instantiate(cardPrefab, handOrigin);

        public int IndexOf(CardController cardController) =>
            _hand.IndexOf(cardController);

        public void MoveCard(CardController cardController, float newY) =>
            MoveCardRpc(IndexOf(cardController), newY);

        [Rpc(SendTo.Everyone)]
        private void MoveCardRpc(int index, float newY)
        {
            CardController card = _hand.Cards[index];
            card.transform.localPosition = new Vector3(card.transform.localPosition.x, newY, card.transform.localPosition.z);
        }
        
        [Rpc(SendTo.Owner)]
        public void InitHandRpc(CardModel[] cards)
        {
            _hand.Init(cards);
            CreateMockHandRpc(_hand.Cards.Count);
            _hand.TrimPairs();
        }

        [Rpc(SendTo.NotMe)]
        private void CreateMockHandRpc(int cardsCount) =>
            _hand.InitMock(cardsCount);
        
        [Rpc(SendTo.Owner)]
        public void SwapCardOwnerRpc(int index, RpcParams rpcParams = default)
        {
            CardModel cardModel = _hand.Cards[index].Model;
            PlayerHandNetworkBehaviour activeHandNb = PlayerFactory.Get<PlayerHandNetworkBehaviour>(rpcParams.Receive.SenderClientId);

            activeHandNb.SwapCardViewsUnsafeRpc(index, cardModel);
            activeHandNb.SwapCardViewsSafeRpc(index);
        }

        [Rpc(SendTo.Owner)]
        private void SwapCardViewsUnsafeRpc(int index, CardModel cardModel, RpcParams rpcParams = default)
        {
            PlayerHandNetworkBehaviour passiveHand = PlayerFactory.Get<PlayerHandNetworkBehaviour>(rpcParams.Receive.SenderClientId);
            CardController card = passiveHand._hand.Cards[index];

            passiveHand._hand.RemoveAt(index);
            
            _hand.Swap(cardModel, card, () =>
            {
                passiveHand._hand.Rearrange();
                _hand.Rearrange();
                _hand.TrimPairs();
                _turnSystem.ProgressRoundRpc();
            });
        }

        [Rpc(SendTo.NotOwner)]
        private void SwapCardViewsSafeRpc(int index, RpcParams rpcParams = default)
        {
            PlayerHandNetworkBehaviour passiveHand = PlayerFactory.Get<PlayerHandNetworkBehaviour>(rpcParams.Receive.SenderClientId);
            CardController card = passiveHand._hand.Cards[index];
            
            //Me - passiveHand.transform.root.name
            //You - transform.root.name
            
            passiveHand._hand.RemoveAt(index);
            
            _hand.Swap(CardModel.Mock, card, () => passiveHand._hand.Rearrange());
        }
        
        [Rpc(SendTo.Everyone)]
        public void TrimPairsRpc(int[] indexes)
        {
            Sequence sequence = DOTween
                .Sequence(gameObject)
                .AppendInterval(1);
            
            foreach (int index in indexes)
                sequence.Join(_hand.DiscardAt(index));
            
            sequence.OnComplete(() => _hand.Rearrange());
        }

        [Rpc(SendTo.Everyone)]
        public void DiscardAllRpc() =>
            _hand.DiscardAll();

        [Rpc(SendTo.Server)]
        public void AllowCropRpc()
        {
            if (!gameObject.TryGetComponent(out FoxClass _))
                return;
            
            isCropable.Value = true;
        }
        
        [Rpc(SendTo.Server)]
        public void DisallowCropRpc()
        {
            if (!gameObject.TryGetComponent(out FoxClass _))
                return;
            
            isCropable.Value = false;
        }

        public void MarkPairRpc()
        {
            //throw new System.NotImplementedException();
        }
    }
}
