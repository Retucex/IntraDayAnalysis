namespace IntradayAnalysis.Training
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Windows.Forms;
	using System.Windows.Forms.DataVisualization.Charting;
	using Microsoft.VisualBasic;

	public partial class Form1 : Form
	{
		public static List<MarketGuess> days = MarketAnalysis.RunSimulation();
		MarketGuess day;
		int count = 1;
		double volAvg = 0;
		double lh;
		double ll;
		MarketDataPoint buyPoint = null;
		MarketDataPoint sellPoint = null;
		double profit = 0;
		bool goingLong = false;


		//Session Stats
		double totalProfit = 0;
		int totalCount = 0;

		StreamWriter testLog = new StreamWriter($"Logs//{DateTimeFileName}-TrainingLog.txt");

		static string DateTimeFileName
			=>
				$"{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}-{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second.ToString("D2")}";

		public Form1()
		{
			InitializeComponent();
			testLog.AutoFlush = true;

			day = GetMarketGuess("", days);

			chart1.MouseMove += chart1_MouseMove;
			tooltip.AutomaticDelay = 10;
		}

		static MarketGuess GetMarketGuess(string search, List<MarketGuess> days)
		{
			Random rand = new Random();
			MarketGuess day;
			if (search == "")
			{
				var shortList = days.Where(x => x.MarketDay.DataPoints.Count > 20 && x.FirstVolume.VolumeClass != VolumeClass.veryLow && x.SecondVolume.VolumeClass != VolumeClass.veryLow).ToList();
				int index = rand.Next(0, shortList.Count);
				day = shortList[index];
			}
			else
			{
				string[] splitSearch = search.Split(' ');

				day =
					days.FirstOrDefault(
						x => x.MarketDay.Ticker == splitSearch[0].ToUpper() && x.MarketDay.DateTime == DateTime.Parse(splitSearch[1]));
			}
			return day;
		}

		void PopulateChart(MarketGuess day)
		{
			foreach (Series series in chart1.Series)
			{
				series.Points.Clear();
			}

			//Console.WriteLine(day.MarketDay.ToStringNice());

			//chart1.Series["LocalHL"].Points.Add(new DataPoint(day.ToOA(9, 30), new []{day.LocalHigh, day.LocalLow}));
			//chart1.Series["LocalHL"].Points.Add(new DataPoint(day.ToOA(10, 30), new []{day.LocalHigh, day.LocalLow}));

			chart1.Series["HighBound"].Points.Add(new DataPoint(day.ToOA(9, 30), lh * 0.99));
			chart1.Series["HighBound"].Points.Add(new DataPoint(day.ToOA(10, 30), lh * 0.99));

			chart1.Series["LowBound"].Points.Add(new DataPoint(day.ToOA(9, 30), ll * 1.01));
			chart1.Series["LowBound"].Points.Add(new DataPoint(day.ToOA(10, 30), ll * 1.01));

			if (buyPoint != null)
			{
				chart1.Series["BuyPrice"].Points.Add(new DataPoint(buyPoint.DateTime.ToOADate(), buyPoint.Close));
				chart1.Series["BuyPrice"].Points.Add(new DataPoint(day.ToOA(16, 30), buyPoint.Close));
			}
			if (sellPoint != null)
			{
				chart1.Series["SellPrice"].Points.Add(new DataPoint(sellPoint.DateTime.ToOADate(), sellPoint.Close));
				chart1.Series["SellPrice"].Points.Add(new DataPoint(day.ToOA(16, 30), sellPoint.Close));
			}

			chart1.Series["GapLine"].Points.Add(new DataPoint(day.ToOA(9, 30), day.MarketDay.Open / day.MarketDay.Gap));
			chart1.Series["GapLine"].Points.Add(new DataPoint(day.ToOA(16, 10), day.MarketDay.Open / day.MarketDay.Gap));

			//chart1.Series["ProfitMargin"].Points.Add(new DataPoint(day.ToOA(10, 30), new[] { day.BuyPrice * (1 - MarketGuess.profit), day.BuyPrice * (1 + MarketGuess.profit) }));
			//chart1.Series["ProfitMargin"].Points.Add(new DataPoint(day.ToOA(16, 10), new[] { day.BuyPrice * (1 - MarketGuess.profit), day.BuyPrice * (1 + MarketGuess.profit) }));

			volAvg = 0;
			ll = 999999;
			lh = 0;
			for (int i = 0; i < count; i++)
			{
				//chart1.Series["ProfitMargin"].Points[i].Color = Color.FromArgb(40, chart1.Series["ProfitMargin"].Points[i].Color);
				//chart1.Series["LongPoints"].Points.Add(new DataPoint(day.LongPoints[i].DateTime.ToOADate(), day.LongPoints[i].High));
				//chart1.Series["ShortPoints"].Points.Add(new DataPoint(day.ShortPoints[i].DateTime.ToOADate(), day.ShortPoints[i].Low));
				chart1.Series["Hloc5Min"].Points.Add(
					new DataPoint(
						day.MarketDay.DataPoints[i].DateTime.ToOADate(),
						new[] { day.MarketDay.DataPoints[i].High, day.MarketDay.DataPoints[i].Low, day.MarketDay.DataPoints[i].Open, day.MarketDay.DataPoints[i].Close }));

				chart1.Series["Volume5Min"].Points.Add(new DataPoint(day.MarketDay.DataPoints[i].DateTime.ToOADate(), (double)day.MarketDay.DataPoints[i].Volume / 5));

				volAvg += (double)day.MarketDay.DataPoints[i].Volume/5;
				ll = (day.MarketDay.DataPoints[i].Low < ll) ? day.MarketDay.DataPoints[i].Low : ll;
				lh = (day.MarketDay.DataPoints[i].High > lh) ? day.MarketDay.DataPoints[i].High : lh;
			}
			volAvg /= count;

			if (buyPoint != null)
			{
				if (goingLong)
				{
					profit = 1 - (buyPoint.Close / day.MarketDay.DataPoints[count - 1].Close);
				}
				else
				{
					profit = (buyPoint.Close / day.MarketDay.DataPoints[count - 1].Close) - 1;
				}
			}


			chart1.ChartAreas["VolumeArea"].AlignWithChartArea = "PriceArea";
			chart1.ChartAreas["VolumeArea"].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
			chart1.ChartAreas["VolumeArea"].AlignmentStyle = AreaAlignmentStyles.All;

			chart1.Titles["TitleStockDetails"].Text = $"Time:{day.MarketDay.DataPoints[count-1].DateTime.ToString("t")}\n" +
													  $"Current:{day.MarketDay.DataPoints[count-1].Close}\n" +
													  $"VolAvg:{volAvg}\n" +
			                                          $"Profit:{profit}";

			chart1.Series["Hloc5Min"].ChartType = SeriesChartType.Candlestick;
			//chart1.Series["Hloc5Min"].Points.Clear();

			chart1.ChartAreas["PriceArea"].AxisY.Minimum = ll;
			chart1.ChartAreas["PriceArea"].AxisY.Maximum = lh;
			chart1.ChartAreas["PriceArea"].AxisY2.Minimum = ll / lh;
			chart1.ChartAreas["PriceArea"].AxisY2.Maximum = 1;
			chart1.ChartAreas["PriceArea"].AxisX.Minimum = day.ToOA(9, 30);
			chart1.ChartAreas["PriceArea"].AxisX.Maximum = day.ToOA(16, 10);
			chart1.ChartAreas["VolumeArea"].AxisX.Minimum = day.ToOA(9, 30);
			chart1.ChartAreas["VolumeArea"].AxisX.Maximum = day.ToOA(16, 10);


			chart1.Refresh();
		}

		Point? prevPosition = null;
		ToolTip tooltip = new ToolTip();

		void chart1_MouseMove(object sender, MouseEventArgs e)
		{
			var pos = e.Location;
			if (prevPosition.HasValue && pos == prevPosition.Value)
				return;
			tooltip.RemoveAll();
			prevPosition = pos;
			var results = chart1.HitTest(pos.X, pos.Y, false,
											ChartElementType.DataPoint);
			foreach (var result in results)
			{
				if (result.ChartElementType == ChartElementType.DataPoint)
				{
					var prop = result.Object as DataPoint;
					if (prop != null)
					{
						double pointXPixel = result.ChartArea.AxisX.ValueToPixelPosition(prop.XValue);
						double pointYPixel = result.ChartArea.AxisY.ValueToPixelPosition(prop.YValues[0]);

						// check if the cursor is really close to the point (2 pixels around the point)
						if (Math.Abs(pos.X - pointXPixel) < 5 &&
							Math.Abs(pos.Y - pointYPixel) < 5)
						{
							if (prop.YValues.Length == 4)
							{
								tooltip.Show(
									$"{DateTime.FromOADate(prop.XValue).ToString("t")}\nH:{prop.YValues[0]}\nL:{prop.YValues[1]}\nO:{prop.YValues[2]}\nC:{prop.YValues[3]}",
									this.chart1,
									pos.X + 15,
									pos.Y - 15);
							}
							else
							{
								tooltip.Show($"{DateTime.FromOADate(prop.XValue).ToString("t")}\nVol:{prop.YValues[0]}", this.chart1, pos.X + 15, pos.Y - 15);
							}
						}
					}
				}
			}
		}

		private void nextDataButton_Click(object sender, EventArgs e)
		{
			if (count < day.MarketDay.DataPoints.Count)
			{
				count++;
				PopulateChart(day);
			}
		}

		private void newButton_Click(object sender, EventArgs e)
		{
			count = 1;
			buyPoint = null;
			profit = 0;

			rakeInButton.Enabled = false;
			goLongButton.Enabled = true;
			shortButton.Enabled = true;
			noTouchButton.Enabled = true;

			day = GetMarketGuess("", days);
			PopulateChart(day);
		}

		private void noTouchButton_Click(object sender, EventArgs e)
		{
			string comment = Interaction.InputBox("Comment");

			testLog.WriteLine(day.ToString());
			testLog.WriteLine($"Did not touch");
			testLog.WriteLine($"Commment:{comment}");
			testLog.WriteLine("----------------------------------------");

			rakeInButton.Enabled = false;
			goLongButton.Enabled = false;
			shortButton.Enabled = false;
			noTouchButton.Enabled = false;

			count = day.MarketDay.DataPoints.Count;
			PopulateChart(day);
		}

		private void rakeInButton_Click(object sender, EventArgs e)
		{
			string comment = Interaction.InputBox("Comment");
			sellPoint = day.MarketDay.DataPoints[count - 1];

			testLog.WriteLine(day.ToString());
			testLog.WriteLine(goingLong ? "Went Long" : "Shorted");
			testLog.WriteLine($"BuyPoint:{buyPoint.ToStringNice()}");
			testLog.WriteLine($"SellPoint:{sellPoint.ToStringNice()}");
			testLog.WriteLine($"Profit:{profit}");
			testLog.WriteLine($"Commment:{comment}");
			testLog.WriteLine("----------------------------------------");

			rakeInButton.Enabled = false;
			goLongButton.Enabled = false;
			shortButton.Enabled = false;
			noTouchButton.Enabled = false;

			totalProfit += profit;
			totalCount++;

			count = day.MarketDay.DataPoints.Count;
			PopulateChart(day);
		}

		private void goLongButton_Click(object sender, EventArgs e)
		{
			if (buyPoint == null)
			{
				rakeInButton.Enabled = true;
				goLongButton.Enabled = false;
				shortButton.Enabled = false;
				noTouchButton.Enabled = false;

				buyPoint = day.MarketDay.DataPoints[count - 1];
				goingLong = true;

				PopulateChart(day);
			}
		}

		private void shortButton_Click(object sender, EventArgs e)
		{
			if (buyPoint == null)
			{
				rakeInButton.Enabled = true;
				goLongButton.Enabled = false;
				shortButton.Enabled = false;
				noTouchButton.Enabled = false;

				buyPoint = day.MarketDay.DataPoints[count - 1];
				goingLong = false;
			
				PopulateChart(day);
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			testLog.WriteLine();
			testLog.WriteLine("------------");
			testLog.WriteLine("Summary:");
			testLog.WriteLine($"Profit:{totalProfit/totalCount}");
			testLog.WriteLine($"Count:{totalCount}");
		}
	}
}
