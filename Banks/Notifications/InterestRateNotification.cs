namespace Banks
{
    public class InterestRateNotification : INotification
    {
        public InterestRateNotification(InterestRates newValue)
        {
            NewValue = newValue;
        }

        private InterestRates NewValue { get; }
        public string GetMessage()
        {
            return $"Interest rates changed. New value: {NewValue}";
        }
    }
}