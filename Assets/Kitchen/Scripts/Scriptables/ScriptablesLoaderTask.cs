using Cysharp.Threading.Tasks;
using Kitchen.Scripts.Generated;
using Kitchen.Scripts.Preloader;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Kitchen.Scripts.Scriptables
{
    public class ScriptablesLoaderTask : ILoaderTask
    {

        public async UniTask Load(DiContainer container)
        {
            var settings = await Addressables.LoadAssetAsync<SettingsScriptableObject>(Settings.SettingsInstallerAsset);
            container.BindInstance(settings).AsSingle();
        }
    }
}
