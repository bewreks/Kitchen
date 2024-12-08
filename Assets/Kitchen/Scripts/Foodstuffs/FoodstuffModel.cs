using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Kitchen.Scripts.Generated;
using UnityEngine;

namespace Kitchen.Scripts.Foodstuffs
{
    [CreateAssetMenu(fileName = "Foodstuff", menuName = "Game/Foodstuff")]
    public class FoodstuffModel : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
        public GameObject Prefab;
        [SerializedDictionary("Action", "Model after action")]
        public SerializedDictionary<FoodstuffActions, FoodstuffModel> Actions = new SerializedDictionary<FoodstuffActions, FoodstuffModel>();
    }
}
