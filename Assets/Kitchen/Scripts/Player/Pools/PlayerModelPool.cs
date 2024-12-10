using Kitchen.Scripts.Player.Models;
using Zenject;
namespace Kitchen.Scripts.Player.Pools
{
    public class PlayerModelPool : MemoryPool<PlayerModel>
    {
        private PlayersPoolContainer _poolContainer;

        public void Initialize(PlayersPoolContainer playersPoolContainer)
        {
            _poolContainer = playersPoolContainer;
        }
    }
}
