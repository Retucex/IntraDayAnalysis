﻿namespace IntradayAnalysis.Charts
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Linq;
	using System.Windows.Forms;
	using System.Windows.Forms.DataVisualization.Charting;

	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			List<MarketGuess> days = MarketAnalysis.RunSimulation();
			string search = Console.ReadLine();
			MarketGuess day;
			if (search == "")
			{
				day = days.FirstOrDefault(x => x.MarketDay.Ticker == "pfpt".ToUpper() && x.MarketDay.DateTime == DateTime.Parse("2016-10-21"));
			}
			else
			{
				string[] splitSearch = search.Split(' ');

				day = days.FirstOrDefault(x => x.MarketDay.Ticker == splitSearch[0].ToUpper() && x.MarketDay.DateTime == DateTime.Parse(splitSearch[1]));
			}

			chart1.MouseMove += chart1_MouseMove;
			tooltip.AutomaticDelay = 10;

			chart1.ChartAreas["VolumeArea"].AlignWithChartArea = "PriceArea";
			chart1.ChartAreas["VolumeArea"].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
			chart1.ChartAreas["VolumeArea"].AlignmentStyle = AreaAlignmentStyles.All;

			chart1.Titles["TitleStockDetails"].Text = day.ToString();

			chart1.Series["Hloc5Min"].ChartType = SeriesChartType.Candlestick;
			chart1.Series["Hloc5Min"].Points.Clear();

			chart1.ChartAreas["PriceArea"].AxisY.Minimum = day.MarketDay.Low * 0.999;
			chart1.ChartAreas["PriceArea"].AxisY.Maximum = day.MarketDay.High * 1.001;
			chart1.ChartAreas["PriceArea"].AxisX.Minimum = day.ToOA(9, 30);
			chart1.ChartAreas["PriceArea"].AxisX.Maximum = day.ToOA(16, 10);
			chart1.ChartAreas["VolumeArea"].AxisX.Minimum = day.ToOA(9, 30);
			chart1.ChartAreas["VolumeArea"].AxisX.Maximum = day.ToOA(16, 10);

			PopulateChart(day);
		}

		void PopulateChart(MarketGuess day)
		{
			Console.WriteLine(day.MarketDay.ToStringNice());

			chart1.Series["LocalHL"].Points.Add(new DataPoint(day.ToOA(9, 30), new []{day.LocalHigh, day.LocalLow}));
			chart1.Series["LocalHL"].Points.Add(new DataPoint(day.ToOA(10, 30), new []{day.LocalHigh, day.LocalLow}));

			chart1.Series["HighBound"].Points.Add(new DataPoint(day.ToOA(9, 30), day.HighBound));
			chart1.Series["HighBound"].Points.Add(new DataPoint(day.ToOA(10, 30), day.HighBound));

			chart1.Series["LowBound"].Points.Add(new DataPoint(day.ToOA(9, 30), day.LowBound));
			chart1.Series["LowBound"].Points.Add(new DataPoint(day.ToOA(10, 30), day.LowBound));

			chart1.Series["BuyPrice"].Points.Add(new DataPoint(day.ToOA(9, 30), day.BuyPrice));
			chart1.Series["BuyPrice"].Points.Add(new DataPoint(day.ToOA(10, 30), day.BuyPrice));

			chart1.Series["GapLine"].Points.Add(new DataPoint(day.ToOA(9, 30), day.MarketDay.Open / day.MarketDay.Gap));
			chart1.Series["GapLine"].Points.Add(new DataPoint(day.ToOA(16, 10), day.MarketDay.Open / day.MarketDay.Gap));

			chart1.Series["LongProfit"].Points.Add(new DataPoint(day.ToOA(10, 30), new[] { chart1.ChartAreas["PriceArea"].AxisY.Maximum, day.BuyPrice * (1 + MarketGuess.profit) }));
			chart1.Series["LongProfit"].Points.Add(new DataPoint(day.ToOA(16, 10), new[] { chart1.ChartAreas["PriceArea"].AxisY.Maximum, day.BuyPrice * (1 + MarketGuess.profit) }));

			chart1.Series["ShortProfit"].Points.Add(new DataPoint(day.ToOA(10, 30), day.BuyPrice * (1 - MarketGuess.profit)));
			chart1.Series["ShortProfit"].Points.Add(new DataPoint(day.ToOA(16, 10), day.BuyPrice * (1 - MarketGuess.profit)));

			foreach (DataPoint dataPoint in chart1.Series["LongProfit"].Points)
			{
				dataPoint.Color = Color.FromArgb(40, dataPoint.Color);
			}

			foreach (DataPoint dataPoint in chart1.Series["ShortProfit"].Points)
			{
				dataPoint.Color = Color.FromArgb(40, dataPoint.Color);
			}

			foreach (DataPoint dataPoint in chart1.Series["LocalHL"].Points)
			{
				dataPoint.Color = Color.FromArgb(40, dataPoint.Color);
			}

			foreach (MarketDataPoint marketDataPoint in day.LongPoints)
			{
				chart1.Series["LongPoints"].Points.Add(new DataPoint(marketDataPoint.DateTime.ToOADate(), marketDataPoint.High));
			}

			foreach (MarketDataPoint marketDataPoint in day.ShortPoints)
			{
				chart1.Series["ShortPoints"].Points.Add(new DataPoint(marketDataPoint.DateTime.ToOADate(), marketDataPoint.Low));
			}

			foreach (MarketDataPoint marketDataPoint in day.MarketDay.DataPoints)
			{
				chart1.Series["Hloc5Min"].Points.Add(
					new DataPoint(
						marketDataPoint.DateTime.ToOADate(),
						new[] { marketDataPoint.High, marketDataPoint.Low, marketDataPoint.Open, marketDataPoint.Close }));

				chart1.Series["Volume5Min"].Points.Add(new DataPoint(marketDataPoint.DateTime.ToOADate(), (double)marketDataPoint.Volume/5));
			}
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
	}
}