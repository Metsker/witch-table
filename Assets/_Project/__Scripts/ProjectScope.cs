using _Project.__Scripts.Shared.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _Project.__Scripts
{
    public class ProjectScope : LifetimeScope
    {
        [SerializeField] private InputReader inputReader;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(inputReader);
        }
    }
}
