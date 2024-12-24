using System;
using Cysharp.Threading.Tasks;
using EasyTransition;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.__Scripts
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private TransitionSettings _transition;
        
        private async void Start()
        {
            NetworkManager.Singleton.StartHost();

            var t = await TransitionManager.Instance.TransitionIn(_transition);
            NetworkManager.Singleton.SceneManager.LoadScene("Core", LoadSceneMode.Single);
            await TransitionManager.Instance.TransitionOut(t.GetComponent<Transition>(), _transition);
        }
    }
}
