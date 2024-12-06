using Cysharp.Threading.Tasks;
using Kitchen.Scripts.Preloader;
using Kitchen.Scripts.Scriptables;
using Unity.Cinemachine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Kitchen.Scripts.Player
{
    public class PlayerLoaderTask : ILoaderTask
    {
        public async UniTask Load(DiContainer container)
        {
            var content = container.Resolve<ContentScriptableObject>();
            var playerObject = await Addressables.InstantiateAsync(content.PlayerPrefab).Task;
            var player = playerObject.GetComponent<PlayerView>();
            var camera = container.Resolve<CinemachineCamera>();
            camera.Follow = player.transform;
            container.Bind<PlayerView>().FromInstance(player).AsSingle();
        }
    }
}