namespace Benchmark
{
  public class BenchmarkContextMetrics
  {
    public BenchmarkContextMetrics(IBenchmarkContext context, BenchmarkMetrics[] metrics)
    {
      Context = context;
      Metrics = metrics;
    }

    public IBenchmarkContext Context { get; }

    public BenchmarkMetrics[] Metrics { get; }
  }
}
