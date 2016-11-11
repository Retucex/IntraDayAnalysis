using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntradayAnalysis
{
	using System.Diagnostics.Contracts;
	using System.Xml.Schema;

	static class MarketAnalysis
	{
		static double gapUpPercLow = 1.035;
		static double gapUpPercHigh = 1.145;

		static ConsoleColor standardColor = ConsoleColor.Gray;
		static ConsoleColor buyColor = ConsoleColor.Green;
		static ConsoleColor shortColor = ConsoleColor.Red;
		static ConsoleColor dontColor = ConsoleColor.DarkGray;
		static ConsoleColor dontPriceColor = ConsoleColor.Blue;

		static StreamWriter logFile = new StreamWriter("log.txt");
		static StreamWriter logFile2 = new StreamWriter("log2.txt");
		static StreamWriter logFile3 = new StreamWriter($"log3-{DateTime.Now.ToFileTime()}.txt");

		static void LC(string s = "", bool color = false, ConsoleColor fore = ConsoleColor.Gray, ConsoleColor back = ConsoleColor.Black)
		{
			if (color)
			{
				Console.ForegroundColor = fore;
				Console.BackgroundColor = back;
			}
			//Console.WriteLine(s);
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

			List<MarketGuess> guesses = new List<MarketGuess>();

			for (int i = 1; i <= daysSpanned; i++)
			{
				LC($"DAY {i}");
				foreach (var marketDay in days.Where(x => x.DateTime == firstDay.AddDays(i)))
				{
					MarketGuess guess = new MarketGuess { MarketDay = marketDay };

					// Determine opening analysis
					guess = OpeningAnalysis(guess);
					guess.DetermineMarketAction();

					// Determine points of 'going long' or shorthing
					guess = LongShortSimulation(guess);

					guesses.Add(guess);

					SimulationLog(guess);
				}
				LC($"END OF DAY {i}");
				LC();
			}

			Dictionary<string, List<MarketGuess>> SuccessFailures = new Dictionary<string, List<MarketGuess>>();
			#region Classification Success/Failures
			List<MarketGuess> Success = new List<MarketGuess>();
			List<MarketGuess> Fail = new List<MarketGuess>();
			List<MarketGuess> Uncertain = new List<MarketGuess>();
			List<MarketGuess> BoundsSuccess = new List<MarketGuess>();
			List<MarketGuess> BoundsFail = new List<MarketGuess>();
			List<MarketGuess> Undefined = new List<MarketGuess>();

			foreach (MarketGuess marketGuess in guesses)
			{
				if (marketGuess.MarketAction == MarketAction.undefined)
				{
					Undefined.Add(marketGuess);
				}

				else if (marketGuess.MarketAction == MarketAction.buy)
				{
					if (marketGuess.LongPoints.Count > marketGuess.ShortPoints.Count
						&& marketGuess.HighestSellPercentage > marketGuess.LowestShortPercentage)
					{
						if (marketGuess.BoundsMarketAction == MarketAction.withinBounds)
						{
							Success.Add(marketGuess);
						}
						else
						{
							BoundsFail.Add(marketGuess);
						}
					}
					else if (marketGuess.LongPoints.Count < marketGuess.ShortPoints.Count
							&& marketGuess.HighestSellPercentage < marketGuess.LowestShortPercentage)
					{
						if (marketGuess.BoundsMarketAction == MarketAction.withinBounds)
						{
							Fail.Add(marketGuess);
						}
						else
						{
							BoundsSuccess.Add(marketGuess);
						}
					}
					else
					{
						Uncertain.Add(marketGuess);
					}
				}

				else if (marketGuess.MarketAction == MarketAction.shrt)
				{
					if (marketGuess.LongPoints.Count < marketGuess.ShortPoints.Count
						&& marketGuess.HighestSellPercentage < marketGuess.LowestShortPercentage)
					{
						if (marketGuess.BoundsMarketAction == MarketAction.withinBounds)
						{
							Success.Add(marketGuess);
						}
						else
						{
							BoundsFail.Add(marketGuess);
						}
					}
					else if (marketGuess.LongPoints.Count > marketGuess.ShortPoints.Count
							&& marketGuess.HighestSellPercentage > marketGuess.LowestShortPercentage)
					{
						if (marketGuess.BoundsMarketAction == MarketAction.withinBounds)
						{
							Fail.Add(marketGuess);
						}
						else
						{
							BoundsSuccess.Add(marketGuess);
						}
					}
					else
					{
						Uncertain.Add(marketGuess);
					}
				}

				else if (marketGuess.MarketAction == MarketAction.doNotTouch)
				{
					if (Math.Abs(marketGuess.BuyPrice) < 0.001)
					{
						Success.Add(marketGuess);
					}
					else if (marketGuess.ShortPoints.Count < 5 && marketGuess.LongPoints.Count < 5)
					{
						Success.Add(marketGuess);
					}
					else if (marketGuess.ShortPoints.Count > 10 || marketGuess.LongPoints.Count > 10)
					{
						Fail.Add(marketGuess);
					}
					else
					{
						Uncertain.Add(marketGuess);
					}
				}
			}

			SuccessFailures.Add("Success", Success);
			SuccessFailures.Add("Fail", Fail);
			SuccessFailures.Add("Uncertain", Uncertain);
			SuccessFailures.Add("BoundsSuccess", BoundsSuccess);
			SuccessFailures.Add("BoundsFail", BoundsFail);
			SuccessFailures.Add("Undefined", Undefined);
			#endregion

			Dictionary<string, Dictionary<MarketAction, List<MarketGuess>>> BreakDownActions = new Dictionary<string, Dictionary<MarketAction, List<MarketGuess>>>();
			#region Action Breakdown
			foreach (KeyValuePair<string, List<MarketGuess>> keyValuePair in SuccessFailures)
			{
				Dictionary<MarketAction, List<MarketGuess>> ActionDict = new Dictionary<MarketAction, List<MarketGuess>>();
				List<MarketGuess> buy = new List<MarketGuess>();
				List<MarketGuess> shrt = new List<MarketGuess>();
				List<MarketGuess> noTouch = new List<MarketGuess>();
				List<MarketGuess> undefined = new List<MarketGuess>();
				List<MarketGuess> outOfBounds = new List<MarketGuess>();
				List<MarketGuess> withinBounds = new List<MarketGuess>();
				foreach (MarketGuess marketGuess in keyValuePair.Value)
				{
					switch (marketGuess.MarketAction)
					{
						case MarketAction.buy:
							buy.Add(marketGuess);
							break;

						case MarketAction.shrt:
							shrt.Add(marketGuess);
							break;

						case MarketAction.doNotTouch:
							noTouch.Add(marketGuess);
							break;

						case MarketAction.undefined:
							undefined.Add(marketGuess);
							break;
					}

					switch (marketGuess.BoundsMarketAction)
					{
						case MarketAction.withinBounds:
							withinBounds.Add(marketGuess);
							break;

						case MarketAction.outOfBounds:
							outOfBounds.Add(marketGuess);
							break;
					}
				}
				ActionDict.Add(MarketAction.buy, buy);
				ActionDict.Add(MarketAction.shrt, shrt);
				ActionDict.Add(MarketAction.doNotTouch, noTouch);
				ActionDict.Add(MarketAction.undefined, undefined);
				ActionDict.Add(MarketAction.withinBounds, withinBounds);
				ActionDict.Add(MarketAction.outOfBounds, outOfBounds);

				BreakDownActions.Add(keyValuePair.Key, ActionDict);
			}			
			#endregion

			double successRate = (double)Success.Count / (Success.Count + Fail.Count);
			double successBoundsRate = (double)(Success.Count + BoundsSuccess.Count) / (Success.Count + BoundsSuccess.Count + Fail.Count + BoundsFail.Count);

			int totalBuy = 0;
			int totalShort = 0;
			int totalDoNotTouch = 0;
			int totalUndefined = 0;
			int totalWithinBounds = 0;
			int totalOutOfBounds = 0;
			foreach (KeyValuePair<string, Dictionary<MarketAction, List<MarketGuess>>> action in BreakDownActions)
			{
				foreach (KeyValuePair<MarketAction, List<MarketGuess>> keyValuePair in action.Value)
				{
					if (keyValuePair.Key == MarketAction.buy)
						totalBuy += keyValuePair.Value.Count;
					if (keyValuePair.Key == MarketAction.shrt)
						totalShort += keyValuePair.Value.Count;
					if (keyValuePair.Key == MarketAction.doNotTouch)
						totalDoNotTouch += keyValuePair.Value.Count;
					if (keyValuePair.Key == MarketAction.undefined)
						totalUndefined += keyValuePair.Value.Count;
					if (keyValuePair.Key == MarketAction.withinBounds)
						totalWithinBounds += keyValuePair.Value.Count;
					if (keyValuePair.Key == MarketAction.outOfBounds)
						totalOutOfBounds += keyValuePair.Value.Count;
				}
			}

			logFile3.WriteLine("---Parameters---");
			logFile3.WriteLine($"Bounds margin:{MarketGuess.highLowBoundsPerc}");
			logFile3.WriteLine($"Profit:{MarketGuess.profit}");
			logFile3.WriteLine();
			logFile3.WriteLine(MarketVolume.OutputVolumeClasses());
			logFile3.WriteLine("------");
			logFile3.WriteLine();

			logFile3.WriteLine($"Guesses: {guesses.Count}");
			logFile3.WriteLine();
			logFile3.WriteLine($"Successes: {Success.Count}\t{(double)Success.Count/guesses.Count}");
			logFile3.WriteLine($"Failures: {Fail.Count}\t{(double)Fail.Count / guesses.Count}");
			logFile3.WriteLine($"Uncertain: {Uncertain.Count}\t{(double)Uncertain.Count / guesses.Count}");
			logFile3.WriteLine($"BoundsSuccess: {BoundsSuccess.Count}\t{(double)BoundsSuccess.Count / guesses.Count}");
			logFile3.WriteLine($"BoundsFail: {BoundsFail.Count}\t{(double)BoundsFail.Count / guesses.Count}");
			logFile3.WriteLine($"Undefined: {Undefined.Count}\t{(double)Undefined.Count / guesses.Count}");
			logFile3.WriteLine();

			logFile3.WriteLine($"Buy: {totalBuy}\t{(double)totalBuy / guesses.Count}");
			logFile3.WriteLine($"Short: {totalShort}\t{(double)totalShort / guesses.Count}");
			logFile3.WriteLine($"DoNotTouch: {totalDoNotTouch}\t{(double)totalDoNotTouch / guesses.Count}");
			logFile3.WriteLine($"Undefined: {totalUndefined}\t{(double)totalUndefined / guesses.Count}");
			logFile3.WriteLine($"WithinBounds: {totalWithinBounds}\t{(double)totalWithinBounds / guesses.Count}");
			logFile3.WriteLine($"OutOfBounds: {totalOutOfBounds}\t{(double)totalOutOfBounds / guesses.Count}");
			logFile3.WriteLine();

			foreach (KeyValuePair<string, Dictionary<MarketAction, List<MarketGuess>>> breakDownAction in BreakDownActions)
			{
				logFile3.WriteLine($"{breakDownAction.Key}:");
				foreach (KeyValuePair<MarketAction, List<MarketGuess>> keyValuePair in breakDownAction.Value)
				{
					logFile3.WriteLine($"{keyValuePair.Key} {keyValuePair.Value.Count}");
				}
				logFile3.WriteLine();
			}

			foreach (KeyValuePair<string, List<MarketGuess>> keyValuePair in SuccessFailures)
			{
				logFile3.WriteLine($"---{keyValuePair.Key}---");
				foreach (MarketGuess marketGuess in keyValuePair.Value)
				{
					logFile3.WriteLine(marketGuess);
				}
			}
		}

		static void SimulationLog(MarketGuess guess)
		{
			List<MarketDataPoint> LongShortList = new List<MarketDataPoint>();
			LongShortList.AddRange(guess.LongPoints);
			LongShortList.AddRange(guess.ShortPoints);

			Console.ForegroundColor = standardColor;

			SetConsoleColorAction(guess);

			LC(guess.MarketDay.ToStringNice(false));
			LC(
				$"||{guess.MarketAction} {guess.BoundsMarketAction}||   1stVol: {guess.FirstVolume.Volume} {guess.FirstVolume.VolumeClass} 2ndVol: {guess.SecondVolume.Volume} {guess.SecondVolume.VolumeClass} Low&High: {guess.LocalLow} - {guess.LocalHigh} bounds: {guess.LocalLow + guess.LocalDiff} - {guess.LocalHigh - guess.LocalDiff} BuyPrice: {guess.BuyPrice}");
			logFile2.WriteLine(guess.MarketDay.ToStringNice(false));
			logFile2.WriteLine(
				$"||{guess.MarketAction} {guess.BoundsMarketAction}||   1stVol: {guess.FirstVolume.Volume} {guess.FirstVolume.VolumeClass} 2ndVol: {guess.SecondVolume.Volume} {guess.SecondVolume.VolumeClass} Low&High: {guess.LocalLow} - {guess.LocalHigh} bounds: {guess.LocalLow + guess.LocalDiff} - {guess.LocalHigh - guess.LocalDiff} BuyPrice: {guess.BuyPrice}");

			Console.ForegroundColor = standardColor;

			foreach (var point in LongShortList.OrderBy(x => x.DateTime))
			{
				if (point.High > guess.SellUpPrice)
				{
					LC(
						$"\t||buy||   Bought: {guess.BuyPrice} Sold: {guess.SellUpPrice} At: {point.DateTime.TimeOfDay} High: {point.High} Percentage: {(point.High / guess.BuyPrice) - 1}",
						true,
						buyColor);
				}

				if (point.Low < guess.SellDownPrice)
				{
					LC(
						$"\t||short||   Bought: {guess.BuyPrice} Sold: {guess.SellDownPrice} At: {point.DateTime.TimeOfDay} Low: {point.Low} Percentage: {1 - (point.Low / guess.BuyPrice)}",
						true,
						shortColor);
				}
			}

			if (guess.HighestSellPoint != null)
			{
				logFile2.WriteLine($"\tbuyCount: {guess.LongPoints.Count} shortCount: {guess.ShortPoints.Count}");
				logFile2.WriteLine(
					$"\tHighest: {guess.HighestSellPoint.DateTime.TimeOfDay} High: {guess.HighestSellPoint.High} Percentage: {guess.HighestSellPercentage}");
				logFile2.WriteLine(
					$"\tLowest: {guess.LowestShortPoint.DateTime.TimeOfDay} Low: {guess.LowestShortPoint.Low} Percentage: {guess.LowestShortPercentage}");
				logFile2.WriteLine();

				LC();
				LC(
					$"\tHighest: {guess.HighestSellPoint.DateTime.TimeOfDay} High: {guess.HighestSellPoint.High} Percentage: {(guess.HighestSellPoint.High / guess.BuyPrice) - 1}",
					true,
					buyColor);
				LC(
					$"\tLowest: {guess.LowestShortPoint.DateTime.TimeOfDay} Low: {guess.LowestShortPoint.Low} Percentage: {1 - (guess.LowestShortPoint.Low / guess.BuyPrice)}",
					true,
					shortColor);
				LC();
				LC("\t---DETAILS---");
				LC($"\t{guess.MarketDay.ToStringNice()}");
			}
			LC();
		}

		static MarketGuess LongShortSimulation(MarketGuess guess)
		{
			foreach (MarketDataPoint marketDataPoint in guess.MarketDay.DataPoints
				.Where(x => x.DateTime.TimeOfDay > new TimeSpan(10, 30, 0)))
			{
				if (marketDataPoint.High > guess.SellUpPrice)
				{
					guess.LongPoints.Add(marketDataPoint);
				}
				if (marketDataPoint.Low < guess.SellDownPrice)
				{
					guess.ShortPoints.Add(marketDataPoint);
				}
			}

			return guess;
		}

		static MarketGuess OpeningAnalysis(MarketGuess guess)
		{
			//From 9:30 to 10:10
			foreach (var marketDataPoint in guess.MarketDay.DataPoints
				.Where(x => x.DateTime.TimeOfDay <= new TimeSpan(10, 10, 0)))
			{
				guess.FirstVolume.Volume += marketDataPoint.Volume;

				if (marketDataPoint.High > guess.LocalHigh)
				{
					guess.LocalHigh = marketDataPoint.High;
				}
				if (marketDataPoint.Low < guess.LocalLow)
				{
					guess.LocalLow = marketDataPoint.Low;
				}
			}

			//From 10:10 to 10:30
			foreach (var marketDataPoint in guess.MarketDay.DataPoints
					.Where(x => x.DateTime.TimeOfDay > new TimeSpan(10, 10, 0) && x.DateTime.TimeOfDay <= new TimeSpan(10, 30, 0)))
			{
				guess.SecondVolume.Volume += marketDataPoint.Volume;

				if (marketDataPoint.High > guess.LocalHigh)
				{
					guess.LocalHigh = marketDataPoint.High;
				}
				if (marketDataPoint.Low < guess.LocalLow)
				{
					guess.LocalLow = marketDataPoint.Low;
				}
			}

			guess.FirstVolume.Volume /= 40;
			guess.SecondVolume.Volume /= 20;
			guess.BuyPrice = guess.MarketDay.DataPoints?
				.FirstOrDefault(x => x.DateTime.TimeOfDay == new TimeSpan(10, 30, 0))?.Close ?? 0;

			return guess;
		}

		static void SetConsoleColorAction(MarketGuess guess)
		{
			// Set console color for pretty text
			switch (guess.MarketAction)
			{
				case MarketAction.buy:
					Console.ForegroundColor = buyColor;
					break;

				case MarketAction.shrt:
					Console.ForegroundColor = shortColor;
					break;

				case MarketAction.doNotTouch:
					Console.ForegroundColor = dontColor;
					break;

				case MarketAction.outOfBounds:
					Console.ForegroundColor = dontPriceColor;
					break;
			}
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
