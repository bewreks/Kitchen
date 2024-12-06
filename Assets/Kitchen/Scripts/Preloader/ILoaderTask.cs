using Cysharp.Threading.Tasks;
using Zenject;

namespace Kitchen.Scripts.Preloader
{
    public interface ILoaderTask
    {
        UniTask Load(DiContainer container);
    }
}