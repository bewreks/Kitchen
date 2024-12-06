using UnityEngine;

namespace Kitchen.Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "SettingsInstaller", menuName = "Installers/SettingsInstaller")]
    public class SettingsScriptableObject : ScriptableObject
    {
        [field: SerializeField] public float PlayerSpeed { get; private set; }
    }
}