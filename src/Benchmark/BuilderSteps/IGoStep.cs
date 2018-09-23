namespace Benchmark.BuilderSteps
{
  public interface IGoStep<TContext>
    where TContext : class, IBenchmarkContext
  {
    BenchmarkReport Go();
  }

  public interface IGoStep
  {
    BenchmarkReport Go();
  }
}
