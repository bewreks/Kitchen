using System.Threading;
using Cysharp.Threading.Tasks;
using Kitchen.Scripts.Generated;
using Kitchen.Scripts.Player.States;
using Kitchen.Scripts.Player.Views;
using Kitchen.Scripts.Preloader;
using Unity.Cinemachine;
using UnityEngine.AddressableAssets;
using Zenject;
namespace Kitchen.Scripts.Player.Loaders
{
    public class PlayerLoaderTask : ILoaderTask
    {
        public async UniTask Load(DiContainer container, CancellationToken token)
        {
            var playerObject = await Addressables.InstantiateAsync(Prefabs.PlayerPrefab).Task;

            if (token.IsCancellationRequested)
            {
                Addressables.ReleaseInstance(playerObject);
                return;
            }
            
            var player = playerObject.GetComponent<PlayerView>();
            var camera = container.Resolve<CinemachineCamera>();
            camera.Follow = player.transform;
            container.BindInstance(player).AsSingle();

            container.Bind<PlayerMovementStateFactory>().AsSingle();

            container.BindFactory<PlayerIdleState, PlayerIdleState.Factory>().WhenInjectedInto<PlayerMovementStateFactory>();
            container.BindFactory<PlayerMovingState, PlayerMovingState.Factory>().WhenInjectedInto<PlayerMovementStateFactory>();
            
            container.Bind<PlayerMovementController>().FromNew().AsSingle();
            container.Resolve<PlayerMovementController>();
        }
    }
}