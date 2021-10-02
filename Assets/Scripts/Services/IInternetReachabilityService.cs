using UniRx;

namespace Services
{
    public interface IInternetReachabilityService
    {
        IReadOnlyReactiveProperty<bool> IsInternetReachable { get; }

        void ReachabilityRequest();
        bool TryCloseInternetAlert();
    }
}