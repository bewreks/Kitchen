using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Kitchen.Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "ContentInstaller", menuName = "Installers/ContentInstaller")]
    public class ContentScriptableObject : ScriptableObject
    {
        [field: SerializeField] public AssetReference PlayerPrefab { get; private set; }
    }
}