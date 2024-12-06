using Kitchen.Scripts.Preloader;
using Kitchen.Scripts.Scriptables;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Kitchen.Scripts
{
    public class Bootstrapper : MonoInstaller<Bootstrapper>
    {
        [SerializeField] private ContentScriptableObject contentScriptableObject;
        [SerializeField] private SettingsScriptableObject settingsScriptableObject;
        [SerializeField] CinemachineCamera mainCamera;

        public override void InstallBindings()
        {
            Container.Settings = new ZenjectSettings(ValidationErrorResponses.Throw, RootResolveMethods.NonLazyOnly, false);
            Container.Bind<ContentScriptableObject>().FromInstance(contentScriptableObject);
            Container.Bind<SettingsScriptableObject>().FromInstance(settingsScriptableObject);
            Container.Bind<CinemachineCamera>().FromInstance(mainCamera);
            var preloader = new GameScenePreloader();
            Container.Inject(preloader);
            Container.Bind<GameScenePreloader>().FromInstance(preloader).AsSingle();
        }
    }
}