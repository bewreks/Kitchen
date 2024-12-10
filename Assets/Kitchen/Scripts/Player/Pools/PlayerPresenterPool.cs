using Kitchen.Scripts.Player.Presenters;
using Zenject;
namespace Kitchen.Scripts.Player.Pools
{
    public class PlayerPresenterPool : MemoryPool<PlayerPresenter>
    {
        private PlayersPoolContainer _poolContainer;
            
        public void Initialize(PlayersPoolContainer playersPoolContainer)
        {
            _poolContainer = playersPoolContainer;
        }
    }
}
