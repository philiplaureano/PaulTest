using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PaulBenchmark
{
	internal class Program
	{
		private static int defaultIterationsCount = 100000;

		private static void Main(string[] args)
		{
			// calling with 'c 100' will execute 100 iterations. adding 'm' will execute on many threads (using TPL Tasks)
			var iterations = GetIterationsCount(args);
			var multithreaded = GetIsMultithreaded(args);
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
			if(multithreaded)
			{
				Console.WriteLine("...on multiple threads");
				foreach (var paulTest in tests)
				{
					RunMultithreaded(paulTest, iterations);
				}
			}
			else
			{
				foreach (var paulTest in tests)
				{
					Run(paulTest, iterations, true);
				}
			}

		}

		private static void RunMultithreaded(IPaulTest test, int count)
		{
			var stopwatch = Stopwatch.StartNew();
			var tasks = new Task[count];
			for (var i = 0; i < count; i++)
			{
				tasks[i] = Task.Factory.StartNew(() =>
				                                 	{
				                                 		var player = test.ResolvePlayer();
				                                 		player.Shoot();
				                                 	});
			}
			Task.WaitAll(tasks);
			Console.WriteLine(" Hey {0} - you did it in {1}", test.GetType().Name, stopwatch.Elapsed);
			
		}

		private static bool GetIsMultithreaded(string[] args)
		{
			var isMultithreaded = Array.IndexOf(args, "m") != -1;
			return isMultithreaded;
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
			if(stopwatch != null)
			{
				Console.WriteLine(" Hey {0} - you did it in {1}", test.GetType().Name, stopwatch.Elapsed);
			}
		}

		private static int GetIterationsCount(string[] args)
		{
			var indexOfCount = Array.IndexOf(args, "c");

			if (indexOfCount < 0 || indexOfCount == args.Length - 1)
			{
				return defaultIterationsCount;
			}
			return int.Parse(args[indexOfCount + 1]);
		}
	}
}