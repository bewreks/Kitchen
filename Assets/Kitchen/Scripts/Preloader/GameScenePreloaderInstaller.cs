using Cysharp.Threading.Tasks;
using Kitchen.Scripts.Input;
using Kitchen.Scripts.Player;
using Zenject;

namespace Kitchen.Scripts.Preloader
{
    public class GameScenePreloader : IInitializable
    {
        [Inject] private DiContainer resolver;
        [Inject] private InitializableManager initializableManager;
        
        private readonly ILoaderTask[][] loaderTasks = new ILoaderTask[][]
        {
            new ILoaderTask[] { new PlayerLoaderTask() },
            new ILoaderTask[] { new UserInputLoaderTask() },
        };

        [Inject]
        private void Construct()
        {
            initializableManager.Add(this);
        }

        public async void Initialize()
        {
            foreach (var parallelLoaderTask in loaderTasks)
            {
                await UniTask.WhenAll(parallelLoaderTask.Select(task => task.Load(resolver)));
            }
        }
    }
}