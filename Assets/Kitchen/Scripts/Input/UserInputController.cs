﻿using System;
using Kitchen.Scripts.Preloader;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Kitchen.Scripts.Input
{
    public class UserInputController : IDisposable
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private DisposableManager _disposableManager;
        
        private InputActions _inputActions;
        
        [Inject]
        public void Construct()
        {
            _inputActions = new InputActions();
            
            _disposableManager.Add(this);
            _disposableManager.Add(_inputActions);
            
            _signalBus.DeclareSignal<MovementSignal>();
            _signalBus.DeclareSignal<CancelMovementSignal>();
            _signalBus.DeclareSignal<InteractSignal>();
            _signalBus.DeclareSignal<InteractCanceledSignal>();
            _signalBus.DeclareSignal<ExitToLobbySignal>();
            
            _signalBus.Subscribe<LoadingCompleteSignal>(OnLoadingComplete);
            
            _inputActions.Player.Move.performed += MoveOnPerformed;
            _inputActions.Player.Move.canceled += MoveOnCanceled;
            _inputActions.Player.Interact.performed += OnInteract;
            _inputActions.Player.Interact.canceled += OnInteractCanceled;
            _inputActions.Player.Exit.performed += OnExit;
        }
        
        private void OnExit(InputAction.CallbackContext obj)
        {
            _signalBus.Fire(new ExitToLobbySignal());
        }
        
        private void OnLoadingComplete()
        {
            _inputActions.Player.Enable();
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
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<LoadingCompleteSignal>(OnLoadingComplete);
        }
    }
}