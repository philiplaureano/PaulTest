using System;
using System.IO;
using System.Text.RegularExpressions;

namespace PaulBenchmark
{
    public abstract class BenchmarkEngine : IDisposable
    {
        protected TextWriter output = TextWriter.Null;
        protected Regex filter;
        protected int iterations;

        protected static IPaulTest[] GetTestSubjects()
        {
            return new IPaulTest[]
			       	{
			       		new CustomContainer(),
                        new Hiro(),
                        new Funq(),
			       		new Windsor(),
			       		new Windsor_delegates(),
			       		new Autofac(),
			       		new Autofac_delegates(),
			       		new Unity(),
			       		new StructureMap(),
			       		new Linfu(),
			       		new Ninject(),			       		
			       	};
        }

        public void Run()
        {
            Warmup();
            Go();

            return;
        }

        protected abstract void Go();

        protected void Run(IPaulTest paulTest, Func<IPaulTest, long> function)
        {
            if (ShouldRun(filter, paulTest))
            {
                output.Write(function(paulTest) + "\t");
            }
            else
            {
                output.Write("0\t");
            }
        }

        private static bool ShouldRun(Regex filter, IPaulTest paulTest)
        {
            if (filter == null) return true;
            var name = paulTest.GetType().Name;
            var shouldRun = filter.IsMatch(name);
            return shouldRun;
        }

        private void Warmup()
        {
            foreach (var paulTest in GetTestSubjects())
            {
                var player = paulTest.ResolvePlayer();
                player.Shoot();
            }
        }

        public virtual void Dispose()
        {
            if (output != null)
            {
                output.WriteLine();
                output.Dispose();
                output = null;
            }
        }
    }
}