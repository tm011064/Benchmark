namespace Benchmark.BuilderSteps
{
  public interface IWithNumberOfDryRunsWithContextStep<TContext> : IGoStep<TContext>
    where TContext : class, IBenchmarkContext
  {
    IGoStep<TContext> WithNumberOfDryRuns(int numberOfRuns, TContext dryRunBenchmarkTestContext = null);
  }
}
