using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntradayAnalysis
{
	public enum MarketAction { buy, shrt, doNotTouch, undefined, outOfBounds, withinBounds }
	class MarketGuess
	{
		// Boundaries calculations
		public static double highLowBoundsPerc = 0.15;

		// Profit Margin
		public static double profit = 0.01;

		double localHigh;
		double localLow;

		public MarketDay MarketDay { get; set; }

		public List<MarketDataPoint> ShortPoints { get; set; } 
		public List<MarketDataPoint> LongPoints { get; set; } 

		public MarketAction MarketAction { get; set; }
		public MarketAction BoundsMarketAction { get; set; }

		public MarketVolume FirstVolume { get; set; }
		public MarketVolume SecondVolume { get; set; }

		public double LocalHigh
		{
			get
			{
				return localHigh;
			}
			set
			{
				localHigh = value;
				LocalDiff = (LocalHigh - LocalLow) * highLowBoundsPerc;
			}
		}
		public double LocalLow
		{
			get
			{
				return localLow;
			}
			set
			{
				localLow = value;
				LocalDiff = (LocalHigh - LocalLow) * highLowBoundsPerc;
			}
		}

		public double BuyPrice { get; set; }
		public double LocalDiff { get; private set; }

		public double SellUpPrice => BuyPrice * (1 + profit);
		public double SellDownPrice => BuyPrice * (1 - profit);

		public MarketDataPoint HighestSellPoint
			=>
			(MarketDay.DataPoints.Count(x => x.DateTime.TimeOfDay > new TimeSpan(10, 30, 0)) != 0) ?
				MarketDay.DataPoints.Where(x => x.DateTime.TimeOfDay > new TimeSpan(10, 30, 0))
					.OrderByDescending(x => x.High)
					.First()
			: null;

		public MarketDataPoint LowestShortPoint
			=>
			(MarketDay.DataPoints.Count(x => x.DateTime.TimeOfDay > new TimeSpan(10, 30, 0)) != 0) ?
				MarketDay.DataPoints.Where(x => x.DateTime.TimeOfDay > new TimeSpan(10, 30, 0))
					.OrderBy(x => x.Low)
					.First()
			: null;

		public double HighestSellPercentage => (HighestSellPoint?.High / BuyPrice) - 1 ?? -1;
		public double LowestShortPercentage => 1 - (LowestShortPoint?.Low / BuyPrice) ?? -1;

		public MarketGuess()
		{
			ShortPoints = new List<MarketDataPoint>();
			LongPoints = new List<MarketDataPoint>();

			LocalHigh = 0;
			LocalLow = double.MaxValue;

			FirstVolume = new MarketVolume(0);
			SecondVolume = new MarketVolume(0);

			BuyPrice = 0;
			LocalDiff = 0;

			MarketAction = MarketAction.undefined;
			BoundsMarketAction = MarketAction.withinBounds;
		}

		public void DetermineMarketAction()
		{
			// Define action based on volume
			if (
				(FirstVolume.VolumeClass == VolumeClass.veryLow && SecondVolume.VolumeClass == VolumeClass.veryLow) ||
				(FirstVolume.VolumeClass == VolumeClass.veryLow && SecondVolume.VolumeClass == VolumeClass.low) ||
				(FirstVolume.VolumeClass == VolumeClass.low && SecondVolume.VolumeClass == VolumeClass.veryLow) ||
				(FirstVolume.VolumeClass == VolumeClass.medium && SecondVolume.VolumeClass == VolumeClass.veryLow)
				)
			{
				MarketAction = MarketAction.doNotTouch;
			}

			if (
				(FirstVolume.VolumeClass == VolumeClass.veryLow && SecondVolume.VolumeClass == VolumeClass.medium) ||
				(FirstVolume.VolumeClass == VolumeClass.low && SecondVolume.VolumeClass == VolumeClass.low) ||
				(FirstVolume.VolumeClass == VolumeClass.low && SecondVolume.VolumeClass == VolumeClass.medium) ||
				(FirstVolume.VolumeClass == VolumeClass.medium && SecondVolume.VolumeClass == VolumeClass.medium) ||
				(FirstVolume.VolumeClass == VolumeClass.medium && SecondVolume.VolumeClass == VolumeClass.high) ||
				(FirstVolume.VolumeClass == VolumeClass.medium && SecondVolume.VolumeClass == VolumeClass.veryHigh) ||
				(FirstVolume.VolumeClass == VolumeClass.high && SecondVolume.VolumeClass == VolumeClass.medium) ||
				(FirstVolume.VolumeClass == VolumeClass.high && SecondVolume.VolumeClass == VolumeClass.high) ||
				(FirstVolume.VolumeClass == VolumeClass.high && SecondVolume.VolumeClass == VolumeClass.veryHigh) ||
				(FirstVolume.VolumeClass == VolumeClass.veryHigh && SecondVolume.VolumeClass == VolumeClass.veryHigh) ||
				(FirstVolume.VolumeClass == VolumeClass.veryHigh && SecondVolume.VolumeClass == VolumeClass.high) ||
				(FirstVolume.VolumeClass == VolumeClass.veryHigh && SecondVolume.VolumeClass == VolumeClass.medium)
				)
			{
				MarketAction = MarketAction.buy;
			}

			if (
				(FirstVolume.VolumeClass == VolumeClass.medium && SecondVolume.VolumeClass == VolumeClass.low) ||
				(FirstVolume.VolumeClass == VolumeClass.high && SecondVolume.VolumeClass == VolumeClass.low)
				)
			{
				MarketAction = MarketAction.shrt;
			}


			// Define action if price is not within bounds
			if (!(BuyPrice > LocalLow + LocalDiff && BuyPrice < LocalHigh - LocalDiff))
			{
				BoundsMarketAction = MarketAction.outOfBounds;
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(MarketDay.ToStringNice(false));
			sb.AppendLine($"Action:{MarketAction} {BoundsMarketAction} Volumes:{FirstVolume.Volume}-{SecondVolume.Volume} {FirstVolume.VolumeClass}-{SecondVolume.VolumeClass} LowHigh:{LocalLow}-{LocalHigh} Bounds:{LocalLow+LocalDiff}-{LocalHigh-LocalDiff} BuyPrice:{BuyPrice}");
			sb.AppendLine($"Long:{LongPoints.Count} Highest:{HighestSellPoint?.High ?? 0} at {HighestSellPoint?.DateTime.TimeOfDay ?? new TimeSpan(0)} {HighestSellPercentage.ToString("f4")}");
			sb.AppendLine($"Short:{ShortPoints.Count} Lowest:{LowestShortPoint?.Low ?? 0} at {LowestShortPoint? .DateTime.TimeOfDay ?? new TimeSpan(0)} {LowestShortPercentage.ToString("f4")}");

			return sb.ToString();
		}
	}
}
