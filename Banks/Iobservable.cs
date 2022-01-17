namespace Banks
{
    public interface IObservable
    {
        void AddObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void NotifySubscribers(INotification notification);
    }
}