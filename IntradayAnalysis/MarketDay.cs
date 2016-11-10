using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntradayAnalysis
{
	class MarketDay
	{
		public List<MarketDataPoint> DataPoints { get; private set; }

		public string Ticker { get; private set; }
		public DateTime DateTime { get; private set; }
		public double Open { get; private set; }
		public double Close { get; private set; }
		public double High { get; private set; }
		public double Low { get; private set; }
		public int Volume { get; private set; }
		public double Gap { get; set; }

		public MarketDay()
		{
			DataPoints = new List<MarketDataPoint>();

			Open = 0;
			Close = 0;
			High = 0;
			Low = 0;
			Volume = 0;
			Gap = 0;
		}

		public void AddDataPoint(MarketDataPoint dataPoint)
		{
			if (DataPoints.Count == 0)
			{
				Ticker = dataPoint.Ticker;
				DateTime = dataPoint.DateTime.Date;
				DataPoints.Add(dataPoint);
			}
			else if (dataPoint.DateTime.Date == DateTime.Date && dataPoint.Ticker == Ticker)
			{
				DataPoints.Add(dataPoint);
			}

			CalculateDay();
		}

		private void CalculateDay()
		{
			DataPoints = DataPoints.OrderBy(x => x.DateTime).ToList();
			Open = DataPoints[0].Open;
			Close = DataPoints[DataPoints.Count - 1].Close;

			High = 0;
			Low = double.MaxValue;
			Volume = 0;

			foreach (MarketDataPoint dataPoint in DataPoints)
			{
				if (dataPoint.High > High)
				{
					High = dataPoint.High;
				}

				if (dataPoint.Low < Low)
				{
					Low = dataPoint.Low;
				}

				Volume += dataPoint.Volume;
			}
		}

		public string ToStringVerbose(bool verbose = true)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(Ticker).Append(",");
			sb.Append(DateTime).Append(",");
			sb.Append(Open.ToString()).Append(",");
			sb.Append(Close.ToString()).Append(",");
			sb.Append(High.ToString()).Append(",");
			sb.Append(Low.ToString()).Append(",");
			sb.Append(Volume.ToString()).Append(",");
			sb.Append(Gap.ToString());

			if (verbose)
			{
				sb.AppendLine();
				foreach (var marketDataPoint in DataPoints)
				{
					sb.Append("\t").AppendLine(marketDataPoint.ToString());
				}
			}

			return sb.ToString();
		}

		public override string ToString()
		{
			return ToStringVerbose(false);
		}
	}
}
