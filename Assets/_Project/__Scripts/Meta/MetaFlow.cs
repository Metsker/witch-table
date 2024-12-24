using _Project.__Scripts.Meta.States;
using TMPro;
using Unity.Services.Multiplayer;
using UnityEngine;
using VContainer;

namespace _Project.__Scripts.Meta
{
    public class MetaFlow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageTMP;
        
        private UIStateMachine _uiStateMachine;

        [Inject]
        private void Construct(UIStateMachine uiStateMachine)
        {
            _uiStateMachine = uiStateMachine;
        }

        public void OnJoinedSession(ISession session)
        {
        }

        public void OnJoiningSession() =>
            _uiStateMachine.SetState(MetaUIStates.Lobby);

        public void OnError(SessionException sessionException)
        {
            _uiStateMachine.SetState(MetaUIStates.MainMenu);
            messageTMP.SetText(sessionException.Message);
        }
        
        public void OnLeave()
        {
            _uiStateMachine.SetState(MetaUIStates.MainMenu);
        }
    }
}
