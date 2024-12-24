using _Project.__Scripts.Core.WitchCard.Entities;
using _Project.__Scripts.Core.WitchCard.Flow;
using _Project.__Scripts.Core.WitchCard.Player.State;
using _Project.__Scripts.Core.WitchCard.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.__Scripts.Core
{
    public class CoreScope : LifetimeScope
    {
        [SerializeField] private EntryPoint entryPoint;
        [SerializeField] private UIFactory uiFactory;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<IDealer>();
            builder.RegisterComponentInHierarchy<ITurnSystem>();
            
            builder.RegisterInstance(uiFactory);
            builder.RegisterComponent(entryPoint);
        }
    }
}
