using Benchmark;

namespace Benchmark.Examples.Loops
{
  public class ForEachLoop : AbstractCandidate<LoopTestCase>
  {
    public override string Name { get; } = "For-Each-Loop";

    protected override void Test(LoopTestCase parameters)
    {
      foreach (var item in parameters.Items)
      {
        for (var j = 0; j < parameters.NumberOfObjectCalls; j++)
        {
          item.Observe();
        }
      }
    }
  }
}
