using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Kitchen.Scripts.Input
{
    public class UserInputController
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private InputActions _inputActions;
        
        [Inject]
        public void Construct()
        {
            _signalBus.DeclareSignal<MovementSignal>();
            _signalBus.DeclareSignal<CancelMovementSignal>();
            _signalBus.DeclareSignal<InteractSignal>();
            _signalBus.DeclareSignal<InteractCanceledSignal>();
            
            _inputActions.Player.Move.performed += MoveOnPerformed;
            _inputActions.Player.Move.canceled += MoveOnCanceled;
            _inputActions.Player.Interact.performed += OnInteract;
            _inputActions.Player.Interact.canceled += OnInteractCanceled;
        }

        private void OnInteractCanceled(InputAction.CallbackContext obj)
        {
            _signalBus.Fire(new InteractCanceledSignal());
        }

        private void OnInteract(InputAction.CallbackContext obj)
        {
            _signalBus.Fire(new InteractSignal());
        }

        private void MoveOnCanceled(InputAction.CallbackContext obj)
        {
            _signalBus.Fire<CancelMovementSignal>();
        }

        private void MoveOnPerformed(InputAction.CallbackContext obj)
        {
            _signalBus.Fire(new MovementSignal
            {
                Direction = obj.ReadValue<Vector2>()
            });
        }
    }
}