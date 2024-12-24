using System;
using System.Collections.Generic;
using _Project.__Scripts.Core.WitchCard.Entities;
using _Project.__Scripts.Shared.Data;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.__Scripts.Core.DicePocker.Dice
{
    public class DiceDealer : MonoBehaviour, IDealer
    {
        [SerializeField] private NetworkObject dicePrefab;
        [SerializeField] private Transform origin;
        [SerializeField, Range(0, 5)] private float radius;
        
        public void Deal(IReadOnlyList<ulong> clientsCompleted)
        {
            Debug.Log("DEAL");
            
            //Get skin and spawn
            
            foreach (ulong id in clientsCompleted)
            {
                for (int i = 0; i < StaticData.Dices; i++)
                {
                    NetworkObject dice = NetworkManager.Singleton.SpawnManager.
                        InstantiateAndSpawn(
                            dicePrefab,
                            id,
                            true,
                            position: origin.position + Random.insideUnitSphere * radius,
                            rotation: Random.rotation
                        );
                    dice.GetComponent<Rigidbody>().AddTorque(Vector3.one * Random.value * 10, ForceMode.Impulse);
                }
            }
        }

        public void DiscardPlayers()
        {
            throw new NotImplementedException();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(origin.position, radius);
        }
    }
}
