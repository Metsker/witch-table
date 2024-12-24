using _Project.__Scripts.Shared.UI;
using Unity.Netcode;

namespace _Project.__Scripts.Meta.Network.Buttons
{
    public class ShutdownButton : ButtonListener
    {
        protected override void OnClick()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                NetworkManager.Singleton.Shutdown();
            }
        }
    }
}
