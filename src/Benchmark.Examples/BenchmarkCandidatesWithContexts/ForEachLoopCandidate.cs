using System.Diagnostics;

namespace Benchmark.Examples.BenchmarkCandidatesWithContexts
{
  public class ForEachLoopCandidate : IBenchmarkCandidate<LoopContext>
  {
    public string Name { get; } = "For-Each-Loop";

    public void Run(LoopContext context)
    {
      foreach (var item in context.Items)
      {
        for (var j = 0; j < context.NumberOfObjectCalls; j++)
        {
          item.Observe();
        }
      }
    }
  }
}
