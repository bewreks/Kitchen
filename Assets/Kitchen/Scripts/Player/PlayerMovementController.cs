using Kitchen.Scripts.Input;
using Kitchen.Scripts.Player.States;
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
        [Inject] private PlayerMovementStateFactory _playerMovementStateFactory;
        
        private Vector2 _rawDirection;
        
        private IApplyDirection _movementState;

        [Inject]
        private void Construct()
        {
            _movementState = _playerMovementStateFactory.CreateIdleState();
            _tickableManager.Add(this);
            
            _signalBus.Subscribe<MovementSignal>(OnMoving);
            _signalBus.Subscribe<CancelMovementSignal>(OnIdle);
        }
        
        private void OnIdle()
        {
            _movementState = _playerMovementStateFactory.CreateIdleState();
        }

        private void OnMoving(MovementSignal signal)
        {
            _rawDirection = signal.Direction;
            _movementState = _playerMovementStateFactory.CreateMovingState();
        }

        public void Tick()
        {
            _movementState.ApplyDirection(_rawDirection);
        }
    }
}