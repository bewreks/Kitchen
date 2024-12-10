using System;
using System.Collections.Generic;
using System.Linq;
using Kitchen.Scripts.CodeGenerator;
using Kitchen.Scripts.Foodstuffs.Attributes;
using UnityEditor;
using UnityEngine;
namespace Kitchen.Scripts.Foodstuffs.Editor
{
    public partial class AdditionalCodeGenerator : MonoBehaviour
    {
        
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
            
            SimpleCodeGenerator.Generate(in node);
        }

        [MenuItem("Assets/Generate/Foodstuffs")]
        public static void GenerateFoodstuffs()
        {
            AssetDatabase.Refresh();

            var foodstuffs = AssetDatabase.FindAssets($"t:{typeof(FoodstuffModel)}")
                .Select(AssetDatabase.GUIDToAssetPath);
            GenerateCodeForFoodstuffs(foodstuffs);
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

            SimpleCodeGenerator.Generate(in node);
        }
    }
}
