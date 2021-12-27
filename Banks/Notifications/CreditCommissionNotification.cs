namespace Banks.Notifications
{
    public class CreditCommissionNotification : INotification
    {
        public CreditCommissionNotification(decimal newValue)
        {
            NewValue = newValue;
        }

        private decimal NewValue { get; }
        public string GetMessage()
        {
            return $"Credit commission changed. New value: {NewValue}";
        }
    }
}