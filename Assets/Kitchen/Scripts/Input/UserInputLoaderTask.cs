using Cysharp.Threading.Tasks;
using Kitchen.Scripts.Preloader;
using Zenject;

namespace Kitchen.Scripts.Input
{
    public class UserInputLoaderTask : ILoaderTask
    {
        public UniTask Load(DiContainer container)
        {
            var inputActions = new InputActions();
            inputActions.Player.Enable();
            container.BindInstance(inputActions);
            
            var movementController = new UserMovementController();
            container.Inject(movementController);
            container.BindInstance(movementController).AsSingle();
            
            return UniTask.CompletedTask;
        }
    }
}