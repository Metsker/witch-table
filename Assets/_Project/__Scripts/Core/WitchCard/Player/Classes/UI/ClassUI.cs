using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Project.__Scripts.Core.WitchCard.Entities.Players.Classes.UI
{
    public class ClassUI : MonoBehaviour
    {
        public Button Button => button;
        
        [SerializeField] private Button button;

        public void RegisterButtonAction(UnityAction action)
        {
            button.onClick.AddListener(action);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }
    }
}
