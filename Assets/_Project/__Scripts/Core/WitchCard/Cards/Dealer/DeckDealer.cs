using System.Collections.Generic;
using _Project.__Scripts.Core.WitchCard.Cards;
using _Project.__Scripts.Core.WitchCard.Entities.Players.Components;
using _Project.__Scripts.Shared.Data;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace _Project.__Scripts.Core.WitchCard.Entities
{
    public class CardDealer : MonoBehaviour, IDealer
    {
        private Deck _deck;

        public void Deal(IReadOnlyList<ulong> clientsCompleted)
        {
            _deck = new Deck();
            DealCards(clientsCompleted);
        }

        public void DiscardPlayers()
        {
            UniTask[] tasks = new UniTask[PlayerFactory.Count];

            for (int i = 0; i < PlayerFactory.Players.Count; i++)
            {
                NetworkClient player = PlayerFactory.Players[i];
                PlayerHandNetworkBehaviour hand = player.PlayerObject.GetComponent<PlayerHandNetworkBehaviour>();
                hand.DiscardAllRpc();
                //tasks[i] = UniTask.WaitUntil(() => hand.Empty);
            }

            //await UniTask.WhenAll(tasks);
        }
        
        private void DealCards(IReadOnlyList<ulong> clientsCompleted)
        {
            int playerCount = NetworkManager.Singleton.ConnectedClients.Count;
            
            if (playerCount < StaticData.MinPlayers)
            {
                Debug.LogError("Not enough players to start.");
                return;
            }
            
            int cardsPerPlayer = _deck.Count / playerCount;
            
            foreach (ulong id in clientsCompleted)
            {
                NetworkObject client = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id);
                PlayerHandNetworkBehaviour playerHandNetworkBehaviour = client.GetComponent<PlayerHandNetworkBehaviour>();
                
                CardModel[] cards = _deck.TakeAndRemove(cardsPerPlayer);
                
                playerHandNetworkBehaviour.InitHandRpc(cards);
            }
        }
    }
}
