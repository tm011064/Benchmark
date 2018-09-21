using Benchmark.Examples.Loops;
using Benchmark.Writers;
using System;
using System.Linq;

namespace Benchmark.Examples
{
  class Program
  {
    static void Main(string[] args)
    {
      var items = Enumerable.Range(0, 10000)
        .Select(_ => new ObservableObject())
        .ToArray();

      var results = Measure<LoopTestCase>
        .Candidates(
          new WhileLoop(),
          new ForLoop(),
          new ForEachLoop())
        .WithTestCases(
          new LoopTestCase(items.Take(1000).ToArray(), 0),
          new LoopTestCase(items.Take(1000).ToArray(), 1),
          new LoopTestCase(items.Take(1000).ToArray(), 10),
          new LoopTestCase(items, 0),
          new LoopTestCase(items, 5))
        .WithNumberOfRuns(300)
        .WithNumberOfDryRuns(10, new LoopTestCase(items.Take(1).ToArray(), 1))
        .Go();

      var writer = new BenchmarkResultConsoleWriter(results, RankMode.Median);
      var output = writer.Write();

      Console.Write(output);

      Console.ReadKey();
    }
  }
}
