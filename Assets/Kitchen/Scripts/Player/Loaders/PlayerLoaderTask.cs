using System.Threading;
using Cysharp.Threading.Tasks;
using Kitchen.Scripts.Generated;
using Kitchen.Scripts.Loading;
using Kitchen.Scripts.Player.States;
using Kitchen.Scripts.Player.Views;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
namespace Kitchen.Scripts.Player.Loaders
{
    public class PlayerLoaderTask : ILoaderTask
    {
        public async UniTask Load(DiContainer container, CancellationToken token)
        {
            var handle = LoadingManager.Load(token, Prefabs.PlayerPrefab);

            await handle;

            if (token.IsCancellationRequested)
            {
                return;
            }

            var playerObject = await LoadingManager.Instantiate(Prefabs.PlayerPrefab, token);
            
            if (token.IsCancellationRequested)
            {
                LoadingManager.Release(Prefabs.PlayerPrefab, playerObject);
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