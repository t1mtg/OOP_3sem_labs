using System.Collections.Generic;

namespace Banks
{
    public class InterestRates
    {
        public InterestRates()
        {
            InterestRate = new List<(int Limit, decimal InterestRateValue)>();
        }

        public List<(int Limit, decimal InterestRateValue)> InterestRate { get; }

        public void AddNewInterestRate(int limit, decimal interestRate)
        {
            InterestRate.Add((limit, interestRate));
        }
    }
}