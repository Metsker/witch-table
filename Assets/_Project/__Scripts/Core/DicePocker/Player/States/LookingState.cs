using System;
using _Project.__Scripts.Core.WitchCard.Entities.Players.Enums;
using Unity.Cinemachine;
using UnityEngine;
using UnityUtils;
using VContainer;

namespace _Project.__Scripts.Core.DicePocker.Player
{
    [Serializable]
    public class LookingState : PlayerState
    {
        private (CursorLockMode lockMode, bool visible) prevState;
        private Transform target;
        private VCamMediator _vCamMediator;

        [Inject]
        private void Construct(VCamMediator vCamMediator)
        {
            _vCamMediator = vCamMediator;
        }
        
        public override void OnEnterState()
        {
            _vCamMediator.SelectMovableCamera(true);
            
            /*prevState = (Cursor.lockState, Cursor.visible);
            
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;*/
        }

        public override void OnExitState()
        {
            /*Cursor.lockState = prevState.lockMode;
            Cursor.visible = prevState.visible;*/
        }
    }
}
