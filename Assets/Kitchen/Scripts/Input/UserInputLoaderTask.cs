using Cysharp.Threading.Tasks;
using Kitchen.Scripts.Preloader;
using Zenject;

namespace Kitchen.Scripts.Input
{
    public class UserInputLoaderTask : ILoaderTask
    {
        public async UniTask Load(DiContainer container)
        {
            var inputActions = new InputActions();
            inputActions.Player.Enable();
            container.BindInstance(inputActions);
            
            await UniTask.Delay(5); 
            
            var movementController = new UserInputController();
            container.Inject(movementController);
            container.BindInstance(movementController).AsSingle();
        }
    }
}