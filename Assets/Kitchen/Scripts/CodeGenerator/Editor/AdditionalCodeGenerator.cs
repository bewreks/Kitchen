using System;
using System.Collections.Generic;
using System.Linq;
using Kitchen.Scripts.Foodstuffs;
using Kitchen.Scripts.Foodstuffs.Actions;
using Kitchen.Scripts.Foodstuffs.Attributes;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Kitchen.Scripts.CodeGenerator.Editor
{
    public class AdditionalCodeGenerator : MonoBehaviour
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
                                            .Where(s => s.StartsWith("Assets/Kitchen/"));
            GenerateCodeForScenes(scenes);
        }
        
        [MenuItem("Assets/Generate/Foodstuff actions")]
        public static void GenerateFoodstuffActions()
        {
            var type = typeof(FoodStuffActionAttribute);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Select(s => (s, s.GetCustomAttributes(type, true)))
                .Where(tuple => tuple.Item2.Any());
            var node = new EnumNode
            {
                Name = "FoodstuffActions",
                Namespace = "Kitchen.Scripts.Generated",
                Path = "/Kitchen/Scripts/Generated/",
                Properties = types.Select(t => new PropertyNode<int> { Name = t.s.Name, Value = ((FoodStuffActionAttribute)t.Item2[0]).ActionId }).ToArray()
            };
            
            SimpleCodeGenerator.Generate(node);
        }

        [MenuItem("Assets/Generate/Foodstuffs")]
        public static void GenerateFoodstuffs()
        {
            AssetDatabase.Refresh();

            var foodstuffs = AssetDatabase.FindAssets($"t:{typeof(FoodstuffModel)}")
                .Select(AssetDatabase.GUIDToAssetPath);
            GenerateCodeForFoodstuffs(foodstuffs);
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

        private static void GenerateCodeForFoodstuffs(IEnumerable<string> foodstuffs)
        {
            var node = new EnumNode
            {
                Name = "Foodstuffs",
                Namespace = "Kitchen.Scripts.Generated",
                Path = "/Kitchen/Scripts/Generated/",
                Properties = foodstuffs.Select(foodstuffPath =>
                {
                    var sceneName = foodstuffPath.Split('/').Last().Split('.').First();
                    var foodstuff = AssetDatabase.LoadAssetAtPath<FoodstuffModel>(foodstuffPath);
                    return new PropertyNode<int>
                    {
                        Name = sceneName,
                        Value = foodstuff.FoodstuffId
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
                Properties = group.entries.Select(entry => new PropertyNode<string> { Name = entry.address.Split('/').Last(), Value = entry.address }).ToArray()
            };

            SimpleCodeGenerator.Generate(node);
        }
    }
}
