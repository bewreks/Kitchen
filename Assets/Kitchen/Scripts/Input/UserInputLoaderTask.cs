using System.Threading;
using Cysharp.Threading.Tasks;
using Kitchen.Scripts.Preloader;
using Zenject;

namespace Kitchen.Scripts.Input
{
    public class UserInputLoaderTask : ILoaderTask
    {
        public UniTask Load(DiContainer container, CancellationToken ctsToken)
        {
            var movementController = new UserInputController();
            container.Inject(movementController);
            container.BindInstance(movementController).AsSingle();
            
            return UniTask.CompletedTask;
        }
    }
}