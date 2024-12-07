using System.Threading;
using Cysharp.Threading.Tasks;
using Kitchen.Scripts.Input;
using Kitchen.Scripts.MainGame;
using Kitchen.Scripts.Player;
using Kitchen.Scripts.Scriptables;
using Zenject;

namespace Kitchen.Scripts.Preloader
{
    public class GameScenePreloader : IInitializable
    {
        [Inject] private DiContainer _container;
        [Inject] private SignalBus _signalBus;
        [Inject] private InitializableManager _initializableManager;
        
        [Inject] private CancellationTokenSource _cts;
        
        private readonly ILoaderTask[][] loaderTasks = new ILoaderTask[][]
        {
            new ILoaderTask[] { new ScriptablesLoaderTask() },
            new ILoaderTask[] { new PlayerLoaderTask(), new UserInputLoaderTask() },
            new ILoaderTask[] { new GameLoaderTask() }
        };

        [Inject]
        private void Construct()
        {
            _signalBus.DeclareSignal<LoadingCompleteSignal>();
            
            _initializableManager.Add(this);
        }

        public async void Initialize()
        {
            foreach (var parallelLoaderTask in loaderTasks)
            {
                await UniTask.WhenAll(parallelLoaderTask.Select(task => task.Load(_container, _cts.Token)));
                
                if (_cts.Token.IsCancellationRequested) return;
            }
            
            _signalBus.Fire<LoadingCompleteSignal>();
        }
    }
}