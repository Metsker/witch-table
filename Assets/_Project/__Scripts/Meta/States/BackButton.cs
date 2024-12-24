using _Project.__Scripts.Shared.UI;
using VContainer;

namespace _Project.__Scripts.Meta.States
{
    public class BackButton : ButtonListener
    {
        private UIStateMachine _uiStateMachine;

        [Inject]
        private void Construct(UIStateMachine uiStateMachine)
        {
            _uiStateMachine = uiStateMachine;
        }
        
        protected override void OnClick() =>
            _uiStateMachine.Back();
    }
}
