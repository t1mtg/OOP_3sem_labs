namespace Banks
{
    public class UnverifiedLimitNotification : INotification
    {
        public UnverifiedLimitNotification(int newValue)
        {
            NewValue = newValue;
        }

        private int NewValue { get; }
        public string GetMessage()
        {
            return $"Unverified limit changed. New value: {NewValue}";
        }
    }
}