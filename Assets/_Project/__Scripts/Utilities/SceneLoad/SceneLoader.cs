using System.Threading;
using _Project.__Scripts.Shared.Data;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace _Project.__Scripts.Utilities.SceneLoad
{
    public static class SceneLoader
    {
        private static readonly CancellationTokenSource SceneLoadTokenSource = new ();
        
        public static async void LoadSceneNetworked(SceneInBuild scene, params object[] dependencies)
        {
            if (!NetworkManager.Singleton.IsListening || !NetworkManager.Singleton.IsServer)
                return;
            
            using (LifetimeScope.Enqueue(builder => dependencies.ForEach(d => builder.RegisterInstance(d).As(d.GetType()))))
            {
                NetworkManager.Singleton.SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Single);
                NetworkManager.Singleton.SceneManager.OnLoadComplete += CancelAwaiting;
                
                await UniTask.WaitUntilCanceled(SceneLoadTokenSource.Token);
                
                NetworkManager.Singleton.SceneManager.OnLoadComplete -= CancelAwaiting;
            }
        }
        
        public static async void LoadSceneLocally(SceneInBuild scene, params object[] dependencies)
        {
            using (LifetimeScope.Enqueue(builder => dependencies.ForEach(d => builder.RegisterInstance(d).As(d.GetType()))))
            {
                SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Single);
                SceneManager.sceneLoaded += CancelAwaiting;
                
                await UniTask.WaitUntilCanceled(SceneLoadTokenSource.Token);
                
                SceneManager.sceneLoaded -= CancelAwaiting;
            }
        }

        private static void CancelAwaiting(Scene _, LoadSceneMode __) =>
            SceneLoadTokenSource.Cancel();

        private static void CancelAwaiting(ulong _, string __, LoadSceneMode ___) =>
            SceneLoadTokenSource.Cancel();
    }
}
