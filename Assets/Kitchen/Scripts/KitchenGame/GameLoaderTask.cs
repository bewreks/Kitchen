using System.Threading;
using Cysharp.Threading.Tasks;
using Kitchen.Scripts.Loading;
using Zenject;
namespace Kitchen.Scripts.KitchenGame
{
    public class GameLoaderTask : ILoaderTask
    {

        public UniTask Load(DiContainer container, CancellationToken ctsToken)
        {
            container.Bind<GameManager>().FromNew().AsSingle();
            container.Resolve<GameManager>();
            return UniTask.CompletedTask;
        }
    }
}
