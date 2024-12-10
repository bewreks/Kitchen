using System;
using Kitchen.Scripts.Input;
using Kitchen.Scripts.Player.States;
using Kitchen.Scripts.Player.Views;
using Kitchen.Scripts.Scriptables;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace Kitchen.Scripts.Player
{
    public class PlayerMovementController : ITickable, IDisposable
    {
        [Inject] private PlayerView _player;
        [Inject] private SettingsScriptableObject _settings;
        [Inject] private ISubscriber<MovementSignal> _movementSubscriber;
        [Inject] private ISubscriber<CancelMovementSignal> _cancelMovementSubscriber;
        [Inject] private PlayerMovementStateFactory _playerMovementStateFactory;
        
        [Inject] private TickableManager _tickableManager;
        [Inject] private DisposableManager _disposableManager;
        
        private Vector2 _rawDirection;
        
        private IApplyDirection _movementState;

        [Inject]
        private void Construct()
        {
            _movementState = _playerMovementStateFactory.CreateIdleState();
            _tickableManager.Add(this);
            _disposableManager.Add(this);

            _disposableManager.Add(_movementSubscriber.Subscribe(OnMoving));
            _disposableManager.Add(_cancelMovementSubscriber.Subscribe(OnIdle));
        }
        
        private void OnIdle(CancelMovementSignal args)
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
        
        public void Dispose()
        {
            
        }
    }
}