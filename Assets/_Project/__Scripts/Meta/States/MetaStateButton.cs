using _Project.__Scripts.Shared.UI;
using UnityEngine;
using VContainer;

namespace _Project.__Scripts.Meta.States
{
    public class MetaStateButton : ButtonListener
    {
        [SerializeField] private MetaUIStates metaState;
        
        private UIStateMachine _uiStateMachine;

        [Inject]
        private void Construct(UIStateMachine uiStateMachine)
        {
            _uiStateMachine = uiStateMachine;
        }
        
        protected override void OnClick()
        {
            _uiStateMachine.SetState(metaState);
        }
    }
}
