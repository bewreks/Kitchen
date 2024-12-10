using System.Threading;
using Cysharp.Threading.Tasks;
using Kitchen.Scripts.Generated;
using Kitchen.Scripts.Loading;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Kitchen.Scripts.Scriptables
{
    public class ScriptablesLoaderTask : ILoaderTask
    {

        public async UniTask Load(DiContainer container, CancellationToken ctsToken)
        {
            var handler = Addressables.LoadAssetAsync<SettingsScriptableObject>(Settings.SettingsInstallerAsset);
            var settings = await handler;
            container.BindInstance(settings).AsSingle();
            
            Addressables.Release(handler);
        }
    }
}
