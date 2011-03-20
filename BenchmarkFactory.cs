using System;
using System.Text.RegularExpressions;

namespace PaulBenchmark
{
	public class BenchmarkFactory
	{
		private readonly string[] arguments;
		private readonly int defaultIterations;

		public BenchmarkFactory(string[] arguments, int defaultIterations = 100000)
		{
			this.arguments = arguments;
			this.defaultIterations = defaultIterations;
		}

		public BenchmarkEngine Create()
		{
			var profilingMode = ReadProfilingMode();
			var memorySnapshotStep = ReadMemorySnapshotStep();
			var mode = ReadMode();
			var iterations = ReadIterationsCount();
			var filter = ReadFilters();
			var results = ReadResultsForm();
			var output = ReadOutputFile();
			if(profilingMode == ProfilingMode.Perf)
			{
				Console.WriteLine("Performance run: {0}\t{1}\t{2}{3}\tresults displayed as {4}",
				                  mode,
				                  iterations,
				                  ((object) filter ?? "no filter"),
				                  ("\t" + (output ?? "no output file")),
				                  results.ToString().Replace("_", " "));

				var engine = new PerformanceBenchmarkEngine(
					mode,
					iterations,
					filter,
					output,
					results);
				return engine;
			}
			Console.WriteLine("Memory run: {0} iterations {1}\t{1}\t{2}",
			                  iterations,
			                  (memorySnapshotStep.HasValue?"stop every "+memorySnapshotStep.Value:""),
			                  ((object)filter ?? "no filter"));
			return new MemoryBenchmarkEngine(memorySnapshotStep, iterations, filter);
		}

		private int? ReadMemorySnapshotStep()
		{
			var step = ReadValue<int>("p");
			if (step == 0)
				return null;
			return step;
		}

		private ProfilingMode ReadProfilingMode()
		{
			if(ReadFlag("p"))
			{
				return ProfilingMode.Memory;
			}
			return ProfilingMode.Perf;
		}

		private Results ReadResultsForm()
		{
			if(ReadFlag("r"))
			{
				return Results.Rate_per_second;
			}
			return Results.Total_time;
		}

		private string ReadOutputFile()
		{
			return ReadValue<string>("o");
		}

		private int ReadIterationsCount()
		{
			return ReadValue("c", defaultIterations);
		}


		private Mode ReadMode()
		{
			if (ReadFlag("m"))
			{
				return Mode.Task_based;
			}
			if (ReadFlag("t"))
			{
				return Mode.Thread_based;
			}
			return Mode.Single_threaded;
		}

		private bool ReadFlag(string name)
		{
			return Array.IndexOf(arguments, name) != -1;
		}

		private T ReadValue<T>(string name, T defaultValue = default(T))
		{
			var indexOfFlag = Array.IndexOf(arguments, name);

			if (indexOfFlag < 0 || indexOfFlag == arguments.Length - 1)
			{
				return defaultValue;
			}
			var indexOfValue = indexOfFlag + 1;
			var stringValue = arguments[indexOfValue];
			try
			{
				return (T) Convert.ChangeType(stringValue, typeof (T));
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}

		private Regex ReadFilters()
		{
			var pattern = ReadValue<string>("f");
			if (pattern == null)
			{
				return null;
			}
			return new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
		}
	}
}