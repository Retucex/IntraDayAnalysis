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

		// Boundaries calculations
		static double marginPerc = 0.15;

		// Profit margins
		static double profit = 0.005;

		static int[] veryLowVolume = { 0, 999 };
		static int[] lowVolume = { 1000, 3999 };
		static int[] mediumVolume = { 4000, 34999 };
		static int[] highVolume = { 35000, 124999 };
		static int[] veryHighVolume = { 125000, 99999999 };

		static ConsoleColor standardColor = ConsoleColor.Gray;
		static ConsoleColor buyColor = ConsoleColor.Green;
		static ConsoleColor shortColor = ConsoleColor.Red;
		static ConsoleColor dontColor = ConsoleColor.DarkGray;
		static ConsoleColor dontPriceColor = ConsoleColor.Blue;

		static StreamWriter logFile = new StreamWriter("log.txt");
		static StreamWriter logFile2 = new StreamWriter("log2.txt");

		enum Action { buy, shrt, doNotTouch, undefined, outOfBounds }
		enum VolumeClass { veryLow, low, medium, high, veryHigh }

		static void LC(string s = "", bool color = false, ConsoleColor fore = ConsoleColor.Gray, ConsoleColor back = ConsoleColor.Black)
		{
			if (color)
			{
				Console.ForegroundColor = fore;
				Console.BackgroundColor = back;
			}
			Console.WriteLine(s);
			logFile.WriteLine(s);
			if (color)
			{
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.BackgroundColor = ConsoleColor.Black;
			}
		}

		public static void RunSimulation()
		{
			List<MarketDay> days = ExtractSimulationData();

			DateTime firstDay = days[0].DateTime;
			DateTime lastDay = days[days.Count - 1].DateTime;
			int daysSpanned = (lastDay - firstDay).Days;

			for (int i = 1; i <= daysSpanned; i++)
			{
				LC($"DAY {i}");
				foreach (var marketDay in days.Where(x => x.DateTime == firstDay.AddDays(i)))
				{
					double localHigh = 0;
					double localLow = double.MaxValue;
					int firstVolume = 0;
					int secondVolume = 0;
					double buyPrice = 0;
					double localDiff = 0;

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
					buyPrice = marketDay.DataPoints?.FirstOrDefault(x => x.DateTime.TimeOfDay == new TimeSpan(10, 30, 0))?.Close ?? 0;
					localDiff = (localHigh - localLow) * marginPerc;

					Console.ForegroundColor = standardColor;
					VolumeClass[] volClasses = new VolumeClass[2];
					Action actionBounds;
					Action action = BuyShortDontChoice(firstVolume, secondVolume, buyPrice, localDiff, localLow, localHigh, out volClasses, out actionBounds);
					string bounds = (actionBounds == Action.outOfBounds) ? "outOfBounds" : "";
					LC(marketDay.ToStringNice(false));
					LC($"||{action} {bounds}||   1stVol: {firstVolume} {volClasses[0]} 2ndVol: {secondVolume} {volClasses[1]} Low&High: {localLow} - {localHigh} bounds: {localLow + localDiff} - {localHigh - localDiff} BuyPrice: {buyPrice}");
					Console.ForegroundColor = standardColor;

					logFile2.WriteLine(marketDay.ToStringNice(false));
					logFile2.WriteLine($"||{action} {bounds}||   1stVol: {firstVolume} {volClasses[0]} 2ndVol: {secondVolume} {volClasses[1]} Low&High: {localLow} - {localHigh} bounds: {localLow + localDiff} - {localHigh - localDiff} BuyPrice: {buyPrice}");

					double sellUpPrice = buyPrice * (1 + profit);
					double sellDownPrice = buyPrice * (1 - profit);
					bool first = true;
					MarketDataPoint highest = new MarketDataPoint("", DateTime.Now, 0,0,0,0,0);
					MarketDataPoint lowest = new MarketDataPoint("", DateTime.Now, 0,0,0,0,0);
					int buyCount = 0;
					int shortCount = 0;
					foreach (MarketDataPoint marketDataPoint in marketDay.DataPoints.Where(x => x.DateTime.TimeOfDay > new TimeSpan(10, 30, 0)))
					{
						if (first)
						{
							first = false;
							highest = marketDataPoint;
							lowest = marketDataPoint;
						}

						if (marketDataPoint.High > sellUpPrice)
						{
							buyCount++;
							if (marketDataPoint.High > highest.High)
							{
								highest = marketDataPoint;
							}
							LC($"\t||buy||   Bought: {buyPrice} Sold: {sellUpPrice} At: {marketDataPoint.DateTime.TimeOfDay} High: {marketDataPoint.High} Percentage: {(marketDataPoint.High / buyPrice) - 1}", true, buyColor);
						}

						if (marketDataPoint.Low < sellDownPrice)
						{
							shortCount++;
							if (marketDataPoint.Low < lowest.Low)
							{
								lowest = marketDataPoint;
							}
							LC($"\t||short||   Bought: {buyPrice} Sold: {sellDownPrice} At: {marketDataPoint.DateTime.TimeOfDay} Low: {marketDataPoint.Low} Percentage: {1 - (marketDataPoint.Low / buyPrice)}", true, shortColor);
						}
					}
					logFile2.WriteLine($"\tbuyCount: {buyCount} shortCount: {shortCount}");
					logFile2.WriteLine($"\tHighest: {highest.DateTime.TimeOfDay} High: {highest.High} Percentage: {(highest.High / buyPrice) - 1}");
					logFile2.WriteLine($"\tLowest: {lowest.DateTime.TimeOfDay} Low: {lowest.Low} Percentage: {1 - (lowest.Low / buyPrice)}");
					logFile2.WriteLine();

					LC();
					LC($"\tHighest: {highest.DateTime.TimeOfDay} High: {highest.High} Percentage: {(highest.High / buyPrice) - 1}", true, buyColor);
					LC($"\tLowest: {lowest.DateTime.TimeOfDay} Low: {lowest.Low} Percentage: {1 - (lowest.Low / buyPrice)}", true, shortColor);
					LC();
					LC("\t---DETAILS---");
					LC($"\t{marketDay.ToStringNice()}");
					LC();
				}
				LC($"END OF DAY {i}");
				LC();
			}
		}

		static Action BuyShortDontChoice(int firstVolume, int secondVolume, double price, double diff, double low, double high, out VolumeClass[] volClasses, out Action boundsAction)
		{
			int[] volumes = new[] {firstVolume, secondVolume};
			volClasses = new VolumeClass[2];

			Action action = Action.undefined;
			boundsAction = Action.undefined;

			// Set volume classes
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
					volClasses[i] = VolumeClass.high;
				}
				if (volumes[i] >= veryHighVolume[0] && volumes[i] <= veryHighVolume[1])
				{
					volClasses[i] = VolumeClass.veryHigh;
				}
			}

			// Define action based on volume
			if (
				(volClasses[0] == VolumeClass.veryLow && volClasses[1] == VolumeClass.veryLow) ||
				(volClasses[0] == VolumeClass.veryLow && volClasses[1] == VolumeClass.low) ||
				(volClasses[0] == VolumeClass.low && volClasses[1] == VolumeClass.veryLow) ||
				(volClasses[0] == VolumeClass.medium && volClasses[1] == VolumeClass.veryLow)
				)
			{
				action = Action.doNotTouch;
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


			// Define action if price is not within bounds
			if (!(price > low + diff && price < high - diff))
			{
				boundsAction = Action.outOfBounds;
			}

			// Set console color for pretty text
			switch (action)
			{
				case Action.buy:
					Console.ForegroundColor = buyColor;
					break;

				case Action.shrt:
					Console.ForegroundColor = shortColor;
					break;

				case Action.doNotTouch:
					Console.ForegroundColor = dontColor;
					break;

				case Action.outOfBounds:
					Console.ForegroundColor = dontPriceColor;
					break;
			}

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
