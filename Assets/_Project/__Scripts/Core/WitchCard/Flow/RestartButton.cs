using System.Linq;
using _Project.__Scripts.Core.WitchCard.Entities;
using _Project.__Scripts.Core.WitchCard.Entities.Players.State;
using _Project.__Scripts.Core.WitchCard.Player.State;
using _Project.__Scripts.Shared.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using VContainer;

namespace _Project.__Scripts.Core.WitchCard.Flow
{
    public class RestartButton : ButtonListener
    {
        private IDealer _cardDealer;
        private ITurnSystem _turnSystem;

        [Inject]
        private void Construct(IDealer cardDealer, ITurnSystem turnSystem)
        {
            _cardDealer = cardDealer;
            _turnSystem = turnSystem;
        }

        private void Start()
        {
            if (!NetworkManager.Singleton.IsServer)
                gameObject.SetActive(false);
        }

        protected override void OnClick()
        {
            NetworkManager.Singleton.SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            /*//TODO: test
            _cardDealer.DiscardPlayers();
            _cardDealer.Deal(PlayerFactory.PlayerIds);
            _turnSystem.StartFirstCircle();*/
        }
    }
}
