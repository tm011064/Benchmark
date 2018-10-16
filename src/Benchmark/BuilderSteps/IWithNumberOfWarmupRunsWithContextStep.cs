namespace Benchmark.BuilderSteps
{
  public interface IWithNumberOfWarmUpRunsWithContextStep<TContext> : IGoStep<TContext>
    where TContext : class, IBenchmarkContext
  {
    IGoStep<TContext> NumberOfWarmUpRuns(int numberOfRuns, TContext WarmUpRunBenchmarkTestContext = null);
  }
}
