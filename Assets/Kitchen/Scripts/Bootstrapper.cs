using Kitchen.Scripts.Preloader;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Kitchen.Scripts
{
    public class Bootstrapper : MonoInstaller<Bootstrapper>
    {
        [SerializeField] private CinemachineCamera _mainCamera;

        public override void InstallBindings()
        {
            Container.Settings = new ZenjectSettings(ValidationErrorResponses.Throw, RootResolveMethods.NonLazyOnly, false);
            SignalBusInstaller.Install(Container);
            Container.Bind<CinemachineCamera>().FromInstance(_mainCamera);
            var preloader = new GameScenePreloader();
            Container.Inject(preloader);
            Container.Bind<GameScenePreloader>().FromInstance(preloader).AsSingle();
        }
    }
}