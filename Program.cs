using System;
using System.IO;

namespace PaulBenchmark
{
    internal class Program
    {
        private static void Main(string[] args)
        {
#if DEBUG
            args = new[]
					{   "t",
						"c",
						"50000",
						"r"
					};
#else
			if(args == null || args.Length==0)
			{
				ShowHelp();
				return;
			}
#endif
            using (var benchmark = new BenchmarkFactory(args).Create())
            {
                benchmark.Run();
            }
        }

        private static void ShowHelp()
        {
            using (var reader = new StreamReader(typeof(Program).Assembly.GetManifestResourceStream("PaulBenchmark.help.txt")))
            {
                Console.WriteLine(reader.ReadToEnd());
            }
        }
    }
}