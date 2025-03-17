using System.Threading.Tasks;
using Messenger;
using UnityEngine;

namespace UI
{
    public interface IMediatorFactory
    {
        Task<TMediator> Get<TMediator>() where TMediator : MonoBehaviour, IMediator;
        Task Show<TMediator>(bool suppressAllWindows = false) where TMediator : MonoBehaviour, IMediator;
        void Init(Transform canvas, IMessenger messenger );
    }
}