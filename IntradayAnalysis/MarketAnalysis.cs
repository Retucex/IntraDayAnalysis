using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntradayAnalysis
{
	static class MarketAnalysis
	{
		static double gapUpPercLow = 1.035;
		static double gapUpPercHigh = 1.145;

		static int[] veryLowVolume = new[] { 0, 999 };
		static int[] lowVolume = new[] { 1000, 3999 };
		static int[] mediumVolume = new[] { 4000, 34999 };
		static int[] highVolume = new[] { 35000, 124999 };
		static int[] veryHighVolume = new[] { 125000, 99999999 };

		static double marginPerc = 0.3;

		enum Action { buy, shrt, dont, undefined, dontPrice }
		enum VolumeClass { veryLow, low, medium, high, veryHigh }

		public static void RunSimulation()
		{
			ConsoleColor standardColor = Console.ForegroundColor;
			ConsoleColor buyColor = ConsoleColor.Green;
			ConsoleColor shortColor = ConsoleColor.Red;
			ConsoleColor dontColor = ConsoleColor.DarkGray;
			ConsoleColor dontPriceColor = ConsoleColor.DarkBlue;

			List<MarketDay> days = ExtractSimulationData();

			DateTime firstDay = days[0].DateTime;
			DateTime lastDay = days[days.Count - 1].DateTime;
			int daysSpanned = (lastDay - firstDay).Days;

			Console.WriteLine(firstDay);
			Console.WriteLine(lastDay);

			for (int i = 1; i <= daysSpanned; i++)
			{
				Console.WriteLine($"DAY {i}");
				foreach (var marketDay in days.Where(x => x.DateTime == firstDay.AddDays(i)))
				{
					double localHigh = 0;
					double localLow = double.MaxValue;
					int firstVolume = 0;
					int secondVolume = 0;

					//From 9:30 to 10:10
					foreach (var marketDataPoint in marketDay.DataPoints.Where(x => x.DateTime.TimeOfDay <= new TimeSpan(10, 10, 0)))
					{
                        firstVolume += marketDataPoint.Volume;

						if (marketDataPoint.High > localHigh)
						{
							localHigh = marketDataPoint.High;
						}
						if (marketDataPoint.Low < localLow)
						{
							localLow = marketDataPoint.Low;
						}
					}

					//From 10:10 to 10:30
					foreach (var marketDataPoint in marketDay.DataPoints.Where(x => x.DateTime.TimeOfDay > new TimeSpan(10, 10, 0) && x.DateTime.TimeOfDay <= new TimeSpan(10, 30, 0)))
					{
						secondVolume += marketDataPoint.Volume;
					}

					firstVolume /= 40;
					secondVolume /= 20;

					double price1030 = marketDay.DataPoints?.FirstOrDefault(x => x.DateTime.TimeOfDay == new TimeSpan(10, 30, 0))?.Close ?? 0;

					Console.ForegroundColor = standardColor;
					Action action = BuyShortDontChoice(firstVolume, secondVolume, price1030, localHigh, localLow);
					switch (action)
					{
							case Action.buy:
							Console.ForegroundColor = buyColor;
							break;

							case Action.shrt:
							Console.ForegroundColor = shortColor;
							break;

							case Action.dont:
							Console.ForegroundColor = dontColor;
							break;

							case Action.dontPrice:
							Console.ForegroundColor = dontPriceColor;
							break;
					}

					Console.WriteLine($"-- localHigh: {localHigh}  localLow: {localLow}  1stVol: {firstVolume}  2ndVol: {secondVolume}");
					Console.ForegroundColor = standardColor;
					Console.WriteLine(marketDay.ToStringVerbose(false));
					Console.WriteLine();
					
				}
				Console.WriteLine($"END OF DAY {i}");
				Console.WriteLine();
			}
		}

		static Action BuyShortDontChoice(int firstVolume, int secondVolume, double price, double high, double low)
		{
			int[] volumes = new[] {firstVolume, secondVolume};
			VolumeClass[] volClasses = new VolumeClass[2];

			Action action = Action.undefined;

			for (int i = 0; i < 2; i++)
			{
				if (volumes[i] >= veryLowVolume[0] && volumes[i] <= veryLowVolume[1])
				{
					volClasses[i] = VolumeClass.veryLow;
				}
				if (volumes[i] >= lowVolume[0] && volumes[i] <= lowVolume[1])
				{
					volClasses[i] = VolumeClass.low;
				}
				if (volumes[i] >= mediumVolume[0] && volumes[i] <= mediumVolume[1])
				{
					volClasses[i] = VolumeClass.medium;
				}
				if (volumes[i] >= highVolume[0] && volumes[i] <= highVolume[1])
				{
					volClasses[i] = VolumeClass.veryLow;
				}
				if (volumes[i] >= veryHighVolume[0] && volumes[i] <= veryHighVolume[1])
				{
					volClasses[i] = VolumeClass.veryHigh;
				}
			}

			if (
				(volClasses[0] == VolumeClass.veryLow && volClasses[1] == VolumeClass.veryLow) ||
				(volClasses[0] == VolumeClass.veryLow && volClasses[1] == VolumeClass.low) ||
				(volClasses[0] == VolumeClass.low && volClasses[1] == VolumeClass.veryLow) ||
				(volClasses[0] == VolumeClass.medium && volClasses[1] == VolumeClass.veryLow)
				)
			{
				action = Action.dont;
			}

			if (
				(volClasses[0] == VolumeClass.low && volClasses[1] == VolumeClass.low) ||
				(volClasses[0] == VolumeClass.medium && volClasses[1] == VolumeClass.medium) ||
				(volClasses[0] == VolumeClass.medium && volClasses[1] == VolumeClass.high) ||
				(volClasses[0] == VolumeClass.medium && volClasses[1] == VolumeClass.veryHigh) ||
				(volClasses[0] == VolumeClass.high && volClasses[1] == VolumeClass.medium) ||
				(volClasses[0] == VolumeClass.high && volClasses[1] == VolumeClass.high) ||
				(volClasses[0] == VolumeClass.high && volClasses[1] == VolumeClass.veryHigh) ||
				(volClasses[0] == VolumeClass.veryHigh && volClasses[1] == VolumeClass.veryHigh) ||
				(volClasses[0] == VolumeClass.veryHigh && volClasses[1] == VolumeClass.high) ||
				(volClasses[0] == VolumeClass.veryHigh && volClasses[1] == VolumeClass.medium)
				)
			{
				action = Action.buy;
			}

			if (
				(volClasses[0] == VolumeClass.medium && volClasses[1] == VolumeClass.low) ||
				(volClasses[0] == VolumeClass.high && volClasses[1] == VolumeClass.low)
				)
			{
				action = Action.shrt;
			}

			/*
			if (!(price > low*(marginPerc + 1) && price < high * (1 - marginPerc)))
			{
				action = Action.dontPrice;
			}
			//*/

			return action;
		}
		static List<MarketDay> ExtractSimulationData()
		{
			var file = File.ReadLines("output.txt");

			List<MarketDay> days = new List<MarketDay>();
			MarketDay day = new MarketDay();
			MarketDataPoint dataPoint = new MarketDataPoint("", DateTime.Now, 0, 0, 0, 0, 0);
			bool first = true;

			foreach (var s in file)
			{
				if (s != "")
				{
					if (!s.Contains("\t"))
					{
						if (!first)
						{
							days.Add(day);
						}
						day = new MarketDay();
						day.Gap = double.Parse(s.Split(',')[7]);
						first = false;
					}
					else
					{
						day.AddDataPoint(ExtractDataString(s));
					}
				}
			}

			foreach (var marketDay in days)
			{
				if (marketDay.DateTime < new DateTime(2016, 11, 1))
				{
					foreach (var marketDataPoint in marketDay.DataPoints)
					{
						marketDataPoint.DateTime = marketDataPoint.DateTime.AddHours(-1);
					}
				}
			}

			return days;
		}

		static MarketDataPoint ExtractDataString(string s)
		{

			//A,2016-10-05 12:00:00 AM,47.04,47.15,47.31,46.9275,1277119,1.04731158855616
			//	A,2016-10-05 10:35:00 AM,47.04,46.965,47.065,46.9275,30215
			var data = s.Split(',');

			// Remove tab
			data[0] = data[0].Split('\t')[1];

			DateTime dt = DateTime.Parse(data[1]);

			try
			{
				MarketDataPoint dataPoint = new MarketDataPoint(
				data[0],
				dt,
				double.Parse(data[2]),
				double.Parse(data[3]),
				double.Parse(data[4]),
				double.Parse(data[5]),
				int.Parse(data[6]));

				return dataPoint;
			}
			catch (Exception e)
			{
				foreach (var s1 in data)
				{
					Console.WriteLine(s1);
				}
			}

			return null;
		}

		public static void CreateGapFile()
		{
			Console.Write("Extracting Data... ");
			List<MarketDataPoint> dataPoints = ExtractData();
			Console.WriteLine(" Done.");

			Console.Write("Populating MarketDays... ");
			List<MarketDay> marketDays = PopulateDays(dataPoints);
			Console.WriteLine(" Done.");

			Console.Write("Populating GappedDays... ");
			List<MarketDay> gappedDays = GetGappedDays(marketDays);
			Console.WriteLine(" Done.");

			var output = new StreamWriter("output.txt");
			foreach (var gappedDay in gappedDays)
			{
				output.WriteLine(gappedDay.ToStringVerbose());
			}
		}

		static List<MarketDataPoint> ExtractData()
		{
			// Process data file
			List<MarketDataPoint> dataPoints = new List<MarketDataPoint>();
			var file = File.ReadLines("data.txt");

			double totalCount = file.LongCount();
			double count = 0;
			double per = 0;
			double previousPer = 0;

			foreach (string s in file)
			{
				count++;
				per = count / totalCount;
				if (per > previousPer + 0.05)
				{
					previousPer = per;
					Console.Write("|");
				}

				var data = s.Split(',');

				// Remove market suffix
				data[0] = data[0].Split('.')[0];

				int year = int.Parse(data[2].Substring(0, 4));
				int month = int.Parse(data[2].Substring(4, 2));
				int day = int.Parse(data[2].Substring(6, 2));

				int hours = int.Parse(data[3].Substring(0, 2));
				int minutes = int.Parse(data[3].Substring(2, 2));
				int seconds = int.Parse(data[3].Substring(4, 2));

				DateTime dt = new DateTime(year, month, day, hours, minutes, seconds);
				dt = dt.AddHours(-5); //Adjust timezone

				MarketDataPoint dataPoint = new MarketDataPoint(
					data[0],
					dt,
					double.Parse(data[4]),
					double.Parse(data[7]),
					double.Parse(data[5]),
					double.Parse(data[6]),
					int.Parse(data[8]));

				dataPoints.Add(dataPoint);
			}

			return dataPoints;
		}

		static List<MarketDay> PopulateDays(List<MarketDataPoint> marketDataPoints)
		{
			List<MarketDay> marketDays = new List<MarketDay>();

			MarketDataPoint currentDP = new MarketDataPoint("", DateTime.Now, 0, 0, 0, 0, 0);
			MarketDataPoint previousDP;

			MarketDay day = new MarketDay();

			bool firstDP = true;

			double totalCount = marketDataPoints.Count;
			double count = 0;
			double per = 0;
			double previousPer = 0;

			foreach (MarketDataPoint dataPoint in marketDataPoints)
			{
				count++;
				per = count / totalCount;
				if (per > previousPer + 0.05)
				{
					previousPer = per;
					Console.Write("|");
				}

				if (firstDP)
				{
					firstDP = false;

					currentDP = dataPoint;
					day = new MarketDay();
					day.AddDataPoint(currentDP);
				}
				else
				{
					previousDP = currentDP;
					currentDP = dataPoint;

					if (currentDP.Ticker == previousDP.Ticker)
					{
						day.AddDataPoint(currentDP);
					}
					else
					{
						marketDays.Add(day);
						day = new MarketDay();
						day.AddDataPoint(currentDP);
					}
				}
			}

			return marketDays;
		}

		static List<MarketDay> GetGappedDays(List<MarketDay> days)
		{
			List<MarketDay> gappedDays = new List<MarketDay>();

			double totalCount = days.Count;
			double count = 0;
			double per = 0;
			double previousPer = 0;

			foreach (var marketDay in days)
			{
				count++;
				per = count / totalCount;
				if (per > previousPer + 0.05)
				{
					previousPer = per;
					Console.Write("|");
				}

				MarketDay previousDay =
					days.Find(x => x.Ticker == marketDay.Ticker && x.DateTime.Day == marketDay.DateTime.AddDays(-1).Day);

				if (previousDay != null)
				{
					if (marketDay.Open / previousDay.High > gapUpPercLow && marketDay.Open / previousDay.High < gapUpPercHigh)
					{
						marketDay.Gap = marketDay.Open / previousDay.High;
						gappedDays.Add(marketDay);
					}
				}
			}

			return gappedDays;
		}
	}
}
