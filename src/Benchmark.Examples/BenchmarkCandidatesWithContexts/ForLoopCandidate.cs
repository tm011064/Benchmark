using System.Linq;

namespace Benchmark.Examples.BenchmarkCandidatesWithContexts
{
  public class ForLoopCandidate : IBenchmarkCandidate<LoopContext>
  {
    public string Name { get; } = "For-Loop";

    public void Run(LoopContext parameters)
    {
      var count = parameters.Items.Count();
      for (var i = 0; i < count; i++)
      {
        for (var j = 0; j < parameters.NumberOfObjectCalls; j++)
        {
          parameters.Items[i].Observe();
        }
      }
    }
  }
}
