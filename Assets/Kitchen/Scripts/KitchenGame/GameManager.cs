using System;
using Kitchen.Scripts.Generated;
using Kitchen.Scripts.Messages;
using MessagePipe;
using UnityEngine.SceneManagement;
using Zenject;
namespace Kitchen.Scripts.KitchenGame
{
    public class GameManager : IDisposable
    {
        [Inject] private ISubscriber<ExitToLobbySignal> _cancelMovementSubscriber;
        [Inject] private DisposableManager _disposableManager;
        
        [Inject]
        private void Construct()
        {
            _disposableManager.Add(_cancelMovementSubscriber.Subscribe(OnExitToLobby));
        }
        
        private void OnExitToLobby(ExitToLobbySignal args)
        {
            SceneManager.LoadScene(Scenes.LobbyScene, LoadSceneMode.Single);
        }
        
        public void Dispose()
        {
        }
    }
}
