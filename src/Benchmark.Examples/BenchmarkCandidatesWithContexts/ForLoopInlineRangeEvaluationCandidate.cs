namespace Benchmark.Examples.BenchmarkCandidatesWithContexts
{
  public class ForLoopInlineRangeEvaluationCandidate : IBenchmarkCandidate<LoopContext>
  {
    public string Name { get; } = "For-Loop-Inline-Range-Evaluation";

    public void Run(LoopContext parameters)
    {
      for (var i = 0; i < parameters.Items.Length; i++)
      {
        for (var j = 0; j < parameters.NumberOfObjectCalls; j++)
        {
          parameters.Items[i].Observe();
        }
      }
    }
  }
}
