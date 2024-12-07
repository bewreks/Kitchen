using UnityEngine;
using Zenject;

namespace Kitchen.Scripts.Player.States
{
    public struct PlayerIdleState : IApplyDirection
    {
        public class Factory : PlaceholderFactory<PlayerIdleState>
        {
        }
        public void ApplyDirection(Vector2 rawDirection)
        {
        }
    }
}
