using UnityEngine;
using UnityEngine.UI;

namespace _Project.__Scripts.Shared.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonListener : MonoBehaviour
    {
        private Button _button;

        private void Awake() =>
            _button = GetComponent<Button>();

        protected virtual void OnEnable() =>
            _button.onClick.AddListener(OnClick);

        protected virtual void OnDisable() =>
            _button.onClick.RemoveListener(OnClick);

        public void SetInteractable(bool state) =>
            _button.interactable = state;

        protected abstract void OnClick();
    }
}
