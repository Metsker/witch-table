using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Project.__Scripts.Meta.Network.CharacterSelection
{
    public class CharacterSelectorView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ownerText;
        [SerializeField] private TextMeshProUGUI characterText;

        public void SetOwnerText(string text) =>
            ownerText.SetText(text);

        public void SetCharacterText(string text) =>
            characterText.SetText(text);

        public void VisualizeState(bool state) =>
            transform.DOScale(state ? 1.2f : 1, 0.25f);
    }
}
