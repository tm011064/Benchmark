namespace Benchmark.Examples.BenchmarkCandidatesWithoutContext
{
  public static class StringConcatenationBenchmark
  {
    public static BenchmarkReport Run()
    {
      return Measure
        .Candidates<ConcatenateStringsCandidate, StringBuilderCandidate>()
        .NumberOfRuns(300)
        .NumberOfWarmUpRuns(10)
        .Go();
    }
  }
}
