using System;
using _Project.__Scripts.Core.DicePocker.Player;
using _Project.__Scripts.Shared.Input;
using Animancer.FSM;
using VContainer;

namespace _Project.__Scripts.Core.WitchCard.Entities.Players.Enums
{
    public abstract class PlayerState : StateBehaviour
    {
        protected InputReader InputReader { get; private set; }
        protected VCamMediator VCamMediator { get; private set; }

        [Inject]
        private void Construct(InputReader inputReader, VCamMediator vCamMediator)
        {
            VCamMediator = vCamMediator;
            InputReader = inputReader;
        }
    }
}
