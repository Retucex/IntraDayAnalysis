using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace IntradayAnalysis.Charts
{
	public partial class VolumeShortLong : Form
	{
		public VolumeShortLong(List<MarketGuess> guesses)
		{
			InitializeComponent();
			int count = 0;
			foreach (var marketGuess in guesses.GroupBy(x => x.BoundsMarketAction))
			{
				foreach (MarketGuess guess in marketGuess)
				{
					count++;
					double longShort = 0;
					if (guess.LongPoints.Count > guess.ShortPoints.Count)
					{
						longShort = 1;
					}
					else
					{
						longShort = -1;
					}
					chart1.Series["VolumeSerie"].Points.Add(new DataPoint(count, new[] { (double)guess.SecondVolume.Volume, (double)guess.FirstVolume.Volume, longShort }));
				}
				
			}

			foreach (var point in chart1.Series["VolumeSerie"].Points)
			{
				if (point.YValues[0] < point.YValues[1])
				{
					point.Color = Color.DarkRed;
				}

				if (point.YValues[2] < 0)
				{
					point.MarkerColor = Color.Red;
				}
				if (point.YValues[2] > 0)
				{
					point.MarkerColor = Color.Green;
				}
			}
		}
	}
}
