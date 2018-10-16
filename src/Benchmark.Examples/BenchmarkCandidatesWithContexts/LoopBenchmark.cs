using System.Linq;

namespace Benchmark.Examples.BenchmarkCandidatesWithContexts
{
  public static class LoopBenchmark
  {
    public static BenchmarkReport Run()
    {
      var items = Enumerable.Range(0, 1000)
        .Select(_ => new ObservableObject())
        .ToArray();

      return Measure<LoopContext>
        .Candidates<WhileLoopCandidate, ForLoopCandidate, ForEachLoopCandidate, ForLoopInlineRangeEvaluationCandidate>()
        .WithContexts(
          new LoopContext(items.ToArray(), 0),
          new LoopContext(items.ToArray(), 1),
          new LoopContext(items.ToArray(), 10))
        .NumberOfRuns(100)
        .NumberOfWarmUpRuns(10, new LoopContext(items.Take(1).ToArray(), 1))
        .Go();
    }
  }
}
