using Cysharp.Threading.Tasks;
using Kitchen.Scripts.Input;
using Kitchen.Scripts.Player;
using Zenject;

namespace Kitchen.Scripts.Preloader
{
    public class GameScenePreloader : IInitializable
    {
        [Inject] private DiContainer _container;
        [Inject] private InitializableManager _initializableManager;
        
        private readonly ILoaderTask[][] loaderTasks = new ILoaderTask[][]
        {
            new ILoaderTask[] { new PlayerLoaderTask(), new UserInputLoaderTask() },
        };

        [Inject]
        private void Construct()
        {
            _initializableManager.Add(this);
        }

        public async void Initialize()
        {
            foreach (var parallelLoaderTask in loaderTasks)
            {
                await UniTask.WhenAll(parallelLoaderTask.Select(task => task.Load(_container)));
            }
        }
    }
}