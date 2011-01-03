using System;
using System.Diagnostics;
using System.Threading;
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
			var multithreaded = ReadMode(args);
			var tests = GetTestSubjects();
			// warmup
			foreach (var paulTest in tests)
			{
				Run(paulTest, 1, false);
			}

			//so that we don't reuse singletons from the warmup run
			tests = GetTestSubjects();
			Console.WriteLine("Running {0} times...", iterations);
			switch (multithreaded)
			{
				case Mode.Thread_based:
					Console.WriteLine("...on multiple threads (using TPL Tasks)");
					foreach (var paulTest in tests)
					{
						RunTaskBased(paulTest, iterations);
					}
					break;
				case Mode.Task_based:
					Console.WriteLine("...on multiple threads (using {0} Threads)", Environment.ProcessorCount);
					foreach (var paulTest in tests)
					{
						RunThreadBased(paulTest, iterations);
					}
					break;
				default:
					foreach (var paulTest in tests)
					{
						Run(paulTest, iterations, true);
					}
					break;
			}
		}

		private static void RunThreadBased(IPaulTest test, int count)
		{
			var stopwatch = Stopwatch.StartNew();
			var threads = new Thread[Environment.ProcessorCount];
			var interval = count/threads.Length;
			for (var i = 0; i < threads.Length; i++)
			{
				var thread = new Thread(() =>
				                        	{
				                        		for (var j = 0; j < interval; j++)
				                        		{
				                        			var player = test.ResolvePlayer();
				                        			player.Shoot();
				                        		}
				                        	});
				thread.Name = test.GetType().Name + " " + (i + 1);
				threads[i] = thread;
				thread.Start();
			}
			for (var i = 0; i < threads.Length; i++)
			{
				threads[i].Join();
			}
			Console.WriteLine(" Hey {0} - you did it in {1}", test.GetType().Name, stopwatch.Elapsed);
		}

		private static IPaulTest[] GetTestSubjects()
		{
			return new IPaulTest[]
			       	{
			       		new CustomContainer(),
			       		new Windsor(),
			       		new Windsor_delegates(),
			       		new Autofac(),
			       		new Autofac_delegates(),
			       		new Unity(),
			       		new StructureMap(),
			       		new Linfu(),
			       		new Funq(),
			       		new Ninject()
			       	};
		}

		private static void RunTaskBased(IPaulTest test, int count)
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

		private static Mode ReadMode(string[] args)
		{
			var isMultithreaded = Array.IndexOf(args, "m") != -1;
			if (isMultithreaded)
			{
				return Mode.Task_based;
			}
			var isThreadBased = Array.IndexOf(args, "t") != -1;
			if (isThreadBased)
			{
				return Mode.Thread_based;
			}
			return Mode.Single_threaded;
		}

		private static void Run(IPaulTest test, int count, bool measure)
		{
			Stopwatch stopwatch = null;
			if (measure)
			{
				stopwatch = Stopwatch.StartNew();
			}
			for (var i = 0; i < count; i++)
			{
				var player = test.ResolvePlayer();
				player.Shoot();
			}
			if (stopwatch != null)
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

	public enum Mode
	{
		Single_threaded,
		Task_based,
		Thread_based
	}
}