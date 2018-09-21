using Benchmark;
using System.Linq;

namespace Benchmark.Examples.Loops
{
  public class ForLoop : AbstractCandidate<LoopTestCase>
  {
    public override string Name { get; } = "For-Loop";

    protected override void Test(LoopTestCase parameters)
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
