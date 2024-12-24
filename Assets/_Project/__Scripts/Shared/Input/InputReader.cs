using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.__Scripts.Shared.Input
{
    [CreateAssetMenu(menuName = "Input Reader")]
    public class InputReader : ScriptableObject, InputSystem_Actions.IPlayerActions
    {
        public Vector3 Pointer => inputActions.Player.PointerPosition.ReadValue<Vector2>();
        public bool IsPressed => inputActions.Player.Press.IsPressed();

        private InputSystem_Actions inputActions;

        public event Action<InputAction.CallbackContext> Pressed;

        private void OnEnable()
        {
            if (inputActions != null)
                return;
            
            inputActions = new InputSystem_Actions();
            inputActions.Player.SetCallbacks(this);
            inputActions.Player.Enable();
        }

        private void OnDisable()
        {
            inputActions.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
        }

        public void OnLook(InputAction.CallbackContext context)
        {
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
        }

        public void OnNext(InputAction.CallbackContext context)
        {
        }

        public void OnPress(InputAction.CallbackContext context)
        {
            Pressed?.Invoke(context);
        }

        public void OnPointerPosition(InputAction.CallbackContext context)
        {
        }
    }
}
