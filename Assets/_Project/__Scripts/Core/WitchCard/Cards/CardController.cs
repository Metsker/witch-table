using _Project.__Scripts.Core.WitchCard.Entities.Players.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace _Project.__Scripts.Core.WitchCard.Cards
{
    public class CardController : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public CardView View => view;
        public CardModel Model => _model;

        [SerializeField] private CardView view;

        private int Index => _playerHandNb.IndexOf(this);
        public bool Marked { get; private set; }

        private CardModel _model;
        private PlayerHandNetworkBehaviour _playerHandNb;
        private float _clampMin;
        private float _clampMax;
        private Color _markColor;

        public void Init(CardModel model, PlayerHandNetworkBehaviour playerHandNetworkBehaviour)
        {
            _model = model;
            _playerHandNb = playerHandNetworkBehaviour;

            if (model == CardModel.Mock)
                view.InitMockInfo();
            else
                View.InitInfo(model);
            
            _clampMin = transform.localPosition.y - 0.15f;
            _clampMax = transform.localPosition.y + 0.15f;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    
                    if (!_playerHandNb.IsPickable)
                        return;

                    _playerHandNb.SwapCardOwnerRpc(Index); //Ask server to remove card from target, add to player
                    _playerHandNb.DisallowPick(); //Close access to target's hand locally
                    view.ShowReset();
                    break;
                case PointerEventData.InputButton.Right:
                    
                    if (!_playerHandNb.IsCropable)
                        return;

                    _markColor = Random.ColorHSV();
                    view.ShowMark(_markColor);
                    _playerHandNb.DisallowCropRpc();
                    Marked = true;
                    break;
                default:
                    return;
            }
        }

        public void AllowPick()
        {
            if (Marked)
                return;
            
            view.ShowPickable();
        }

        public void DisallowPick()
        {
            if (Marked)
                return;
            
            view.ShowReset();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_playerHandNb.IsPickable)
                return;
            
            view.ShowOverlap();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (Marked)
            {
                view.ShowMark(_markColor);
                return;
            }
            
            if (!_playerHandNb.IsPickable)
                return;
            
            view.ShowPickable();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_playerHandNb.IsOwner)
                return;

            if (eventData.delta.y == 0)
                return;
            
            float y = Mathf.Clamp(transform.localPosition.y + eventData.delta.y * 0.005f, _clampMin, _clampMax);
            _playerHandNb.MoveCard(this, y);
        }
    }
}
