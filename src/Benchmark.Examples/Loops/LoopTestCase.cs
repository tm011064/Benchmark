using Benchmark;
using System.Linq;

namespace Benchmark.Examples.Loops
{
  public class LoopTestCase : ICandidateTestCase
  {
    public LoopTestCase(ObservableObject[] items, int numberOfObjectCalls)
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
