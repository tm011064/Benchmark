using Benchmark;
using System.Linq;

namespace Benchmark.Examples.Loops
{
  public class WhileLoop : AbstractCandidate<LoopTestCase>
  {
    public override string Name { get; } = "While-Loop";

    protected override void Test(LoopTestCase parameters)
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
