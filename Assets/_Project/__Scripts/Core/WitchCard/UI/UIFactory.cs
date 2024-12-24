using UnityEngine;

namespace _Project.__Scripts.Core.WitchCard.UI
{
    public class UIFactory : MonoBehaviour
    {
        [SerializeField] private RectTransform container;
        
        public T Create<T>(T prefab) where T : MonoBehaviour
        {
            return Instantiate(prefab, container);
        }
    }
}
