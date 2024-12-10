using System.Threading;
using Cysharp.Threading.Tasks;
using Kitchen.Scripts.Input;
using Kitchen.Scripts.KitchenGame;
using Kitchen.Scripts.Loading;
using Kitchen.Scripts.Messages;
using Kitchen.Scripts.Player;
using Kitchen.Scripts.Player.Loaders;
using Kitchen.Scripts.Scriptables;
using MessagePipe;
using Zenject;

namespace Kitchen.Scripts.Preloader
{
    public class GameScenePreloader : IInitializable
    {
        [Inject] private DiContainer _container;
        [Inject] private InitializableManager _initializableManager;
        
        [Inject] private LazyInject<IPublisher<LoadingCompleteSignal>> _loadingCompletePublisher;
        
        [Inject] private CancellationTokenSource _cts;
        
        private readonly ILoaderTask[][] loaderTasks = new ILoaderTask[][]
        {
            new ILoaderTask[] { new MessagesLoadingTask() },
            new ILoaderTask[] { new ScriptablesLoaderTask() },
            new ILoaderTask[] { new PlayerLoaderTask(), new UserInputLoaderTask() },
            new ILoaderTask[] { new GameLoaderTask() }
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
                await UniTask.WhenAll(parallelLoaderTask.Select(task => task.Load(_container, _cts.Token)));
                
                if (_cts.Token.IsCancellationRequested) return;
            }
            
            _loadingCompletePublisher.Value.Publish(new LoadingCompleteSignal());
        }
    }
}