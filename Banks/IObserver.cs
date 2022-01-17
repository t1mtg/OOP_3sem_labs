namespace Banks
{
    public interface IObserver
    {
        void Update(INotification notification);
    }
}