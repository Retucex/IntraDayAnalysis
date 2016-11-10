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
		static readonly int[] lowVolume = { 1000, 3999 };
		static readonly int[] mediumVolume = { 4000, 34999 };
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
	}
}
