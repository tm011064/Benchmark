using System.Linq;

namespace Benchmark.Examples.BenchmarkCandidatesWithContexts
{
  public class LoopContext : IBenchmarkContext
  {
    public LoopContext(ObservableObject[] items, int numberOfObjectCalls)
    {
      Items = items;
      NumberOfObjectCalls = numberOfObjectCalls;
    }

    public ObservableObject[] Items { get; }

    public int NumberOfObjectCalls { get; }

    public string Description =>
        $"{Items.Count()} objects, {NumberOfObjectCalls} calls";
  }
}
