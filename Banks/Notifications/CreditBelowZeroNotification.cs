namespace Banks.Notifications
{
    public class CreditBelowZeroNotification : INotification
    {
        public CreditBelowZeroNotification(int newValue)
        {
            NewValue = newValue;
        }

        private int NewValue { get; }
        public string GetMessage()
        {
            return $"Below zero limit changed. New value: {NewValue}";
        }
    }
}