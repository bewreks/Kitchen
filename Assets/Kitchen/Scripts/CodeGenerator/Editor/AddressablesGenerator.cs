using System.Collections.Generic;
using System.Linq;
using Kitchen.Scripts.CodeGenerator;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kitchen.Scripts.Scriptables.Editor
{
    public class AddressablesGenerator : MonoBehaviour
    {

        [MenuItem("Assets/Generate/Addressables")]
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

        [MenuItem("Assets/Generate/Scenes")]
        public static void GenerateScenes()
        {
            AssetDatabase.Refresh();

            var scenes = AssetDatabase.FindAssets($"t:Scene")
                                            .Select(AssetDatabase.GUIDToAssetPath)
                                            .Where(s => s.StartsWith("Assets/Kitchen/")).ToArray();
            GenerateCodeForScenes(scenes);
        }
        private static void GenerateCodeForScenes(IEnumerable<string> scenes)
        {
            var node = new ClassNode
            {
                Name = "Scenes",
                Namespace = "Kitchen.Scripts.Generated",
                Path = "/Kitchen/Scripts/Generated/",
                Properties = scenes.Select(scene =>
                {
                    var sceneName = scene.Split('/').Last().Split('.').First();
                    return new PropertyNode
                    {
                        Name = sceneName,
                        Value = sceneName
                    };
                }).ToArray()
            };
            
            SimpleCodeGenerator.Generate(node);
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
