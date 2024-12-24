using _Project.__Scripts.Core.WitchCard.Entities.Players.Classes.Base;
using _Project.__Scripts.Core.WitchCard.Entities.Players.Classes.UI;
using _Project.__Scripts.Core.WitchCard.UI;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Project.__Scripts.Core.WitchCard.Entities.Players.Classes
{
    //дополнительный обмен
    public class RabbitClass : ClassBase
    {
        [SerializeField] private ClassUI uiPrefab;
        
        private UIFactory _uiFactory;
        private Button _button;

        [Inject]
        private void Construct(UIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsOwner)
                return;
            
            ClassUI ui = _uiFactory.Create(uiPrefab);
            _button = ui.Button;
            _button.onClick.AddListener(OnButtonClicked);
        }
        
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            
            if (!IsOwner)
                return;
            
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            //TurnSystem.InsertTurnCopyRpc();
            
            _button.interactable = false;
        }
        
        protected override void OnLocalPlayerTurnStarted() =>
            _button.interactable = true;

        protected override void OnLocalPlayerTurnEnded() =>
            _button.interactable = false;
    }
}
