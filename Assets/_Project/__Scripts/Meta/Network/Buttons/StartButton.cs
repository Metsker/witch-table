using System.Collections.Generic;
using _Project.__Scripts.Meta.Network.CharacterSelection;
using _Project.__Scripts.Shared.Data;
using _Project.__Scripts.Shared.UI;
using _Project.__Scripts.Utilities.SceneLoad;
using Unity.Netcode;
using UnityEngine;

namespace _Project.__Scripts.Meta.Network.Buttons
{
    public class StartButton : ButtonListener
    {
        [SerializeField] private CharacterSelectionProvider characterSelectionProvider;

        private void Start()
        {
            SetInteractable(false);
            NetworkManager.Singleton.OnConnectionEvent += OnConnection;
        }

        private void OnDestroy()
        {
            if (NetworkManager.Singleton != null)
                NetworkManager.Singleton.OnConnectionEvent -= OnConnection;
        }

        private void OnConnection(NetworkManager manager, ConnectionEventData data)
        {
            gameObject.SetActive(manager.IsServer);
            SetInteractable(manager.IsServer);
        }

        protected override void OnClick()
        {
            Dictionary<ulong, CharacterModel> selection = characterSelectionProvider.GetSelectionForPlayers();
            
            SceneLoader.LoadSceneNetworked(SceneInBuild.Core, selection);
        }
    }
}
