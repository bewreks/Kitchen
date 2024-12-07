using System;
using Kitchen.Scripts.Input;
using UnityEngine.SceneManagement;
using Zenject;

namespace Kitchen.Scripts.MainGame
{
    public class GameManager : IDisposable
    {
        [Inject] private SignalBus _signalBus;
        
        [Inject]
        private void Construct()
        {
            _signalBus.Subscribe<ExitToLobbySignal>(OnExitToLobby);
        }
        
        private void OnExitToLobby()
        {
            SceneManager.LoadScene(Generated.Scenes.LobbyScene, LoadSceneMode.Single);
        }
        
        public void Dispose()
        {
            _signalBus.Unsubscribe<ExitToLobbySignal>(OnExitToLobby);
        }
    }
}
