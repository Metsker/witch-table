using System.Collections.Generic;
using System.Linq;
using NTC.FiniteStateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.__Scripts.Meta.States
{
    public class UIStateMachine : SerializedMonoBehaviour
    {
        [SerializeField] private Dictionary<MetaUIStates, IState<UIStateMachine>> states;
        
        private StateMachine<UIStateMachine> _stateMachine = new();

        private void Start()
        {
            _stateMachine.AddStates(states.Values.ToArray());
            
            foreach (IState<UIStateMachine> v in states.Values)
                v.OnExit();
            
            _stateMachine.SetState(states[0]);
        }

        public void SetState(MetaUIStates state) =>
            _stateMachine.SetState(states[state]);
        
        public void Back() =>
            _stateMachine.Back();
    }
}
