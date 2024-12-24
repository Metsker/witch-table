using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

namespace _Project.__Scripts.Core.WitchCard.Entities.Players.Components
{
    public class PlayerInitializer : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                
            }
        }
    }
}
