using System.Linq;

namespace Benchmark.Examples.BenchmarkCandidatesWithContexts
{
  public class WhileLoopCandidate : IBenchmarkCandidate<LoopContext>
  {
    public string Name { get; } = "While-Loop";

    public void Run(LoopContext parameters)
    {
      var count = parameters.Items.Count();
      var i = -1;

      while (++i < count)
      {
        for (var j = 0; j < parameters.NumberOfObjectCalls; j++)
        {
          parameters.Items[i].Observe();
        }
      }
    }
  }
}
