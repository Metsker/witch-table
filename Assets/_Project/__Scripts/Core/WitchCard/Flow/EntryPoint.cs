using System.Collections.Generic;
using System.Linq;
using _Project.__Scripts.Core.WitchCard.Entities;
using _Project.__Scripts.Core.WitchCard.Entities.Players.Classes.Enums;
using _Project.__Scripts.Core.WitchCard.Entities.Players.State;
using _Project.__Scripts.Core.WitchCard.Player.State;
using _Project.__Scripts.Meta.Network.CharacterSelection;
using _Project.__Scripts.Shared.Flow;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace _Project.__Scripts.Core.WitchCard.Flow
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform[] origins;
        [SerializeField] private NetworkObject defaultPlayerPrefab;

        private IDealer _dealer;
        private ITurnSystem _turnSystem;
        private IObjectResolver _resolver;

        [Inject]
        private void Construct(IDealer cardDealer, ITurnSystem turnSystem, IObjectResolver resolver)
        {
            _resolver = resolver;
            _dealer = cardDealer;
            _turnSystem = turnSystem;
        }

        private void Awake()
        {
            if (!NetworkManager.Singleton.IsServer)
                return;

            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
        }

        private void OnDestroy()
        {
            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer)
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnLoadEventCompleted;
        }

        private async void OnLoadEventCompleted(string _ = null, LoadSceneMode __ = default, List<ulong> clientsCompleted = null, List<ulong> ___ = null)
        {
            //TODO: test

            await CreatePlayers(clientsCompleted);

            _dealer.Deal(clientsCompleted);
            _turnSystem.StartFirstCircle();
        }

        private async UniTask CreatePlayers(IReadOnlyList<ulong> clientsCompleted)
        {
            if (!_resolver.TryResolve(out Dictionary<ulong, CharacterModel> playerCharacters))
            {
                playerCharacters = new Dictionary<ulong, CharacterModel>
                {
                    {
                        NetworkManager.ServerClientId, new CharacterModel
                        {
                            character = Character.Fox,
                            prefab = defaultPlayerPrefab
                        }
                    }
                };
            }

            UniTask[] tasks = new UniTask[clientsCompleted.Count];

            for (int index = 0; index < clientsCompleted.Count; index++)
            {
                ulong clientId = clientsCompleted[index];
                tasks[index] = PlayerFactory.Create(playerCharacters[clientId].prefab, origins[index], clientId);
            }

            await UniTask.WhenAll(tasks);
        }
    }
}
