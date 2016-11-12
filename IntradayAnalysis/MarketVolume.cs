using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntradayAnalysis
{
	public enum VolumeClass { veryLow, low, medium, high, veryHigh }
	class MarketVolume
	{
		static readonly int[] veryLowVolume = { 0, 999 };
		static readonly int[] lowVolume = { 1000, 4999 };
		static readonly int[] mediumVolume = { 5000, 34999 };
		static readonly int[] highVolume = { 35000, 124999 };
		static readonly int[] veryHighVolume = { 125000, 99999999 };

		public VolumeClass VolumeClass { get; private set; }

		int volume;
		public int Volume
		{
			get
			{
				return volume;
			}
			set
			{
				volume = value;

				if (Volume >= veryLowVolume[0] && Volume <= veryLowVolume[1])
				{
					VolumeClass = VolumeClass.veryLow;
				}
				if (Volume >= lowVolume[0] && Volume <= lowVolume[1])
				{
					VolumeClass = VolumeClass.low;
				}
				if (Volume >= mediumVolume[0] && Volume <= mediumVolume[1])
				{
					VolumeClass = VolumeClass.medium;
				}
				if (Volume >= highVolume[0] && Volume <= highVolume[1])
				{
					VolumeClass = VolumeClass.high;
				}
				if (Volume >= veryHighVolume[0] && Volume <= veryHighVolume[1])
				{
					VolumeClass = VolumeClass.veryHigh;
				}
			}
		}

		public MarketVolume()
		{
			Volume = 0;
		}

		public MarketVolume(int volume)
		{
			Volume = volume;
		}

		public static string OutputVolumeClasses()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"VeryLow: {veryLowVolume[0]}-{veryLowVolume[1]}");
			sb.AppendLine($"Low: {lowVolume[0]}-{lowVolume[1]}");
			sb.AppendLine($"Medium: {mediumVolume[0]}-{mediumVolume[1]}");
			sb.AppendLine($"High: {highVolume[0]}-{highVolume[1]}");
			sb.AppendLine($"VeryHigh: {veryHighVolume[0]}-{veryHighVolume[1]}");

			return sb.ToString();
		}
	}
}
