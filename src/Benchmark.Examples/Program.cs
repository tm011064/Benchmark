using Benchmark.Examples.LambdaActions;
using System;

namespace Benchmark.Examples
{
  class Program
  {
    static void Main(string[] args)
    {
      // TODO (Roman): allow multiple stopwatches
      // TODO (Roman): allow automatic run number
      // TODO (Roman): make observable object public
      var report = NamedActionBenchmark.Run();

      Console.Write(report.ToString());

      Console.ReadKey();
    }
  }
}
