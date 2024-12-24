using System.Collections.Generic;
using System.Linq;
using _Project.__Scripts.Core.WitchCard.Entities.Players.Components;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.__Scripts.Core.WitchCard.Entities
{
    public static class PlayerFactory
    {
        public static IReadOnlyList<NetworkClient> Players => NetworkManager.Singleton.ConnectedClients.Values.ToList();
        public static IReadOnlyList<ulong> PlayerIds => NetworkManager.Singleton.ConnectedClients.Keys.ToList();
        public static int Count => NetworkManager.Singleton.ConnectedClients.Count;
        public static int InGameCount => NetworkManager.Singleton.ConnectedClients.Values.Select(c => c.PlayerObject.GetComponent<PlayerHandNetworkBehaviour>()).Count(h => !h.Empty);
        
        public static async UniTask Create(NetworkObject playerPrefab, Transform origin, ulong ownerId)
        {
            NetworkObject playerObj = Object.Instantiate(
                playerPrefab,
                origin.position + new Vector3(0, playerPrefab.transform.localScale.y, 0),
                origin.rotation);
            
            playerObj.SpawnAsPlayerObject(ownerId, true);
            
            await UniTask.WaitUntil(() => playerObj.IsSpawned);
        }

        [Pure]
        public static T GetLocal<T>() =>
            Get<T>(NetworkManager.Singleton.LocalClientId);

        [Pure]
        public static T Get<T>(ulong id)
        {
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(id, out NetworkClient networkClient))
            {
                if (networkClient.PlayerObject.TryGetComponent(out T component))
                    return component;
                
                Debug.LogError("No component of type " + typeof(T));
            }
            else
                Debug.LogError("Player not found with id: " + id);
            
            return default;
        }

        public static int IndexOf(ulong id) =>
            NetworkManager.Singleton.ConnectedClients.Keys.ToList().IndexOf(id);
        
        public static ulong IdOf(int index) =>
            NetworkManager.Singleton.ConnectedClients.Keys.ToList()[index];

        public static bool HasLoser(out ulong id)
        {
            PlayerHandNetworkBehaviour[] hands = NetworkManager.Singleton.ConnectedClients.Values.Select(c => c.PlayerObject.GetComponent<PlayerHandNetworkBehaviour>()).ToArray();
            if (hands.Count(h => !h.Empty) == 1)
            {
                id = hands.First(h => !h.Empty).OwnerClientId;
                return true;
            }
            id = default(ulong);
            return false;
        }
    }
}
