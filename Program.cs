using System;
using System.Diagnostics;

namespace PaulBenchmark
{
	internal class Program
	{
		private static int defaultIterationsCount = 100000;

		private static void Main(string[] args)
		{
			var iterations = GetIterationsCount(args);
			IPaulTest[] tests = {
			                    	new CustomContainer(),
			                    	new Windsor(),
			                    	new Autofac(),
			                    	new Unity(),
			                    	new Ninject(),
			                    	new StructureMap()
			                    };
			// warmup
			foreach (var paulTest in tests)
			{
				Run(paulTest, 1, false);
			}

			Console.WriteLine("Running {0} times...", iterations);
			foreach (var paulTest in tests)
			{
				Run(paulTest, iterations, true);
			}

		}

		private static void Run(IPaulTest test, int count, bool measure)
		{
			Stopwatch stopwatch = null;
			if(measure)
			{
				stopwatch = Stopwatch.StartNew();
			}
			for (var i = 0; i < count; i++)
			{
				var player = test.ResolvePlayer();
				player.Shoot();
			}
			if(stopwatch!=null)
			{
				Console.WriteLine(" Hey {0} - you did it in {1}", test.GetType().Name, stopwatch.Elapsed);
			}
		}

		private static int GetIterationsCount(string[] args)
		{
			var indexOfCount = Array.IndexOf(args, "c");

			if (indexOfCount < 0 && indexOfCount == args.Length - 1)
			{
				return defaultIterationsCount;
			}
			return int.Parse(args[indexOfCount + 1]);
		}
	}
}