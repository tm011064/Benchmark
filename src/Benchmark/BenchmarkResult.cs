namespace Benchmark
{
  public class BenchmarkResult
  {
    public BenchmarkResult(IBenchmarkContext context, CandidateMetrics[] candidateMetrics)
    {
      Context = context;
      CandidateMetrics = candidateMetrics;
    }

    public IBenchmarkContext Context { get; }

    public CandidateMetrics[] CandidateMetrics { get; }
  }
}
