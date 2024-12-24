using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.__Scripts.Meta.Network.CharacterSelection
{
    public class CharacterSelectorController : NetworkBehaviour
    {
        public const ulong NotSelected = 420;
        public CharacterModel Model => model;

        [SerializeField] private CharacterSelectorView view;
        [SerializeField] private CharacterModel model;
        [SerializeField] private Toggle toggle;

        public NetworkVariable<ulong> owner = new ();

        private void Awake()
        {
            toggle.interactable = false;
            view.SetCharacterText(model.character.ToString());
        }

        private void OnEnable() =>
            toggle.onValueChanged.AddListener(OnToggleStateChanged);

        private void OnDisable() =>
            toggle.onValueChanged.RemoveListener(OnToggleStateChanged);

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                owner.Value = NotSelected;
                toggle.interactable = true;
            }
            else
                OnValueChanged(NotSelected, owner.Value);
            
            owner.OnValueChanged += OnValueChanged;
        }

        public override void OnNetworkDespawn() =>
            owner.OnValueChanged -= OnValueChanged;

        [Rpc(SendTo.Server)]
        private void SelectRpc(bool toggleState, RpcParams @params = default) =>
            owner.Value = toggleState ? @params.Receive.SenderClientId : NotSelected;

        private void OnValueChanged(ulong _, ulong newValue)
        {
            bool state = newValue != NotSelected;
           
            switch (state)
            {
                case true:
                {
                    if (owner.Value == NetworkManager.LocalClientId)
                    {
                        view.SetOwnerText("Me");
                        toggle.interactable = true;
                    }
                    else
                    {
                        view.SetOwnerText($"Player {owner.Value}");
                        toggle.interactable = false;
                    }
                    break;
                }
                case false:
                    view.SetOwnerText("Available");
                    toggle.interactable = true;
                    break;
            }
            view.VisualizeState(state);
        }

        private void OnToggleStateChanged(bool state) =>
            SelectRpc(state);
    }
}
