using Benchmark.Examples.LambdaActions;
using System;

namespace Benchmark.Examples
{
  class Program
  {
    static void Main(string[] args)
    {
      //var report = LoopBenchmarkRunner.Run();
      //var report = StringConcatenationBenchmark.Run();
      //var report = NamedActionBenchmark.Run();
      var report = UnnamedActionBenchmark.Run();

      Console.Write(report.ToString());
      Console.WriteLine();
      Console.Write(report.ToMarkupTable());
      Console.WriteLine();
      Console.Write(report.ToCsv());

      Console.ReadKey();
    }
  }
}
