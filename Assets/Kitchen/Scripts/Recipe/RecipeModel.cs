using UnityEngine;
namespace Kitchen.Scripts.Recipe
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "Game/Recipe")]
    public class RecipeModel : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
        
    }
}
