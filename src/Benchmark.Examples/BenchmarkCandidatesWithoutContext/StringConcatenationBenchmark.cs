namespace Benchmark.Examples.BenchmarkCandidatesWithoutContext
{
  public static class StringConcatenationBenchmark
  {
    public static BenchmarkReport Run()
    {
      return Measure
        .Candidates<ConcatenateStringsCandidate, StringBuilderCandidate>()
        .WithNumberOfRuns(300)
        .WithNumberOfDryRuns(10)
        .Go();
    }
  }
}
