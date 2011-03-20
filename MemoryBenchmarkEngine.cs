using System;
using System.Text.RegularExpressions;

namespace PaulBenchmark
{
	public class MemoryBenchmarkEngine : BenchmarkEngine
	{
		private int? memorySnapshotStep;

		public MemoryBenchmarkEngine(int? memorySnapshotStep, int iterations, Regex filter)
		{
			base.filter = filter;
			base.iterations = iterations;
			this.memorySnapshotStep = memorySnapshotStep;
		}

		protected override void Go()
		{
			Console.WriteLine("Running {0} times", iterations);
			var tests = GetTestSubjects();
			if(memorySnapshotStep.HasValue && memorySnapshotStep.Value < iterations)
			{
				Console.WriteLine("...stoping every {0} iterations", memorySnapshotStep.Value);
				foreach (var paulTest in tests)
				{
					Run(paulTest, RunWithStop);
				}
			}
			else
			{
				foreach (var paulTest in tests)
				{
					Run(paulTest, RunContinuously);
				}
			}
		}

		private long RunContinuously(IPaulTest test)
		{
			Console.WriteLine(" Testing {0,-20}", test.GetType().Name);
			for (var i = 0; i < iterations; i++)
			{
				var player = test.ResolvePlayer();
				player.Shoot();
			}
			PrintResult(test);
			var disposable = test as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			return 0;
		}

		private void PrintResult(IPaulTest test)
		{
			Console.WriteLine(" Hey {0,-20} - you're done. Collect snapshot and proceed with enter", test.GetType().Name);
			Console.ReadLine();
		}

		private long RunWithStop(IPaulTest test)
		{
			Console.WriteLine(" Testing {0,-20}", test.GetType().Name);
			var count = 0;
			for (var i = 0; i < iterations; i++)
			{
				count++;
				var player = test.ResolvePlayer();
				player.Shoot();
				if(count ==memorySnapshotStep)
				{
					CollectSnapshot(i);
					count = 0;
				}
			}
			PrintResult(test);
			var disposable = test as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			return 0;
		}

		private void CollectSnapshot(int i)
		{
			Console.WriteLine(" We're {0} out of {1} iterations into the test. Collect your snapshot", i, iterations);
			Console.ReadLine();
		}
	}
}