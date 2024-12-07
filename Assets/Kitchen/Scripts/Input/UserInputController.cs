using System;
using Kitchen.Scripts.Preloader;
using MessagePipe;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Kitchen.Scripts.Input
{
    public class UserInputController : IDisposable
    {
        [Inject] private DiContainer _container;
        [Inject] private DisposableManager _disposableManager;
        
        [Inject] private ISubscriber<LoadingCompleteSignal> _cancelMovementSubscriber;
        [Inject] private IPublisher<MovementSignal> _startPublisher;
        [Inject] private IPublisher<CancelMovementSignal> _cancelPublisher;
        [Inject] private IPublisher<InteractSignal> _interactPublisher;
        [Inject] private IPublisher<InteractCanceledSignal> _interactCanceledPublisher;
        [Inject] private IPublisher<ExitToLobbySignal> _exitToLobbyPublisher;
        
        [Inject] private ISubscriber<LoadingCompleteSignal> _loadingCompleteSubscriber;
        
        private InputActions _inputActions;
        
        [Inject]
        public void Construct()
        {
            _inputActions = new InputActions();
            
            _disposableManager.Add(this);
            _disposableManager.Add(_inputActions);
            
            _disposableManager.Add(_loadingCompleteSubscriber.Subscribe(OnLoadingComplete));

            _inputActions.Player.Move.performed += MoveOnPerformed;
            _inputActions.Player.Move.canceled += MoveOnCanceled;
            _inputActions.Player.Interact.performed += OnInteract;
            _inputActions.Player.Interact.canceled += OnInteractCanceled;
            _inputActions.Player.Exit.performed += OnExit;
        }
        
        private void OnExit(InputAction.CallbackContext obj)
        {
            _exitToLobbyPublisher.Publish(new ExitToLobbySignal());
        }
        
        private void OnLoadingComplete(LoadingCompleteSignal args)
        {
            _inputActions.Player.Enable();
        }

        private void OnInteractCanceled(InputAction.CallbackContext obj)
        {
            _interactCanceledPublisher.Publish(new InteractCanceledSignal());
        }

        private void OnInteract(InputAction.CallbackContext obj)
        {
            _interactPublisher.Publish(new InteractSignal());
        }

        private void MoveOnCanceled(InputAction.CallbackContext obj)
        {
            _cancelPublisher.Publish(new CancelMovementSignal());
        }

        private void MoveOnPerformed(InputAction.CallbackContext obj)
        {
            _startPublisher.Publish(new MovementSignal
            {
                Direction = obj.ReadValue<Vector2>()
            });
        }
        
        public void Dispose()
        {
            _inputActions.Player.Move.performed -= MoveOnPerformed;
            _inputActions.Player.Move.canceled -= MoveOnCanceled;
            _inputActions.Player.Interact.performed -= OnInteract;
            _inputActions.Player.Interact.canceled -= OnInteractCanceled;
            _inputActions.Player.Exit.performed -= OnExit;
        }
    }
}