namespace Benchmark.BuilderSteps
{
  public interface IWithNumberOfRunsWithContextStep<TContext>
    where TContext : class, IBenchmarkContext
  {
    IWithNumberOfWarmUpRunsWithContextStep<TContext> WithNumberOfRuns(int numberOfRuns);
  }
}
