using System.Threading;
using Cysharp.Threading.Tasks;
using Kitchen.Scripts.Input;
using Kitchen.Scripts.Preloader;
using MessagePipe;
using Zenject;

namespace Kitchen.Scripts.Messages
{
    public class MessagesLoadingTask : ILoaderTask
    {
        public UniTask Load(DiContainer container, CancellationToken ctsToken)
        {
            var options = container.Resolve<MessagePipeOptions>();

            container.BindMessageBroker<MovementSignal>(options);
            container.BindMessageBroker<CancelMovementSignal>(options);
            container.BindMessageBroker<InteractSignal>(options);
            container.BindMessageBroker<InteractCanceledSignal>(options);
            container.BindMessageBroker<ExitToLobbySignal>(options);

            container.BindMessageBroker<LoadingCompleteSignal>(options);

            return UniTask.CompletedTask; 
        }
    }
}
