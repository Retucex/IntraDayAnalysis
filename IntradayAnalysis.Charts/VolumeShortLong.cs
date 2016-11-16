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
		public VolumeShortLong()
		{
			InitializeComponent();
			int count = 0;
			foreach (var marketGuess in MainCandleStick.days.GroupBy(x => x.FirstVolume.VolumeClass))
			{
				foreach (var guess in marketGuess.ToList().GroupBy(x => x.SecondVolume.VolumeClass))
				{
					foreach (var marketGuess1 in guess)
					{
						chart1.Series["VolumeSerie"].Points.Add(new DataPoint(count, new[] { (double)marketGuess1.FirstVolume.Volume, (double)marketGuess1.SecondVolume.Volume }));
						count++;
					}
				}
				
			}

			foreach (var point in chart1.Series["VolumeSerie"].Points)
			{
				if (point.YValues[0] < point.YValues[1])
				{
					point.Color = Color.Red;
				}
			}
		}
	}
}
