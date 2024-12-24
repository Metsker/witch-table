using UnityEngine;

namespace _Project.__Scripts.Core.WitchCard.Cards
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer faceSR;
        [SerializeField] private SpriteRenderer backSR;
        [SerializeField] private CardDataTable cardDataTable;
        
        private Renderer Renderer => backSR;
        public float Wight => faceSR.sprite.bounds.size.x * transform.lossyScale.x;

        private Color _startColor;

        private void Awake()
        {
            _startColor = Renderer.material.color;
        }

        public void InitInfo(CardModel model)
        {
            faceSR.sprite = cardDataTable.GetSprite(model);
            //cardText.SetText(value + " of " + suit);
        }

        public void InitMockInfo()
        {
            faceSR.sprite = cardDataTable.GetSpriteForMock();
        }

        public void ShowPickable()
        {
            Renderer.material.color = Color.green;
        }
        public void ShowReset()
        {
            Renderer.material.color = _startColor;
        }

        public void ShowOverlap()
        {
            Renderer.material.color = Color.yellow;
        }
        
        public void ShowMark(Color color)
        {
            Renderer.material.color = color;
        }
    }
}
