using System.Threading;
using Kitchen.Scripts.Preloader;
using MessagePipe;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Kitchen.Scripts
{
    public class Bootstrapper : MonoInstaller<Bootstrapper>
    {
        [SerializeField] private CinemachineCamera _mainCamera;
        
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public override void InstallBindings()
        {
            Container.Settings = new ZenjectSettings(ValidationErrorResponses.Throw, RootResolveMethods.NonLazyOnly, false);

            Container.BindMessagePipe();
            
            Container.BindInstance(_mainCamera).AsCached();
            Container.BindInstance(_cts).AsCached();
            
            var preloader = new GameScenePreloader();
            Container.Inject(preloader);
            Container.BindInstance(preloader).AsSingle();
        }

        private void OnDestroy()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}