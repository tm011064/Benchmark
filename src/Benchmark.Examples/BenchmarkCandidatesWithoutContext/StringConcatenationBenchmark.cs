namespace Benchmark.Examples.BenchmarkCandidatesWithoutContext
{
  public static class StringConcatenationBenchmark
  {
    public static BenchmarkReport Run()
    {
      return Measure
        .Candidates<ConcatenateStringsCandidate, StringBuilderCandidate>()
        .WithNumberOfRuns(300)
        .WithNumberOfWarmUpRuns(10)
        .Go();
    }
  }
}
