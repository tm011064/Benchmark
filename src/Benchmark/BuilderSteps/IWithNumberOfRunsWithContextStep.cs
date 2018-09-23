namespace Benchmark.BuilderSteps
{
  public interface IWithNumberOfRunsWithContextStep<TContext>
    where TContext : class, IBenchmarkContext
  {
    IWithNumberOfDryRunsWithContextStep<TContext> WithNumberOfRuns(int numberOfRuns);
  }
}
