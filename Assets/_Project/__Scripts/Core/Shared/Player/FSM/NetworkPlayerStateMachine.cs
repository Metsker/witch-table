using _Project.__Scripts.Core.WitchCard.Entities.Players.Enums;
using Animancer.FSM;
using Unity.Netcode;
using UnityEngine;

namespace _Project.__Scripts.Core.DicePocker.Player
{
    public class NetworkPlayerStateMachine : NetworkBehaviour
    {
        [SerializeField] private StateMachine<PlayerState> stateMachine;
        [SerializeField] private RollingState rollingState;
        [SerializeField] private LookingState lookingState;
        
        public StateMachine<PlayerState> StateMachine => stateMachine;
    
        protected virtual void Awake()
        {
            stateMachine.InitializeAfterDeserialize();
        }
        
        public void SetLooking() => stateMachine.TrySetState(lookingState);
    }
}
