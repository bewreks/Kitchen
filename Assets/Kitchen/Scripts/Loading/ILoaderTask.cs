using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;
namespace Kitchen.Scripts.Loading
{
    public interface ILoaderTask
    {
        UniTask Load(DiContainer container, CancellationToken ctsToken);
    }
}