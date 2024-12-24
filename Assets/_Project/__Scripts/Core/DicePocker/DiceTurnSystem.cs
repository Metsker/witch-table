using System;
using _Project.__Scripts.Core.WitchCard.Player.State;
using Unity.Netcode;
using UnityEngine;

namespace _Project.__Scripts.Core.DicePocker
{
    public class DiceTurnSystem : NetworkBehaviour, ITurnSystem
    {
        public event Action LocalPlayerTurnStarted;
        public event Action LocalPlayerTurnEnded;
        
        public void StartFirstCircle()
        {
            Debug.Log("StartFirstCircle");
        }

        public void ProgressRoundRpc()
        {
            Debug.Log("ProgressRoundRpc");
        }
    }
}
