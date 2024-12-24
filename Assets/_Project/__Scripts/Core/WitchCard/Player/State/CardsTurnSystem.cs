using System;
using System.Collections.Generic;
using System.Linq;
using _Project.__Scripts.Core.WitchCard.Entities;
using _Project.__Scripts.Core.WitchCard.Entities.Players.Classes.Enums;
using _Project.__Scripts.Core.WitchCard.Entities.Players.Components;
using _Project.__Scripts.Core.WitchCard.Entities.Players.State;
using _Project.__Scripts.Meta.Network.CharacterSelection;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace _Project.__Scripts.Core.WitchCard.Player.State
{
    public interface ITurnSystem
    {
        event Action LocalPlayerTurnStarted;
        event Action LocalPlayerTurnEnded;
        void StartFirstCircle();
        void ProgressRoundRpc();
    }

    public class CardsTurnSystem : NetworkBehaviour, ITurnSystem
    {
        [SerializeField] private TextMeshProUGUI winnerTmp;
        
        private const float TurnEndDelay = 2f;
        
        private readonly Queue<TurnInfo> roundQueue = new ();

        public TurnInfo CurrentTurnInfo => _currentTurnInfo.Value;
        private bool _gameStarted;
        
        private readonly NetworkVariable<TurnInfo> _currentTurnInfo = new ();
        private int _currentPicks = 1;
        
        private IObjectResolver _resolver;
        private Dictionary<ulong, CharacterModel> playerCharacters;
        private bool _catPicked;

        public event Action LocalPlayerTurnStarted;
        public event Action LocalPlayerTurnEnded;

        [Inject]
        private void Construct(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                playerCharacters = _resolver.Resolve<Dictionary<ulong, CharacterModel>>();
                _catPicked = playerCharacters.Any(p => p.Value.character == Character.Cat);
            }
        }

        public void StartFirstCircle()
        {
            SetLabelRpc(" ");
            
            roundQueue.Clear();
            
            InitCircle();
            ProgressRoundRpc();
        }

        private void TryStartNewCircle()
        {
            if (PlayerFactory.HasLoser(out ulong loserId))
            {
                //TODO: End game
                print("Game ended by player: " + loserId);
                SetLabelRpc($"{(NetworkManager.ServerClientId == loserId ? "Host" : "Client")} lost the game");
                return;
            }
            
            print("New circle");
            
            InitCircle();
            ProgressRoundRpc();
        }

        [Rpc(SendTo.Server)]
        public void ProgressRoundRpc()
        {
            _currentPicks--;
            
            if (HavePicksLeft())
                return;

            ProgressRoundAsync();
        }

        [Rpc(SendTo.Everyone)]
        public void SetLabelRpc(string text)
        {
            winnerTmp.SetText(text);
        }

        private async void ProgressRoundAsync()
        {
            await UniTask.WaitForSeconds(TurnEndDelay);
            
            if (_gameStarted)
                LocalPlayerTurnEnded?.Invoke();
            
            while (roundQueue.TryDequeue(out TurnInfo info))
            {
                ulong activeId = info.ActiveId;
                ulong passiveId = info.PassiveId;
                
                if (IsEmpty(activeId) || IsEmpty(passiveId))
                    continue;

                _currentTurnInfo.Value = info;
                _currentPicks = _currentTurnInfo.Value.MaxPicks;
                _gameStarted = true;
                
                ResetOthers(activeId, passiveId);
                
                RequestTurnRpc(passiveId, RpcTarget.Single(activeId, RpcTargetUse.Temp));
                return;
            }
            TryStartNewCircle();
        }

        private bool HavePicksLeft()
        {
            if (_currentPicks > 0 && _gameStarted && !IsEmpty(_currentTurnInfo.Value.PassiveId))
            {
                RequestTurnRpc(_currentTurnInfo.Value.PassiveId, RpcTarget.Single(_currentTurnInfo.Value.ActiveId, RpcTargetUse.Temp));
                return true;
            }
            return false;
        }

        private bool IsEmpty(ulong id) =>
            PlayerFactory.Get<PlayerHandNetworkBehaviour>(id).Empty;

        private void InitCircle()
        {
            int count = PlayerFactory.Count;
            
            for (int i = 0; i < count; i++)
            {
                int maxPicks = _catPicked && playerCharacters[PlayerFactory.IdOf(i)].character == Character.Cat ? 2 : 1;
                
                TurnInfo first = new ()
                {
                    ActiveIndex = i,
                    PassiveIndex = (i + 1) % count,
                    MaxPicks = maxPicks
                };
                TurnInfo second = new ()
                {
                    ActiveIndex = (i + 1) % count,
                    PassiveIndex = i,
                    MaxPicks = maxPicks
                };
                
                if (i % 2 != 0 || i == count - 1)
                    (first, second) = (second, first);
                
                roundQueue.Enqueue(first);
                roundQueue.Enqueue(second);
            }
        }

        [Rpc(SendTo.Server)]
        public void InsertTurnCopyRpc()
        {
            if (!_gameStarted)
                return;
            
            roundQueue.Enqueue(_currentTurnInfo.Value);
        }

        [Rpc(SendTo.SpecifiedInParams)]
        private void RequestTurnRpc(ulong target, RpcParams _ = default)
        {
            PlayerHandNetworkBehaviour targetHand = PlayerFactory.Get<PlayerHandNetworkBehaviour>(target);
            PlayerNetworkTransform localPlayerNetwork = PlayerFactory.GetLocal<PlayerNetworkTransform>();

            targetHand.AllowPickRpc();
            PlayerFactory.Get<PlayerNetworkTransform>(target).LookAtRpc(localPlayerNetwork.transform.position);
            localPlayerNetwork.LookAtRpc(targetHand.transform.position);
            
            LocalPlayerTurnStarted?.Invoke();
        }

        private static void ResetOthers(ulong activeId, ulong passiveId)
        {
            foreach (NetworkClient other in PlayerFactory.Players.Where(p => p.ClientId != activeId && p.ClientId != passiveId))
                other.PlayerObject.GetComponent<PlayerNetworkTransform>().ResetRotationRpc();
        }
    }
}
