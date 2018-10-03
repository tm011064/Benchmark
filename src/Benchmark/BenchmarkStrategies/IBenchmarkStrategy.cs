namespace Benchmark.BenchmarkStrategies
{
  internal interface IBenchmarkStrategy<TContext>
    where TContext : class, IBenchmarkContext
  {
    BenchmarkResult Execute();
  }
}
