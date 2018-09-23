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
      var report = NamedActionBenchmark.Run();

      Console.Write(report.ToString());
      Console.WriteLine();
      Console.Write(report.ToMarkuo());
      Console.WriteLine();
      Console.Write(report.ToCsv());

      Console.ReadKey();
    }
  }
}
