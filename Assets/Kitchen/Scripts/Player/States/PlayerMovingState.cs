using Kitchen.Scripts.Scriptables;
using UnityEngine;
using Zenject;
namespace Kitchen.Scripts.Player.States
{
    public struct PlayerMovingState : IApplyDirection
    {
        [Inject] private PlayerView _player;
        [Inject] private SettingsScriptableObject _settings;
        
        public class Factory : PlaceholderFactory<PlayerMovingState>
        {
        }

        public void ApplyDirection(Vector2 rawDirection)
        {
            var direction = new Vector3(rawDirection.x, 0, rawDirection.y);

            var canMove = CanMove(direction);

            if (!canMove)
            {
                direction = new Vector3(rawDirection.x, 0, 0);
                canMove = CanMove(direction);

                if (!canMove)
                {
                    direction = new Vector3(0, 0, rawDirection.y);
                    canMove = CanMove(direction);
                }
            }

            if (canMove)
            {
                _player.transform.position += direction * (Time.deltaTime * _settings.PlayerRotationSpeed);
                _player.transform.forward = Vector3.Slerp(_player.transform.forward, direction, Time.deltaTime * _settings.PlayerRotationSpeed);
            }
        }
        
        private bool CanMove(Vector3 direction)
        {
            var startPosition = _player.transform.position;
            return !Physics.CapsuleCast(startPosition, 
                startPosition + new Vector3(0, 2, 0), 
                0.5f, direction, Time.deltaTime * _settings.PlayerRotationSpeed);
        }
    }

}
