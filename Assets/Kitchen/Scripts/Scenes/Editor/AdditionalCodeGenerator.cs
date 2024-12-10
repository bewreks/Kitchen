using System.Collections.Generic;
using System.Linq;
using Kitchen.Scripts.CodeGenerator;
using UnityEditor;
using UnityEngine;
namespace Kitchen.Scripts.Scenes.Editor
{
    public partial class AdditionalCodeGenerator : MonoBehaviour
    {
        [MenuItem("Assets/Generate/Scenes")]
        public static void GenerateScenes()
        {
            AssetDatabase.Refresh();

            var scenes = AssetDatabase.FindAssets($"t:Scene")
                                            .Select(AssetDatabase.GUIDToAssetPath)
                                            .Where(s => s.StartsWith("Assets/Kitchen/"));
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
                    return new PropertyNode<string>
                    {
                        Name = sceneName,
                        Value = sceneName
                    };
                }).ToArray()
            };
            
            SimpleCodeGenerator.Generate(node);
        }
    }
}
