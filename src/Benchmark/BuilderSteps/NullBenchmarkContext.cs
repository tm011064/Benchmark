namespace Benchmark.BuilderSteps
{
  internal class NullBenchmarkContext : IBenchmarkContext
  {
    public static NullBenchmarkContext Instance { get; } = new NullBenchmarkContext();

    public string Description => string.Empty;
  }
}
