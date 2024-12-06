using Zenject;
namespace Kitchen.Scripts.Player.States
{
    public class PlayerMovementStateFactory
    {
        [Inject] private PlayerMovingState.Factory _playerMovingStateFactory;
        [Inject] private PlayerIdleState.Factory _playerIdleStateFactory;
        
        public PlayerMovingState CreateMovingState() => _playerMovingStateFactory.Create();
        public PlayerIdleState CreateIdleState() => _playerIdleStateFactory.Create();
    }
}
