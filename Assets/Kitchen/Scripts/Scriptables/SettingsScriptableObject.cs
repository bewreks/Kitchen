using UnityEngine;

namespace Kitchen.Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "SettingsInstaller", menuName = "Installers/SettingsInstaller")]
    public class SettingsScriptableObject : ScriptableObject
    {
        [field: SerializeField] public float PlayerMovementSpeed { get; private set; }
        [field: SerializeField] public float PlayerRotationSpeed { get; private set; }
    }
}