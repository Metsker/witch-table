using _Project.__Scripts.Core.WitchCard.Entities.Players.State;
using _Project.__Scripts.Core.WitchCard.Player.State;
using Unity.Netcode;
using VContainer;

namespace _Project.__Scripts.Core.WitchCard.Entities.Players.Classes.Base
{
    public abstract class ClassBase : NetworkBehaviour
    {
        protected ITurnSystem TurnSystem { get; private set; }

        [Inject]
        private void Construct(ITurnSystem turnSystem)
        {
            TurnSystem = turnSystem;
        }
        
        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                TurnSystem.LocalPlayerTurnStarted += OnLocalPlayerTurnStarted;
            }
        }
        
        public override void OnNetworkDespawn()
        {
            if (IsOwner)
            {
                TurnSystem.LocalPlayerTurnStarted -= OnLocalPlayerTurnStarted;
            }
        }

        protected virtual void OnLocalPlayerTurnStarted()
        {
        }
        
        protected virtual void OnLocalPlayerTurnEnded()
        {
        }
    }
}
