using System;

namespace Arbitrage
{
    public class ExchangeEntry
    {
        public string FromCountry { get; private set; }

        public string ToCountry { get; private set; }

        public double Rate { get; private set; }

        public ExchangeEntry(string text)
        {
            var textArray = text.Split('\t', StringSplitOptions.RemoveEmptyEntries);
            if (textArray.Length < 3 || textArray.Length > 3)
                throw new Exception("Invalid Entry");

            FromCountry = textArray[0].Trim();
            ToCountry = textArray[1].Trim();

            double rate;
            if (!double.TryParse(textArray[2], out rate))
                throw new Exception("Invalid Exchange Rate");
            Rate = rate;
        }
    }
}
