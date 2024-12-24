using _Project.__Scripts.Shared.UI;
using Unity.Netcode;

namespace _Project.__Scripts.Meta.Network.Buttons
{
    public class StartHost : ButtonListener
    {
        protected override void OnClick()
        {
            NetworkManager.Singleton.StartHost();
        }
    }
}
