using Kitchen.Scripts.Generated;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Kitchen.Scripts.Lobby
{
    public class LobbySceneInstaller : MonoInstaller
    {
        [field: SerializeField] public Button PlayButton { get; private set; }
        [field: SerializeField] public Button QuitButton { get; private set; }
        
        public override void InstallBindings()
        {
            QuitButton.onClick.AddListener(Application.Quit);
            PlayButton.onClick.AddListener(() => SceneManager.LoadScene(Scenes.GameScene));
        }
    }
}
