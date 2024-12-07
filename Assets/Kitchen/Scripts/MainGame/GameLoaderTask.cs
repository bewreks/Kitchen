using System.Threading;
using Cysharp.Threading.Tasks;
using Kitchen.Scripts.Preloader;
using Zenject;

namespace Kitchen.Scripts.MainGame
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
