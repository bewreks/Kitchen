using Kitchen.Scripts.Preloader;
using Kitchen.Scripts.Scriptables;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Kitchen.Scripts
{
    public class Bootstrapper : MonoInstaller<Bootstrapper>
    {
        [SerializeField] private ContentScriptableObject _contentScriptableObject;
        [SerializeField] private SettingsScriptableObject _settingsScriptableObject;
        [SerializeField] private CinemachineCamera _mainCamera;

        public override void InstallBindings()
        {
            Container.Settings = new ZenjectSettings(ValidationErrorResponses.Throw, RootResolveMethods.NonLazyOnly, false);
            SignalBusInstaller.Install(Container);
            Container.Bind<ContentScriptableObject>().FromInstance(_contentScriptableObject);
            Container.Bind<SettingsScriptableObject>().FromInstance(_settingsScriptableObject);
            Container.Bind<CinemachineCamera>().FromInstance(_mainCamera);
            var preloader = new GameScenePreloader();
            Container.Inject(preloader);
            Container.Bind<GameScenePreloader>().FromInstance(preloader).AsSingle();
        }
    }
}