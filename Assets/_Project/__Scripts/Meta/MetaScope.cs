using _Project.__Scripts.Meta.States;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.__Scripts.Meta
{
    public class MetaScope  : LifetimeScope
    {
        [SerializeField] private UIStateMachine uiStateMachine;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(uiStateMachine);
        }
        
    }
}
