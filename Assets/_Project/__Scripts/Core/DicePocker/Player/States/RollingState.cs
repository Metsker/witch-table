using System;
using _Project.__Scripts.Core.WitchCard.Entities.Players.Enums;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityUtils;
using VContainer;

namespace _Project.__Scripts.Core.DicePocker.Player
{
    public class RollingState : PlayerState
    {
        [SerializeField] private float speed = 2;

        private Rigidbody cup;
        private Camera _camera;
        private bool canRoll;
        private float _distance;

        public override bool CanExitState => !canRoll;

        private void Start()
        {
            _camera = Camera.main;
            _distance = Vector3.Distance(_camera.transform.position, cup.transform.position);
        }

        private void FixedUpdate()
        {
            if (canRoll && cup != null)
            {
                Vector3 mousePosition = InputReader.Pointer.With(z: _distance);
                cup.MovePosition(Vector3.MoveTowards(cup.position, _camera.ScreenToWorldPoint(mousePosition).Add(y: 0.15f), Time.fixedDeltaTime * speed));
            }
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            
            cup = GameObject.Find("Cup").GetComponent<Rigidbody>();
            
            VCamMediator.SelectLookAtCamera(cup.transform, true);
            
            canRoll = InputReader.IsPressed;
            
            InputReader.Pressed += OnPress;
        }

        public override void OnExitState()
        {
            base.OnExitState();
            
            InputReader.Pressed -= OnPress;
        }

        private void OnPress(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                canRoll = true;
            }
            else if (context.canceled)
            {
                print("CANCEL");
                canRoll = false;
                GameObject.Find("Cup").GetComponent<Collider>().enabled = false;
                GetComponent<NetworkPlayerStateMachine>().SetLooking();
            }
        }
    }
}
