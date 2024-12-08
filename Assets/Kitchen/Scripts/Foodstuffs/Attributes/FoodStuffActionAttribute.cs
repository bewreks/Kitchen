using System;

namespace Kitchen.Scripts.Foodstuffs.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class FoodStuffActionAttribute : Attribute
    {
        public int ActionId { get; }
        
        public FoodStuffActionAttribute(int actionId) => ActionId = actionId;
    }
}
