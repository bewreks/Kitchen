using Kitchen.Scripts.Player;
using Kitchen.Scripts.Scriptables;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Kitchen.Scripts.Input
{
    public class UserMovementController : ITickable
    {
        [Inject] private PlayerView player;
        [Inject] private InputActions inputActions;
        [Inject] private SettingsScriptableObject settings;
        [Inject] private TickableManager tickableManager;
        [Inject] private CinemachineCamera camera;
        
        private Vector2 _rawDirection;
        
        [Inject]
        public void Construct()
        {
            tickableManager.Add(this);
            inputActions.Player.Move.performed += MoveOnPerformed;
            inputActions.Player.Move.canceled += MoveOnCanceled;
        }

        private void MoveOnCanceled(InputAction.CallbackContext obj)
        {
            _rawDirection = Vector2.zero;
        }

        private void MoveOnPerformed(InputAction.CallbackContext obj)
        {
            _rawDirection = obj.ReadValue<Vector2>();
        }

        public void Tick()
        {
            if (_rawDirection == Vector2.zero) return;

            var direction = new Vector3(_rawDirection.x, 0, _rawDirection.y);

            var canMove = CanMove(direction);

            if (!canMove)
            {
                direction = new Vector3(_rawDirection.x, 0, 0);
                canMove = CanMove(direction);

                if (!canMove)
                {
                    direction = new Vector3(0, 0, _rawDirection.y);
                    canMove = CanMove(direction);
                }
            }

            if (canMove)
            {
                player.transform.position += direction * (Time.deltaTime * settings.PlayerRotationSpeed);
                player.transform.forward = Vector3.Slerp(player.transform.forward, direction, Time.deltaTime * settings.PlayerRotationSpeed);
            }
        }

        private bool CanMove(Vector3 direction)
        {
            var startPosition = player.transform.position;
            return !Physics.CapsuleCast(startPosition, 
                startPosition + new Vector3(0, 2, 0), 
                0.5f, direction, Time.deltaTime * settings.PlayerRotationSpeed);
        }
    }
}