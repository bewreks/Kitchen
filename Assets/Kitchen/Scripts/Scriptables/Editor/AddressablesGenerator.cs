using System.Linq;
using Kitchen.Scripts.CodeGenerator;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Kitchen.Scripts.Scriptables.Editor
{
    public class AddressablesGenerator : MonoBehaviour
    {

        [MenuItem("Assets/Generate Addressables")]
        public static void GenerateAddressables()
        {
            AssetDatabase.Refresh();
                
            var assets = AssetDatabase.FindAssets($"t:{typeof(AddressableAssetGroup)}");
            foreach (var asset in assets)
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                var group = AssetDatabase.LoadAssetAtPath<AddressableAssetGroup>(path);
                GenerateCodeForGroup(group);
            }
        }
        
        private static void GenerateCodeForGroup(AddressableAssetGroup group)
        {
            var node = new ClassNode
            {
                Name = group.name,
                Namespace = "Kitchen.Scripts.Generated",
                Path = "/Kitchen/Scripts/Generated/",
                Properties = group.entries.Select(entry => new PropertyNode { Name = entry.address.Split('/').Last(), Value = entry.address }).ToArray()
            };

            SimpleCodeGenerator.Generate(node);
        }
    }
}
