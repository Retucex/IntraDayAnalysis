using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntradayAnalysis
{
	public class MarketDataPoint
	{
		public string Ticker { get; private set; }
		public DateTime DateTime { get; set; }
		public double Open { get; private set; }
		public double Close { get; private set; }
		public double High { get; private set; }
		public double Low { get; private set; }
		public int Volume { get; private set; }

		public MarketDataPoint(
			string ticker,
			DateTime dateTime,
			double open,
			double close,
			double high,
			double low,
			int volume)
		{
			Ticker = ticker;
			DateTime = dateTime;
			Open = open;
			Close = close;
			High = high;
			Low = low;
			Volume = volume;
		}

		public string ToStringNice()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Ticker:").Append(Ticker).Append(" ");
			sb.Append("DateTime:").Append(DateTime).Append(" ");
			sb.Append("Open:").Append(Open.ToString()).Append(" ");
			sb.Append("Close:").Append(Close.ToString()).Append(" ");
			sb.Append("High:").Append(High.ToString()).Append(" ");
			sb.Append("Low:").Append(Low.ToString()).Append(" ");
			sb.Append("Volume:").Append(Volume.ToString());

			return sb.ToString();
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(Ticker).Append(",");
			sb.Append(DateTime).Append(",");
			sb.Append(Open.ToString()).Append(",");
			sb.Append(Close.ToString()).Append(",");
			sb.Append(High.ToString()).Append(",");
			sb.Append(Low.ToString()).Append(",");
			sb.Append(Volume.ToString());

			return sb.ToString();
		}
	}
}
