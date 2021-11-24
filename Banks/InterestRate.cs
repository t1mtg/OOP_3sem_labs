namespace Banks
{
    public class InterestRate
    {
        public InterestRate(int limit, decimal interestRateValue)
        {
            Limit = limit;
            InterestRateValue = interestRateValue;
        }

        public int Limit { get; set; }
        public decimal InterestRateValue { get; set; }
    }
}