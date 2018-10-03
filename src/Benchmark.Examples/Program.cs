using Benchmark.Examples.LambdaActions;
using System;

namespace Benchmark.Examples
{
  class Program
  {
    static void Main(string[] args)
    {
      // TODO (Roman): allow multiple stopwatches
      // TODO (Roman): make observable object public
      var ts = TimeSpan.FromSeconds(5);
      var rr = ts - TimeSpan.FromSeconds(100);

      var report = NamedActionBenchmark.Run();

      Console.Write(report.ToString());

      Console.ReadKey();
    }
  }
}
