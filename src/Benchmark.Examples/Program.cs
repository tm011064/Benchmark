using Benchmark.Examples.LambdaActions;
using System;

namespace Benchmark.Examples
{
  class Program
  {
    static void Main(string[] args)
    {
      var report = NamedActionBenchmark.Run();

      Console.Write(report.ToString());

      Console.ReadKey();
    }
  }
}
