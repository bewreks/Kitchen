using UnityEngine;
namespace Kitchen.Scripts.Player.States
{
    public interface IApplyDirection
    {
        void ApplyDirection(Vector2 rawDirection);
    }
}
