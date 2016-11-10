using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

namespace IntradayAnalysis
{
	class Program
	{
		static void Main(string[] args)
		{
			//MarketAnalysis.CreateGapFile();
			MarketAnalysis.RunSimulation();

			Console.WriteLine("Press a key to exit...");
			//Console.ReadKey();
		}
	}
}
