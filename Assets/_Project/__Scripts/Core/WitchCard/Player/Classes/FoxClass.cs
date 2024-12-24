using _Project.__Scripts.Core.WitchCard.Entities.Players.Classes.Base;
using _Project.__Scripts.Core.WitchCard.Entities.Players.Components;
using VContainer;

namespace _Project.__Scripts.Core.WitchCard.Entities.Players.Classes
{
    //кроп карты на 1 круг
    public class FoxClass : ClassBase
    {
        private PlayerHandNetworkBehaviour _hand;

        [Inject]
        private void Construct(PlayerHandNetworkBehaviour hand)
        {
            _hand = hand;
        }
        
        protected override void OnLocalPlayerTurnStarted()
        {
            _hand.AllowCropRpc();
        }
    }
}
