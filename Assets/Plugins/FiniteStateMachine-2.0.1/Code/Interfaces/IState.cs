using System.Threading.Tasks;

namespace NTC.FiniteStateMachine
{
    public interface IState<TInitializer>
    {
        public TInitializer Initializer { get; }
        IState<TInitializer> PreviousState { get; set; }
        public void OnEnter();
        public void OnRun() { }
        public void OnExit();
    }
}