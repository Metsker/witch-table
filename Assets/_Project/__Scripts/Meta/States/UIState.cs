using NTC.FiniteStateMachine;
using UnityEngine;

namespace _Project.__Scripts.Meta.States
{
    public abstract class UIState : MonoBehaviour, IState<UIStateMachine>
    {
        public UIStateMachine Initializer { get; }
        public IState<UIStateMachine> PreviousState { get; set; }

        private void Awake() =>
            transform.localPosition = Vector3.zero;

        public virtual void OnEnter()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnExit()
        {
            gameObject.SetActive(false);
        }
    }
}
