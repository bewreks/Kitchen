using Kitchen.Scripts.Input;
using Kitchen.Scripts.Scriptables;
using UnityEngine;
using Zenject;

namespace Kitchen.Scripts.Player
{
    public class PlayerMovementController : ITickable
    {
        [Inject] private PlayerView _player;
        [Inject] private SettingsScriptableObject _settings;
        [Inject] private SignalBus _signalBus;
        [Inject] private TickableManager _tickableManager;
        
        private Vector2 _rawDirection;

        [Inject]
        private void Construct()
        {
            _tickableManager.Add(this);
            
            _signalBus.Subscribe<MovementSignal>(Callback);
        }

        private void Callback(MovementSignal signal)
        {
            _rawDirection = signal.Direction;
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
                _player.transform.position += direction * (Time.deltaTime * _settings.PlayerRotationSpeed);
                _player.transform.forward = Vector3.Slerp(_player.transform.forward, direction, Time.deltaTime * _settings.PlayerRotationSpeed);
            }
        }
        
        private bool CanMove(Vector3 direction)
        {
            var startPosition = _player.transform.position;
            return !Physics.CapsuleCast(startPosition, 
                startPosition + new Vector3(0, 2, 0), 
                0.5f, direction, Time.deltaTime * _settings.PlayerRotationSpeed);
        }
    }
}