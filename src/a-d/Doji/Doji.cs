namespace Skender.Stock.Indicators;

public static partial class Indicator
{
    // Doji
    /// <include file='./info.xml' path='indicator/*' />
    ///
    public static IEnumerable<CandleResult> GetDoji<TQuote>(
        this IEnumerable<TQuote> quotes,
        double maxPriceChangePercent = 0.1)
        where TQuote : IQuote
    {
        // check parameter arguments
        ValidateDoji(maxPriceChangePercent);

        // initialize
        List<CandleResult> results = quotes.ToCandleResults();
        decimal maxPctChange = (decimal)maxPriceChangePercent / 100m;
        int length = results.Count;

        // roll through candles
        for (int i = 0; i < length; i++)
        {
            CandleResult r = results[i];

            // check for current signal
            if (r.Candle.Open != 0)
            {
                if (Math.Abs((r.Candle.Close / r.Candle.Open) - 1m) <= maxPctChange)
                {
                    r.Price = r.Candle.Close;
                    r.Match = Match.Neutral;
                }
            }
        }

        return results;
    }

    // parameter validation
    private static void ValidateDoji(
        double maxPriceChangePercent)
    {
        // check parameter arguments
        if (maxPriceChangePercent is < 0 or > 0.5)
        {
            throw new ArgumentOutOfRangeException(nameof(maxPriceChangePercent), maxPriceChangePercent,
                "Maximum Percent Change must be between 0 and 0.5 for Doji (0% to 0.5%).");
        }
    }
}
