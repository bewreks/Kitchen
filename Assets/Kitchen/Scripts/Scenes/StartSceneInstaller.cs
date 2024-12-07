using UnityEngine.SceneManagement;
using Zenject;

namespace Kitchen.Scripts.Scenes
{
    public class StartSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            if (SceneManager.GetActiveScene().name != Generated.Scenes.LobbyScene)
            {
                SceneManager.LoadScene(Generated.Scenes.LobbyScene, LoadSceneMode.Single);
            }
        }
    }
}
