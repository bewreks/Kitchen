using Zenject;
namespace Kitchen.Scripts.Player.Pools
{
    public class PlayersPoolContainer
    {
        [Inject] private PlayerModelPool _playerModelPool;
        [Inject] private PlayerPresenterPool _playerPresenterPool;

        [Inject]
        private void Construct()
        {
            _playerModelPool.Initialize(this);
            _playerPresenterPool.Initialize(this);
        }
    }


}
