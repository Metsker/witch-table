using _Project.__Scripts.Core.WitchCard.Entities.Players.Classes.Base;
using _Project.__Scripts.Core.WitchCard.Entities.Players.Components;

namespace _Project.__Scripts.Core.WitchCard.Entities.Players.Classes
{
    //подсвечивает случайную пару
    public class SquirrelClass : ClassBase
    {
        protected override void OnLocalPlayerTurnStarted()
        {
            //PlayerHandNetworkBehaviour hand = PlayerFactory.Get<PlayerHandNetworkBehaviour>(TurnSystem.CurrentTurnInfo.PassiveId);
            //hand.MarkPairRpc();
        }
    }
}
