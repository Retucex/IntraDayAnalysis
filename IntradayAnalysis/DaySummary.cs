using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntradayAnalysis
{
	public enum MarketBetStatus
	{
		soldLong,
		soldShort,
		failedLong,
		failedShort,
		error
	}
	class DaySummary
	{
		Dictionary<MarketBetStatus, List<MarketGuess>> statusGuesses;
		double dayProfit;
		public Dictionary<MarketBetStatus, List<MarketGuess>> StatusGuesses
		{
			get
			{
				return statusGuesses;
			}
			set
			{
				statusGuesses = value;
				dayProfit = CalculateProfit();
			}
		}
		public DateTime Date { get; set; }

		public int LossShort => StatusGuesses[MarketBetStatus.failedShort].Count;
		public int LossLong => StatusGuesses[MarketBetStatus.failedLong].Count;
		public int SoldShort => StatusGuesses[MarketBetStatus.soldShort].Count;
		public int SoldLong => StatusGuesses[MarketBetStatus.soldLong].Count;
		public int TotalLoss => LossShort + LossLong;
		public int TotalSold => SoldShort + SoldLong;
		public int TotalTrades => TotalLoss + TotalSold;
		public double DayProfit => dayProfit;

		public double AvgLoss
			=>
			(TotalLoss > 0) ?
				StatusGuesses[MarketBetStatus.failedLong]
					.Concat(StatusGuesses[MarketBetStatus.failedShort])
					.Sum(x => x.Profit)
					/ TotalLoss
			: 0;

		public DaySummary()
		{
			StatusGuesses = new Dictionary<MarketBetStatus, List<MarketGuess>>();
		}

		double CalculateProfit()
		{
			double profit = 0;
			int count = 0;
			StatusGuesses.Values.ToList().ForEach(x => count += x.Count);

			foreach (KeyValuePair<MarketBetStatus, List<MarketGuess>> keyValuePair in StatusGuesses)
			{
				if (keyValuePair.Key == MarketBetStatus.failedShort || keyValuePair.Key == MarketBetStatus.failedLong)
				{
					foreach (MarketGuess marketGuess in keyValuePair.Value)
					{
						MarketDataPoint fail = marketGuess.MarketDay.DataPoints.FirstOrDefault(x => x.DateTime.TimeOfDay >= new TimeSpan(15, 55, 0));
						marketGuess.Profit = Math.Abs(1 - (fail.Close / marketGuess.BuyPrice)) * -1;
						profit += marketGuess.Profit;
					}
				}
				if (keyValuePair.Key == MarketBetStatus.soldShort || keyValuePair.Key == MarketBetStatus.soldLong)
				{
					foreach (MarketGuess marketGuess in keyValuePair.Value)
					{
						marketGuess.Profit = MarketGuess.profit;
						profit += marketGuess.Profit;
					}
				}
			}

			return profit / count;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(Date.ToShortDateString());
			sb.AppendLine($" Sold:{TotalSold}\t{((double)TotalSold/TotalTrades).ToString("f4")}");
			sb.AppendLine($"  Long:{SoldLong}\t{((double)SoldLong / TotalSold).ToString("f4")}");
			sb.AppendLine($"  Short:{SoldShort}\t{((double)SoldShort / TotalSold).ToString("f4")}");
			sb.AppendLine($" Loss:{TotalLoss}\t{((double)TotalLoss / TotalTrades).ToString("f4")}");
			sb.AppendLine($"  Long:{LossLong}\t{((double)LossLong / TotalLoss).ToString("f4")}");
			sb.AppendLine($"  Short:{LossShort}\t{((double)LossShort / TotalLoss).ToString("f4")}");
			sb.AppendLine($" Profit:{DayProfit}");
			sb.Append($" Avg Loss:{AvgLoss}");

			return sb.ToString();
		}
	}
}
