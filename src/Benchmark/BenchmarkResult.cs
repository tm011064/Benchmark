namespace Benchmark
{
  public class BenchmarkResult
  {
    public BenchmarkResult(ICandidateTestCase testCase, CandidateMetrics[] metrics)
    {
      TestCase = testCase;
      Metrics = metrics;
    }

    public ICandidateTestCase TestCase { get; }

    public CandidateMetrics[] Metrics { get; }
  }
}
