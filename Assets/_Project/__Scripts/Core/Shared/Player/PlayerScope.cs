using _Project.__Scripts.Core.DicePocker.Player;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.__Scripts.Core.WitchCard.Entities.Players.Components
{
    public class PlayerScope : LifetimeScope
    {
        [SerializeField] private PlayerHandNetworkBehaviour playerHandController;
        [SerializeField] private PlayerNetworkTransform playerNetworkTransform;
        [SerializeField] private VCamMediator vCamMediator;
        

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(playerHandController);
            builder.RegisterInstance(playerNetworkTransform);
            builder.RegisterInstance(vCamMediator);
        }
    }
}
